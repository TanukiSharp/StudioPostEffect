using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Xml;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SkyboxScenePlugin
{
	public enum Face
	{
		Front,
		Back,
		Top,
		Bottom,
		Right,
		Left
	}

	public class Skybox : IDisposable
	{
		private Device m_Device;
		private VertexBuffer[] m_VertexBuffer = new VertexBuffer[6];
		private Texture[] m_Textures = new Texture[6];
		private SkyboxFace[] m_Faces = new SkyboxFace[6];

		public Skybox(Device device, string xmlFile)
		{
			m_Device = device;

			ProcessXmlFile(xmlFile);

			FileSystemWatcher fsw = new FileSystemWatcher(Path.GetDirectoryName(xmlFile), Path.GetFileName(xmlFile));
			fsw.EnableRaisingEvents = true;
			fsw.IncludeSubdirectories = false;
			fsw.Changed += new FileSystemEventHandler(OnFileChanged);

			//m_Device.DeviceReset += new EventHandler(OnDeviceReset);
			//OnDeviceReset(m_Device, null);
		}

		private void ProcessXmlFile(string xmlFile)
		{
			string basePath = Path.GetDirectoryName(xmlFile);
			XmlDocument doc = new XmlDocument();
			doc.Load(xmlFile);
			foreach (XmlNode node in doc.DocumentElement.ChildNodes)
			{
				string name = node.Name.ToLower();
				switch (name)
				{
					case "front": m_Faces[0] = new SkyboxFace(basePath, node); break;
					case "back": m_Faces[1] = new SkyboxFace(basePath, node); break;
					case "top": m_Faces[2] = new SkyboxFace(basePath, node); break;
					case "bottom": m_Faces[3] = new SkyboxFace(basePath, node); break;
					case "right": m_Faces[4] = new SkyboxFace(basePath, node); break;
					case "left": m_Faces[5] = new SkyboxFace(basePath, node); break;
				}
			}
			doc = null;
			GC.Collect();
		}

		private bool m_Switch = false;
		private bool m_RequestReload = false;
		private void OnFileChanged(object sender, FileSystemEventArgs e)
		{
			// the switch mechanism is to avoid the problem of double call
			// of the OnFileChanged event when file is modified
			if (m_Switch == false)
				m_Switch = true;
			else
			{
				ProcessXmlFile(e.FullPath);
				m_RequestReload = true;
				m_Switch = false;
			}
		}

		internal void OnDeviceReset(object sender, EventArgs e)
		{
			CreateVertexBuffer();
			ReloadTextures();
		}

		private void DisposeVertexBuffers()
		{
			for (int i = 0; i < 6; i++)
			{
				if (m_VertexBuffer[i] != null)
					m_VertexBuffer[i].Dispose();
			}
		}

		private void DisposeTextures()
		{
			for (int i = 0; i < 6; i++)
			{
				if (m_Textures[i] != null)
					m_Textures[i].Dispose();
			}
		}

		public void Dispose()
		{
			DisposeVertexBuffers();
			DisposeTextures();
		}

		private void CreateVertexBuffer()
		{
			DisposeVertexBuffers();

			RefPoint n;
			RefPoint[] c;
			CustomVertex.PositionTextured[] vertices = new CustomVertex.PositionTextured[6];

			// front
			c = m_Faces[0].Coords;
			n = c[uv00]; vertices[0] = new CustomVertex.PositionTextured(-1.0f, +1.0f, -1.0f, n.X, n.Y);
			n = c[uv01]; vertices[1] = new CustomVertex.PositionTextured(-1.0f, -1.0f, -1.0f, n.X, n.Y);
			n = c[uv11]; vertices[2] = new CustomVertex.PositionTextured(+1.0f, -1.0f, -1.0f, n.X, n.Y);
			n = c[uv00]; vertices[3] = new CustomVertex.PositionTextured(-1.0f, +1.0f, -1.0f, n.X, n.Y);
			n = c[uv11]; vertices[4] = new CustomVertex.PositionTextured(+1.0f, -1.0f, -1.0f, n.X, n.Y);
			n = c[uv10]; vertices[5] = new CustomVertex.PositionTextured(+1.0f, +1.0f, -1.0f, n.X, n.Y);

			m_VertexBuffer[0] = new VertexBuffer(typeof(CustomVertex.PositionTextured), vertices.Length, m_Device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionTextured.Format, Pool.Default);
			m_VertexBuffer[0].SetData(vertices, 0, LockFlags.None);


			// back
			c = m_Faces[1].Coords;
			n = c[uv00]; vertices[0] = new CustomVertex.PositionTextured(+1.0f, +1.0f, +1.0f, n.X, n.Y);
			n = c[uv01]; vertices[1] = new CustomVertex.PositionTextured(+1.0f, -1.0f, +1.0f, n.X, n.Y);
			n = c[uv11]; vertices[2] = new CustomVertex.PositionTextured(-1.0f, -1.0f, +1.0f, n.X, n.Y);
			n = c[uv00]; vertices[3] = new CustomVertex.PositionTextured(+1.0f, +1.0f, +1.0f, n.X, n.Y);
			n = c[uv11]; vertices[4] = new CustomVertex.PositionTextured(-1.0f, -1.0f, +1.0f, n.X, n.Y);
			n = c[uv10]; vertices[5] = new CustomVertex.PositionTextured(-1.0f, +1.0f, +1.0f, n.X, n.Y);
			
			m_VertexBuffer[1] = new VertexBuffer(typeof(CustomVertex.PositionTextured), vertices.Length, m_Device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionTextured.Format, Pool.Default);
			m_VertexBuffer[1].SetData(vertices, 0, LockFlags.None);


			// top
			c = m_Faces[2].Coords;
			n = c[uv00]; vertices[0] = new CustomVertex.PositionTextured(-1.0f, +1.0f, +1.0f, n.X, n.Y);
			n = c[uv01]; vertices[1] = new CustomVertex.PositionTextured(-1.0f, +1.0f, -1.0f, n.X, n.Y);
			n = c[uv11]; vertices[2] = new CustomVertex.PositionTextured(+1.0f, +1.0f, -1.0f, n.X, n.Y);
			n = c[uv00]; vertices[3] = new CustomVertex.PositionTextured(-1.0f, +1.0f, +1.0f, n.X, n.Y);
			n = c[uv11]; vertices[4] = new CustomVertex.PositionTextured(+1.0f, +1.0f, -1.0f, n.X, n.Y);
			n = c[uv10]; vertices[5] = new CustomVertex.PositionTextured(+1.0f, +1.0f, +1.0f, n.X, n.Y);

			m_VertexBuffer[2] = new VertexBuffer(typeof(CustomVertex.PositionTextured), vertices.Length, m_Device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionTextured.Format, Pool.Default);
			m_VertexBuffer[2].SetData(vertices, 0, LockFlags.None);


			// bottom
			c = m_Faces[3].Coords;
			n = c[uv00]; vertices[0] = new CustomVertex.PositionTextured(-1.0f, -1.0f, -1.0f, n.X, n.Y);
			n = c[uv01]; vertices[1] = new CustomVertex.PositionTextured(-1.0f, -1.0f, +1.0f, n.X, n.Y);
			n = c[uv11]; vertices[2] = new CustomVertex.PositionTextured(+1.0f, -1.0f, +1.0f, n.X, n.Y);
			n = c[uv00]; vertices[3] = new CustomVertex.PositionTextured(-1.0f, -1.0f, -1.0f, n.X, n.Y);
			n = c[uv11]; vertices[4] = new CustomVertex.PositionTextured(+1.0f, -1.0f, +1.0f, n.X, n.Y);
			n = c[uv10]; vertices[5] = new CustomVertex.PositionTextured(+1.0f, -1.0f, -1.0f, n.X, n.Y);

			m_VertexBuffer[3] = new VertexBuffer(typeof(CustomVertex.PositionTextured), vertices.Length, m_Device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionTextured.Format, Pool.Default);
			m_VertexBuffer[3].SetData(vertices, 0, LockFlags.None);


			// right
			c = m_Faces[4].Coords;
			n = c[uv00]; vertices[0] = new CustomVertex.PositionTextured(+1.0f, +1.0f, -1.0f, n.X, n.Y);
			n = c[uv01]; vertices[1] = new CustomVertex.PositionTextured(+1.0f, -1.0f, -1.0f, n.X, n.Y);
			n = c[uv11]; vertices[2] = new CustomVertex.PositionTextured(+1.0f, -1.0f, +1.0f, n.X, n.Y);
			n = c[uv00]; vertices[3] = new CustomVertex.PositionTextured(+1.0f, +1.0f, -1.0f, n.X, n.Y);
			n = c[uv11]; vertices[4] = new CustomVertex.PositionTextured(+1.0f, -1.0f, +1.0f, n.X, n.Y);
			n = c[uv10]; vertices[5] = new CustomVertex.PositionTextured(+1.0f, +1.0f, +1.0f, n.X, n.Y);

			m_VertexBuffer[4] = new VertexBuffer(typeof(CustomVertex.PositionTextured), vertices.Length, m_Device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionTextured.Format, Pool.Default);
			m_VertexBuffer[4].SetData(vertices, 0, LockFlags.None);


			// left
			c = m_Faces[5].Coords;
			n = c[uv00]; vertices[0] = new CustomVertex.PositionTextured(-1.0f, +1.0f, +1.0f, n.X, n.Y);
			n = c[uv01]; vertices[1] = new CustomVertex.PositionTextured(-1.0f, -1.0f, +1.0f, n.X, n.Y);
			n = c[uv11]; vertices[2] = new CustomVertex.PositionTextured(-1.0f, -1.0f, -1.0f, n.X, n.Y);
			n = c[uv00]; vertices[3] = new CustomVertex.PositionTextured(-1.0f, +1.0f, +1.0f, n.X, n.Y);
			n = c[uv11]; vertices[4] = new CustomVertex.PositionTextured(-1.0f, -1.0f, -1.0f, n.X, n.Y);
			n = c[uv10]; vertices[5] = new CustomVertex.PositionTextured(-1.0f, +1.0f, -1.0f, n.X, n.Y);

			m_VertexBuffer[5] = new VertexBuffer(typeof(CustomVertex.PositionTextured), vertices.Length, m_Device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionTextured.Format, Pool.Default);
			m_VertexBuffer[5].SetData(vertices, 0, LockFlags.None);
		}

		private void ReloadTextures()
		{
			DisposeTextures();

			for (int i = 0; i < 6; i++)
			{
				//m_Textures[i] = Texture.FromBitmap(m_Device, (Bitmap)Bitmap.FromFile(m_Faces[i].TextureFilename, false), Usage.Dynamic, Pool.Default);
				//m_Textures[i] = TextureLoader.FromFile(m_Device, m_Faces[i].TextureFilename);
				m_Textures[i] = TextureLoader.FromFile(m_Device, m_Faces[i].TextureFilename, 0, 0, 1, Usage.Dynamic, Format.Unknown, Pool.Default, Filter.Linear, Filter.None, 0);
			}
		}

		private float GetX0()
		{
			return (0.0f);
		}

		private float GetX1()
		{
			return (1.0f);
		}

		private float GetY0()
		{
			return (0.0f);
		}

		private float GetY1()
		{
			return (1.0f);
		}


		public void Draw()
		{
			if (m_RequestReload)
			{
				CreateVertexBuffer();
				m_RequestReload = false;
			}

			m_Device.VertexFormat = CustomVertex.PositionTextured.Format;
			for (int i = 0; i < 6; i++)
			{
				m_Device.SetTexture(0, m_Textures[i]);
				m_Device.SetStreamSource(0, m_VertexBuffer[i], 0);
				m_Device.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
			}
		}

		public const int uv00 = 0;
		public const int uv01 = 1;
		public const int uv11 = 2;
		public const int uv10 = 3;
	}

	internal class SkyboxFace
	{
		private RefPoint[] m_Coords = new RefPoint[4];
		private string m_TextureFilename;

		public string TextureFilename
		{
			get
			{
				return (m_TextureFilename);
			}
		}

		public RefPoint[] Coords
		{
			get
			{
				return (m_Coords);
			}
		}

		public SkyboxFace(string basePath, XmlNode node)
		{
			basePath = basePath.TrimEnd('\\');

			m_Coords[Skybox.uv00] = new RefPoint(0, 0);
			m_Coords[Skybox.uv01] = new RefPoint(0, 1);
			m_Coords[Skybox.uv11] = new RefPoint(1, 1);
			m_Coords[Skybox.uv10] = new RefPoint(1, 0);

			foreach (XmlAttribute attr in node.Attributes)
			{
				string attrName = attr.Name.ToLower();
				if (attrName == "texture" || attrName == "tex")
					m_TextureFilename = string.Format("{0}\\{1}", basePath, attr.Value.TrimStart('\\'));
				else if (attrName == "flipx" && ParseBool(attr.Value))
				{
					FlipX();
				}
				else if (attrName == "flipy" && ParseBool(attr.Value))
				{
					FlipY();
				}
				else if (attrName == "rotation" || attrName == "rotate" || attrName == "rot")
				{
					int rotation = ParseInt(attr.Value);
					for (int i = 0; i < rotation; i++)
						Rotate();
				}
			}
		}

		private int ParseInt(string value)
		{
			value = value.Trim();

			int test;
			if (int.TryParse(value, out test) == false)
				return (0);
			return (test % 4);
		}

		private bool ParseBool(string value)
		{
			value = value.Trim().ToLower();

			int test;
			if (int.TryParse(value, out test))
				return (test != 0);

			if (value == "true" || value == "yes")
				return (true);

			return (false);
		}

		private void FlipX()
		{
			RefPoint tmp = m_Coords[0];
			m_Coords[0] = m_Coords[3];
			m_Coords[3] = tmp;

			tmp = m_Coords[1];
			m_Coords[1] = m_Coords[2];
			m_Coords[2] = tmp;
		}

		private void FlipY()
		{
			RefPoint tmp = m_Coords[0];
			m_Coords[0] = m_Coords[1];
			m_Coords[1] = tmp;

			tmp = m_Coords[3];
			m_Coords[3] = m_Coords[2];
			m_Coords[2] = tmp;
		}

		private void Rotate()
		{
			RefPoint tmp = m_Coords[3];
			m_Coords[3] = m_Coords[2];
			m_Coords[2] = m_Coords[1];
			m_Coords[1] = m_Coords[0];
			m_Coords[0] = tmp;
		}
	}

	internal class RefPoint
	{
		public int X;
		public int Y;

		public RefPoint(int x, int y)
		{
			X = x;
			Y = y;
		}

		public override string ToString()
		{
			return (string.Format("{0}, {1}", X, Y));
		}
	}
}
