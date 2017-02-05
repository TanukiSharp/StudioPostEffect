using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.IO;
using Microsoft.DirectX;
using System.Drawing;

namespace ModelScenePlugin
{
	/// <summary>
	/// Represent a model.
	/// </summary>
	public class Model : IDisposable
	{
		/// <summary>
		/// DirectX device instance.
		/// </summary>
		private Device m_Device;
		/// <summary>
		/// Absolute filename of the DirectX mesh file.
		/// </summary>
		private string m_Filename;
		/// <summary>
		/// DirectX Mesh instance.
		/// </summary>
		private Mesh m_Mesh;
		/// <summary>
		/// Materials (colors, textures, etc...) of the model.
		/// </summary>
		private ExtendedMaterial[] m_ExtendedMaterials;

		/// <summary>
		/// DirectX-ready model materials.
		/// </summary>
		private Material[] m_Materials;
		/// <summary>
		/// DirectX-ready model textures.
		/// </summary>
		private Texture[] m_Textures;

		/// <summary>
		/// Instance a model asset.
		/// </summary>
		/// <param name="device">DirectX device instance.</param>
		/// <param name="filename">Absolure filename of the model to load.</param>
		public Model(Device device, string filename)
		{
			m_Device = device;
			m_Filename = filename;

			// hook the DeviceReset event for data auto-reload
			m_Device.DeviceReset += new EventHandler(OnDeviceReset);

			OnDeviceReset(m_Device, null);
		}

		/// <summary>
		/// Happens when the DirectX device is lost and reset.
		/// </summary>
		/// <param name="sender">Component that raised the event (DirectX device).</param>
		/// <param name="e">Base .NET event.</param>
		private void OnDeviceReset(object sender, EventArgs e)
		{
			// dispose the previous loaded data to ensure no memory-leaks
			Dispose();

			// loads mesh data and retrieve the extended materials at once
			m_Mesh = Mesh.FromFile(m_Filename, MeshFlags.Dynamic, m_Device, out m_ExtendedMaterials);
			try
			{
				// try to recompute the normals (sometime it fails and throw an exception
				m_Mesh.ComputeNormals();
			}
			catch { }

			// prepare materials and textures arrays
			m_Textures = new Texture[m_ExtendedMaterials.Length];
			m_Materials = new Material[m_ExtendedMaterials.Length];

			// iterate through the mesh extended materials to extract the material and texture information
			for (int i = 0; i < m_ExtendedMaterials.Length; i++)
			{
				ExtendedMaterial extmat = m_ExtendedMaterials[i];

				// check if texture is defined
				if (extmat.TextureFilename != null)
				{
					// create texture filename according to the mesh file location
					string textureFilename = Path.Combine(Path.GetDirectoryName(m_Filename), extmat.TextureFilename);
					// check if the texture file exists
					if (File.Exists(textureFilename))
					{
						// loads texture with no mipmapping and linear texture filter
						m_Textures[i] = TextureLoader.FromFile(m_Device, textureFilename, 0, 0, 1, Usage.Dynamic, Format.Unknown, Pool.Default, Filter.Linear, Filter.None, 0);
					}
				}

				// set material
				m_Materials[i] = extmat.Material3D;
				// override ambient material using diffuse one
				m_Materials[i].Ambient = m_Materials[i].Diffuse;
			}
		}

		/// <summary>
		/// Renders the model.
		/// </summary>
		public void Render()
		{
			// ensure model is not disposed (or disposing)
			if (m_Mesh.Disposed)
				return;

			// ensure lighting is on
			m_Device.RenderState.Lighting = true;
			// set black material to global ambient light
			m_Device.RenderState.Ambient = Color.Black;

			// set texture and material to use for the current mesh subset render
			for (int i = 0; i < m_ExtendedMaterials.Length; i++)
			{
				// set texture
				m_Device.SetTexture(0, m_Textures[i]);
				// set material
				m_Device.Material = m_Materials[i];
				// renders
				m_Mesh.DrawSubset(i);
			}
		}

		/// <summary>
		/// Releases the allocated data.
		/// </summary>
		public void Dispose()
		{
			if (m_Textures != null)
			{
				for (int i = 0; i < m_ExtendedMaterials.Length; i++)
				{
					if (m_Textures[i] != null)
						m_Textures[i].Dispose(); // dispose loaded texture
					m_Textures[i] = null;
				}
			}

			// dispose loaded mesh
			if (m_Mesh != null)
				m_Mesh.Dispose();
		}

		/// <summary>
		/// Releases the allocated data.
		/// </summary>
		void IDisposable.Dispose()
		{
			Dispose();
		}
	}
}
