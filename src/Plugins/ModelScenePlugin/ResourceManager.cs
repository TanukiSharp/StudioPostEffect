using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.IO;

namespace ModelScenePlugin
{
	/// <summary>
	/// Model container for cached model asset management.
	/// </summary>
	internal class ModelContainer
	{
		/// <summary>
		/// Gets or sets the absolute filename of the model.
		/// </summary>
		internal string Filename;
		/// <summary>
		/// Gets or sets the usage flag of the model.
		/// </summary>
		internal bool InUse;
		/// <summary>
		/// Gets or sets the model asset.
		/// </summary>
		internal Model Model;

		/// <summary>
		/// Instanciate a model container.
		/// </summary>
		/// <param name="filename">Absolute filename of the associated model.</param>
		/// <param name="model">Model asset.</param>
		internal ModelContainer(string filename, Model model)
		{
			Filename = filename;
			// default flag is set as 'used'
			InUse = true;
			Model = model;
		}
	}

	/// <summary>
	/// Cache management component for model assets.
	/// </summary>
	public static class ResourceManager
	{
		/// <summary>
		/// DirectX device instance.
		/// </summary>
		private static Device m_Device;
		/// <summary>
		/// Model collection.
		/// </summary>
		private static List<ModelContainer> m_Models = new List<ModelContainer>();
		/// <summary>
		/// Ability to use asset caching.
		/// </summary>
		private static bool m_UseCache;

		/// <summary>
		/// Initializes the ResourceManager.
		/// </summary>
		/// <param name="device">DirectX device instance.</param>
		public static void Initialize(Device device)
		{
			m_UseCache = true;
			m_Device = device;
		}

		/// <summary>
		/// Releases all the loaded model assets.
		/// </summary>
		public static void Clean()
		{
			// disposes all the models
			foreach (ModelContainer mc in m_Models)
				mc.Model.Dispose();
			// clears the model collection
			m_Models.Clear();
		}

		/// <summary>
		/// Gets or sets the ability to use the asset caching.
		/// </summary>
		public static bool UseCache
		{
			get
			{
				return (m_UseCache);
			}
			set
			{
				m_UseCache = value;
			}
		}

		/// <summary>
		/// Starts a 'garbage collecting' session.
		/// </summary>
		/// <remarks>
		/// Unset the InUse flag of all the ModelContainer instances in the model collection.
		/// When EndSession will be called, all the models which have not been updated will be automatically disposed.
		/// 
		/// You loads a scene 'A', which includes models 'M1', 'M2' and 'M3'.
		/// The StartSession method is called and does nothing because there is no model loaded.
		/// Then models 'M1', 'M2' and 'M3' are loaded into the cache through calls to GetModel method.
		/// The EndSession method is called and does nothing either since there is nothing to collect.
		/// 
		/// Now you want to change the scene and loads the 'B' one, which includes models 'M1', 'M3' and 'M4'.
		/// The StartSession is called and marks all the models in the cache as 'not used'.
		/// The models 'M1' and 'M3' are just retrieved from the cache and updated (marked) as 'used', the model 'M4' is loaded into the cache (through GetModel method).
		/// The EndSession method is called and the model 'M2' is disposed because it has not been used between the end of the previous session and the current new one.
		/// 
		/// This process speeds us the loading of scenes that often share same models, without slowing down loading of scene which share no models.
		/// The cost to pay for this optimization is an additional memory usage. It can become a problem if you switch from two different scenes which
		/// both use huge model data. For exemple, if the first session loads 600MB of models, and the second scene does the same,
		/// just before the second EndSession method call, the memory will be storing 1200MB of model data.
		/// 
		/// This can be desactivated from the Option tab panel.
		/// </remarks>
		/// <example>
		/// ResourceManager.StartSession();
		/// //...
		/// Model m1 = ResourceManager.GetModel(m1Filename);
		/// Model m2 = ResourceManager.GetModel(m2Filename);
		/// Model m3 = ResourceManager.GetModel(m3Filename);
		/// //...
		/// ResourceManager.EndSession();
		/// //...
		/// //...
		/// ResourceManager.StartSession();
		/// //...
		/// Model m1 = ResourceManager.GetModel(m1Filename);
		/// Model m3 = ResourceManager.GetModel(m3Filename);
		/// Model m4 = ResourceManager.GetModel(m4Filename);
		/// //...
		/// ResourceManager.EndSession();
		/// </example>
		/// <see cref="EndSession"/>
		/// <see cref="GetModel"/>
		/// <see cref="ModelContainer"/>
		public static void StartSession()
		{
			if (!m_UseCache)
			{
				// cleans the cache and skip other actions
				Clean();
			}
			else
			{
				// unset all 'InUse' flags
				foreach (ModelContainer mc in m_Models)
					mc.InUse = false;
			}
		}

		/// <summary>
		/// Ends the 'garbage collecting' session.
		/// </summary>
		/// <remarks>
		/// Automatically disposes all the models which haven't been requested from StartSession until then.
		/// 
		/// You loads a scene 'A', which includes models 'M1', 'M2' and 'M3'.
		/// The StartSession method is called and does nothing because there is no model loaded.
		/// Then models 'M1', 'M2' and 'M3' are loaded into the cache through calls to GetModel method.
		/// The EndSession method is called and does nothing either since there is nothing to collect.
		/// 
		/// Now you want to change the scene and loads the 'B' one, which includes models 'M1', 'M3' and 'M4'.
		/// The StartSession is called and marks all the models in the cache as 'not used'.
		/// The models 'M1' and 'M3' are just retrieved from the cache and updated (marked) as 'used', the model 'M4' is loaded into the cache (through GetModel method).
		/// The EndSession method is called and the model 'M2' is disposed because it has not been used between the end of the previous session and the current new one.
		/// 
		/// This process speeds us the loading of scenes that often share same models, without slowing down loading of scene which share no models.
		/// The cost to pay for this optimization is an additional memory usage. It can become a problem if you switch from two different scenes which
		/// both use huge model data. For exemple, if the first session loads 600MB of models, and the second scene does the same,
		/// just before the second EndSession method call, the memory will be storing 1200MB of model data.
		/// 
		/// This can be desactivated from the Option tab panel.
		/// </remarks>
		/// <example>
		/// ResourceManager.StartSession();
		/// //...
		/// Model m1 = ResourceManager.GetModel(m1Filename);
		/// Model m2 = ResourceManager.GetModel(m2Filename);
		/// Model m3 = ResourceManager.GetModel(m3Filename);
		/// //...
		/// ResourceManager.EndSession();
		/// //...
		/// //...
		/// ResourceManager.StartSession();
		/// //...
		/// Model m1 = ResourceManager.GetModel(m1Filename);
		/// Model m3 = ResourceManager.GetModel(m3Filename);
		/// Model m4 = ResourceManager.GetModel(m4Filename);
		/// //...
		/// ResourceManager.EndSession();
		/// </example>
		/// <see cref="StartSession"/>
		/// <see cref="GetModel"/>
		/// <see cref="ModelContainer"/>
		public static void EndSession()
		{
			if (!m_UseCache)
				return; // skip if cache is desactivated

			List<ModelContainer> removeList = new List<ModelContainer>();

			// iterates through all models and drop the unused one to the removal list
			foreach (ModelContainer mc in m_Models)
			{
				if (mc.InUse == false)
					removeList.Add(mc);
			}

			// disposes and removes all the unused models
			foreach (ModelContainer mc in removeList)
			{
				mc.Model.Dispose();
				m_Models.Remove(mc);
			}
		}

		/// <summary>
		/// Retrieves a previously loaded model from cache, or loads it to the cache and return.
		/// </summary>
		/// <remarks>
		/// This method updates the usage flag of the model, for 'garbage collecting' purpose.
		/// </remarks>
		/// <param name="filename">Absolute filename of the model to retrieve.</param>
		/// <returns>Returns a model instance.</returns>
		/// <see cref="StartSession"/>
		/// <see cref="EndSession"/>
		/// <see cref="ModelContainer"/>
		public static Model GetModel(string filename)
		{
			// check filename
			if (filename == null)
				return (null);

			// ensure to casing mistake
			filename = filename.ToLower();
			// look for existing model, identified by it's absoluate filename
			ModelContainer found = m_Models.Find(delegate(ModelContainer mc) { return (mc.Filename.ToLower() == filename); });
			if (found == null)
			{
				// cache fault, instanciates the model and adds it to the cache
				found = new ModelContainer(filename, new Model(m_Device, filename));
				m_Models.Add(found);
			}
			// updates the usage flag
			found.InUse = true;
			// returns the model asset
			return (found.Model);
		}
	}
}
