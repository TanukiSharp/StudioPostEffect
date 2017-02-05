using System;
using System.Collections.Generic;
using System.Text;

namespace InternalEffect
{
	public static class ControlNaming
	{
		private static Dictionary<Type, byte[]> m_Names = new Dictionary<Type, byte[]>();

		public static string GetNextName<T>()
		{
			byte[] letters;
			if (m_Names.TryGetValue(typeof(T), out letters) == false)
			{
				letters = new byte[3];
				m_Names.Add(typeof(T), letters);
			}

			string name = string.Format("{0}_{1}{2}{3}", typeof(T).Name, (char)(letters[2] + 'A'), (char)(letters[1] + 'A'), (char)(letters[0] + 'A'));

			letters[0]++;
			if (letters[0] >= 26)
			{
				letters[0] = 0;
				letters[1]++;
				if (letters[1] >= 26)
				{
					letters[1] = 0;
					letters[2]++;
					if (letters[2] >= 26)
						throw new Exception("Maximum name capacity reached");
				}
			}

			return (name);
		}
	}
}
