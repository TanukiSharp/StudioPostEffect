using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace InternalEffect
{
	public class CustomEffect : CustomBaseElement
	{
		private Device m_Device;
		private string m_RelativeEffectFilename;
		private string m_EffectFilename;

		private bool m_MarkToReload;
		private bool m_MarkToRebuild;
		public delegate void ShaderBuildHandler(string value, bool request);
		public event ShaderBuildHandler OnShaderRebuid;
		public event EventHandler Reloaded;

		private CustomTechnique[] m_Techniques;
		private CustomParameter[] m_Parameters;

		private ShaderFlags m_ShaderFlags;

		public CustomEffect(Device device, string effectFilename, string relativeEffectFilename)
			: base(null)
		{
			m_Device = device;
			m_RelativeEffectFilename = relativeEffectFilename;
			m_EffectFilename = effectFilename;

			m_ShaderFlags = ShaderFlags.SkipValidation;
			m_Name = Path.GetFileNameWithoutExtension(effectFilename);

			InternalReloadEffect();
		}

		public string Filename
		{
			get
			{
				return (m_EffectFilename);
			}
		}

		public string RelativeEffectFilename
		{
			get
			{
				return (m_RelativeEffectFilename);
			}
		}

		public void MarkToReload()
		{
			m_MarkToReload = true;
		}

		private string m_ShaderCode;
		private bool m_ShaderRebuildParam;
		public void MarkToRebuild(string shaderCode, bool shaderRebuildParam)
		{
			m_ShaderCode = shaderCode;
			m_ShaderRebuildParam = shaderRebuildParam;
			m_MarkToRebuild = true;
		}

		public bool CheckEffect()
		{
			if (m_MarkToRebuild)
			{
				string errors;

				try
				{
					Effect.FromString(m_Device, m_ShaderCode, null, m_ShaderFlags, null, out errors);
				}
				catch (Exception ex)
				{
					errors = string.Format("Unexpected error: {0}", ex.Message);
				}
				if (OnShaderRebuid != null)
					OnShaderRebuid(errors, m_ShaderRebuildParam);
				m_MarkToRebuild = false;
				return (true);
			}

			if (m_MarkToReload)
			{
				InternalReloadEffect();
				m_MarkToReload = false;
				return (true);
			}
			return (false);
		}


		private void InternalReloadEffect()
		{
			//List<string> checkLossList = BuildLossList();

			if (m_Effect != null)
				m_Effect.Dispose();

			string errors;
			m_Effect = Effect.FromFile(m_Device, m_EffectFilename, null, m_ShaderFlags, null, out errors);
			if (m_Effect == null)
				throw new InvalidProgramException(errors);

			if (m_Effect == null)
				throw new Exception("Impossible to load effect");

			CreateFromEffect(m_Effect);

			//List<string> newCheckLossList = BuildLossList();
			//foreach (string str in newCheckLossList)
			//    checkLossList.Remove(str);
			//return (checkLossList);
		}

		private List<string> BuildParamList()
		{
			List<string> prms = new List<string>();

			if (m_Parameters != null)
			{
				foreach (CustomParameter pr in m_Parameters)
					prms.Add(pr.Name);
			}

			return (prms);
		}

		private List<string> BuildLossList()
		{
			List<string> checkLoss = new List<string>();

			if (m_Techniques != null)
			{
				foreach (CustomTechnique t in m_Techniques)
				{
					checkLoss.Add(string.Format("tech_{0}", t.Name));
					foreach (CustomPass ps in t.Passes)
						checkLoss.Add(string.Format("pass_{0}", ps.Name));
				}
			}
			if (m_Parameters != null)
			{
				foreach (CustomParameter pr in m_Parameters)
					checkLoss.Add(string.Format("prmt_{0}", pr.Name));
			}

			return (checkLoss);
		}

		private CustomParameter m_PixelSize;
		private CustomParameter m_RenderTargetSize;
		private CustomParameter m_RandomSeed;

		private void CreateFromEffect(Effect effect)
		{
			if (effect == null)
				throw new ArgumentException("null parameter", "effect");
			m_Handle = null;

			List<CustomTechnique> customTechs = new List<CustomTechnique>();
			for (int i = 0; i < m_Effect.Description.Techniques; i++)
			{
				EffectHandle techHandle = m_Effect.GetTechnique(i);
				CustomTechnique tech = new CustomTechnique(m_Effect, techHandle, this);
				customTechs.Add(tech);
			}
			m_Techniques = customTechs.ToArray();

			List<CustomParameter> customParams = new List<CustomParameter>();
			for (int i = 0; i < m_Effect.Description.Parameters; i++)
			{
				EffectHandle paramHandle = m_Effect.GetParameter(null, i);
				customParams.Add(new CustomParameter(m_Effect, paramHandle, this));
			}
			m_Parameters = customParams.ToArray();

			EffectHandle pixSizeHandle = m_Effect.GetParameter(null, "PIXEL_SIZE");
			if (pixSizeHandle != null)
				m_PixelSize = new CustomParameter(m_Effect, pixSizeHandle, this);

			EffectHandle rtSizeHandle = m_Effect.GetParameter(null, "RENDER_TARGET_SIZE");
			if (rtSizeHandle != null)
				m_RenderTargetSize = new CustomParameter(m_Effect, rtSizeHandle, this);

			EffectHandle rtRandomSeedHandle = m_Effect.GetParameter(null, "RANDOMSEED");
			if (rtRandomSeedHandle != null)
				m_RandomSeed = new CustomParameter(m_Effect, rtRandomSeedHandle, this);

			m_Effect = effect;

			if (Reloaded != null)
				Reloaded(this, new EventArgs());
		}

		public CustomBaseElement Find(string xpath)
		{
			string[] path = xpath.Split('/', '\\');

			if (path.Length > 0)
			{
				CustomTechnique tech = FindTechnique(path[0]);
				if (path.Length > 1)
				{
					CustomPass pass = FindPass(tech, path[1]);
					if (path.Length > 2)
						return (null);
					else
						return (pass);
				}
				else
				{
					if (tech == null)
					{
						// couldn't find the technique, let's search for parameter
						CustomParameter prm = FindParameter(path[0]);
						return (prm);
					}
					return (tech);
				}
			}
			else
			{
				return (null);
			}
		}

		public CustomPass FindPass(CustomTechnique technique, string name)
		{
			if (technique == null)
				return (null);
			return (technique.FindPass(name));
		}

		public CustomTechnique FindTechnique(string name)
		{
			return (FindElement(m_Techniques, name) as CustomTechnique);
		}

		public CustomParameter FindParameter(string name)
		{
			return (FindElement(m_Parameters, name) as CustomParameter);
		}

		internal static CustomBaseElement FindElement(CustomBaseElement[] elements, string name)
		{
			string nameToLower = name.ToLower();
			foreach (CustomBaseElement elem in elements)
			{
				if (elem.Name.ToLower() == nameToLower)
					return (elem);
			}
			return (null);
		}

		public CustomTechnique[] Techniques
		{
			get
			{
				return (m_Techniques);
			}
		}

		public CustomParameter[] Parameters
		{
			get
			{
				return (m_Parameters);
			}
		}

		public CustomParameter PixelSize
		{
			get
			{
				return (m_PixelSize);
			}
		}

		public CustomParameter RenderTargetSize
		{
			get
			{
				return (m_RenderTargetSize);
			}
		}

		public CustomParameter RandomSeed
		{
			get
			{
				return (m_RandomSeed);
			}
		}
	}
}
