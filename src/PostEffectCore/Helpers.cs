using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;

namespace PostEffectCore
{
	#region IO Helper

	public static class IOHelper
	{
		public static bool TryCreateDirectory(string path)
		{
			try
			{
				Directory.CreateDirectory(path);
				return (true);
			}
			catch
			{
				return (false);
			}
		}

		public static string GetProductVersion()
		{
			string[] ver = Application.ProductVersion.Split('.');
			if (ver.Length != 4)
				return (Application.ProductVersion);
			return (string.Format("{0}.{1}.{2}", ver[0], ver[1], ver[3]));
		}
	}

	#endregion

	#region String Helper

	public static class StringHelper
	{
		public static int StrCmp(string str1, string str2, bool ignoreCase)
		{
			if (str1 == null || str2 == null || str1.Length == 0 || str2.Length == 0)
				return (-1);

			string str1_tmp = null;
			string str2_tmp = null;

			if (ignoreCase)
			{
				str1_tmp = str1.ToLower();
				str2_tmp = str2.ToLower();
			}
			else
			{
				str1_tmp = str1;
				str2_tmp = str2;
			}

			int len = Math.Min(str1_tmp.Length, str2_tmp.Length);

			for (int i = 0; i < len; i++)
			{
				if (str1_tmp[i] != str2_tmp[i])
					return (i);
			}
			return (len);
		}

		public static bool TryParseBool(string str, out bool result)
		{
			result = false;

			if (str == null)
				return (false);

			str = str.Trim().ToLower();
			if (str.Length == 0)
				return (false);

			if (str == "true")
			{
				result = true;
				return (true);
			}
			else if (str == "false")
			{
				result = false;
				return (true);
			}
			else
			{
				long val;
				if (long.TryParse(str, out val) == false)
					return (false);

				result = (val != 0);
				return (true);
			}
		}

		public static float[] ToFloatArray(string[] array)
		{
			float[] result = new float[array.Length];
			for (int i = 0; i < result.Length; i++)
				result[i] = float.Parse(array[i]);
			return (result);
		}

		public static int[] ToIntegerArray(string[] array)
		{
			int[] result = new int[array.Length];
			for (int i = 0; i < result.Length; i++)
				result[i] = int.Parse(array[i]);
			return (result);
		}

		public static bool[] ToBooleanArray(string[] array)
		{
			bool[] result = new bool[array.Length];
			for (int i = 0; i < result.Length; i++)
				result[i] = (int.Parse(array[i]) != 0);
			return (result);
		}

		public static string Capitalize(string str)
		{
			if (str == null)
				return ("");
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < str.Length; i++)
			{
				if (i == 0 || char.IsWhiteSpace(str, i - 1))
					sb.Append(char.ToUpper(str[i]));
				else
					sb.Append(str[i]);
			}
			return (sb.ToString());
		}

		public static string RemoveWhiteSpaces(string str)
		{
			StringBuilder sb = new StringBuilder();
			foreach (char c in str)
			{
				if (char.IsWhiteSpace(c) == false)
					sb.Append(c);
			}
			return (sb.ToString());
		}
	}

	#endregion

	#region XML Helper

	public static class XmlHelper
	{
		public static int GetNodeAttributeValueInt(XmlNode node, string attrName)
		{
			string value = GetNodeAttributeValue(node, attrName);
			int result;
			if (int.TryParse(value, out result) == false)
				throw new FormatException(string.Format("Attribute '{0}' in the xml node '{1}' is not a proper integer value.", attrName, GetNodeFullPath(node)));
			return (result);
		}

		public static bool GetNodeAttributeValueBoolean(XmlNode node, string attrName)
		{
			string value = GetNodeAttributeValue(node, attrName);
			value = value.ToLower();

			if (value == "true")
				return (true);
			if (value == "false")
				return (false);

			int result;
			if (int.TryParse(value, out result) == false || result < 0 || result > 1)
				throw new FormatException(string.Format("Attribute '{0}' in the xml node '{1}' is not a proper boolean value.", attrName, GetNodeFullPath(node)));

			return (result == 1);
		}

		public static string GetNodeAttributeValue(XmlNode node, string attrName)
		{
			XmlAttribute attr = node.Attributes[attrName];
			if (attr == null)
				throw new FormatException(string.Format("Can't find '{0}' attribute in the xml node '{1}'.", attrName, GetNodeFullPath(node)));
			if (attr.Value.Trim().Length == 0)
				throw new FormatException(string.Format("Attribute '{0}' in the xml node '{1}' must be set.", attrName, GetNodeFullPath(node)));
			return (attr.Value);
		}

		public static XmlNode GetNode(XmlDocument doc, string xpath)
		{
			XmlNode node = doc.SelectSingleNode(xpath);
			if (node == null)
				throw new FormatException(string.Format("Can't find xml node '{0}'.", xpath));
			return (node);
		}

		public static string GetNodeFullPath(XmlNode node)
		{
			if (node.ParentNode == null)
				return (node.Name);
			return (string.Format("{0}/{1}", GetNodeFullPath(node.ParentNode), node.Name));
		}
	}

	#endregion

	#region DirectX Helper

	public static class DirectXHelper
	{
		public static bool CheckForMultiSampling(Device device, out MultiSampleType maxSampleType, out int maxQuality)
		{
			maxSampleType = MultiSampleType.None;
			maxQuality = 0;

			int result;
			int quality;

			for (int i = 16; i >= 2; i--)
			{
				bool available = Manager.CheckDeviceMultiSampleType(device.CreationParameters.AdapterOrdinal, device.CreationParameters.DeviceType,
					device.PresentationParameters.BackBufferFormat, true, (MultiSampleType)i, out result, out quality);

				if (available)
				{
					maxSampleType = (MultiSampleType)i;
					maxQuality = quality - 1;
					return (true);
				}
			}
			return (false);
		}
	}

	#endregion
}
