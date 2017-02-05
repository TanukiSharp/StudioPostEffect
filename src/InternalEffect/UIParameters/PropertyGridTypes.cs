using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.IO;
using System.Globalization;
using System.ComponentModel.Design.Serialization;
using HDRColorPicker;
using Microsoft.DirectX.Direct3D;

namespace InternalEffect
{
	public interface IParameterUI
	{
		string Name { get; }
		ParameterType ParameterType { get; }
		string GetValues();
		bool SetValues(string values);
		string GetMinimums();
		bool SetMinimums(string values);
		string GetMaximums();
		bool SetMaximums(string values);
	}




	// TypeDescriptor.GetProperties(component)["PropertyName"].SetValue(component, newValue)




	internal class BooleanConverter : ExpandableObjectConverter
	{
		private static TypeConverter.StandardValuesCollection m_BooleanValues;

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return ((destinationType == typeof(InstanceDescriptor)) || base.CanConvertTo(context, destinationType));
		}


		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is UIParameter)
				return (null);

			if (destinationType == typeof(string))
			{
				if (value is BooleanSelector)
				{
					BooleanSelector bsl = (BooleanSelector)value;
					return (bsl.Value.ToString());
				}
			}

			return (base.ConvertTo(context, culture, value, destinationType));
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string str = value as string;
			if (str == null)
				return (base.ConvertFrom(context, culture, value));

			str = str.Trim().ToLower();
			if (str.Length == 0)
				return (null);

			Type type = context.PropertyDescriptor.PropertyType;

			if (type == typeof(BooleanSelector))
			{
				bool val;
				if (bool.TryParse(str, out val) == false)
					throw new FormatException();
				return (val);
			}

			return (null);
		}


		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (m_BooleanValues == null)
				m_BooleanValues = new TypeConverter.StandardValuesCollection(new object[] { true, false });
			return (m_BooleanValues);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return (true);
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return (true);
		}

		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return (false);
		}
	}




	internal class NonExpandableConverter : ExpandableObjectConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return ((destinationType == typeof(InstanceDescriptor)) || base.CanConvertTo(context, destinationType));
		}


		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is UIParameter)
				return (null);

			if (destinationType == typeof(string))
			{
				if (value is ColorSelector)
				{
					ColorSelector csl = (ColorSelector)value;
					return (string.Format("{0}; {1}; {2}; {3}",
						(int)(csl.Value.R * 255.0),
						(int)(csl.Value.G * 255.0),
						(int)(csl.Value.B * 255.0),
						(int)(csl.Value.A * 255.0)));
				}
				/*
				else if (value is MousePadSelector)
				{
					MousePadSelector mpsl = (MousePadSelector)value;
					return (string.Format("{0}; {1}", mpsl.Value[0], mpsl.Value[1]));
				}
				*/
			}

			return (base.ConvertTo(context, culture, value, destinationType));
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string str = value as string;
			if (str == null)
				return (base.ConvertFrom(context, culture, value));

			str = str.Trim().ToLower();
			if (str.Length == 0)
				return (null);

			Type type = context.PropertyDescriptor.PropertyType;

			if (type == typeof(ColorSelector))
			{
				string[] vals = str.Split(';');
				if (vals.Length != 4)
					throw new FormatException();
				int[] tmp = new int[4];
				for (int i = 0; i < 4; i++)
				{
					if (int.TryParse(vals[i], out tmp[i]) == false)
						throw new FormatException();
				}
				return (new ColorF((float)tmp[3] / 255.0f, (float)tmp[0] / 255.0f, (float)tmp[1] / 255.0f, (float)tmp[2] / 255.0f));
			}
			/*
			else if (type == typeof(MousePadSelector))
			{
				string[] vals = str.Split(';');
				if (vals.Length != 2)
					throw new FormatException();
				int[] tmp = new int[2];
				for (int i = 0; i < 2; i++)
				{
					if (int.TryParse(vals[i], out tmp[i]) == false)
						throw new FormatException();
				}
				return (new float[] { tmp[0], tmp[1] });
			}
			*/

			return (null);
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return (true);
		}

		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return (false);
		}
	}










	internal class CustomObjectConverter : ExpandableObjectConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return ((destinationType == typeof(InstanceDescriptor)) || base.CanConvertTo(context, destinationType));
		}


		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is UIParameter)
				return (null);

			if (destinationType == typeof(string))
			{
				if (value is FloatSlider)
				{
					FloatSlider fsl = (FloatSlider)value;
					return (fsl.Value.ToString());
				}
				else if (value is IntegerSlider)
				{
					IntegerSlider isl = (IntegerSlider)value;
					return (isl.Value.ToString());
				}
			}

			return (base.ConvertTo(context, culture, value, destinationType));
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string str = value as string;
			if (str == null)
				return (base.ConvertFrom(context, culture, value));

			str = str.Trim().ToLower();
			if (str.Length == 0)
				return (null);

			Type type = context.PropertyDescriptor.PropertyType;

			if (type == typeof(FloatSlider))
			{
				float val;
				if (float.TryParse(str, out val) == false)
					throw new FormatException();
				return (val);
			}
			else if (type == typeof(IntegerSlider))
			{
				int val;
				if (int.TryParse(str, out val) == false)
					throw new FormatException();
				return (val);
			}
			else if (type == typeof(ColorSelector))
			{
				string[] vals = str.Split(';');
				if (vals.Length != 4)
					throw new FormatException();
				int[] tmp = new int[4];
				for (int i = 0; i < 4; i++)
				{
					if (int.TryParse(vals[i], out tmp[i]) == false)
						throw new FormatException();
				}
				return (new ColorF((float)tmp[3] / 255.0f, (float)tmp[0] / 255.0f, (float)tmp[1] / 255.0f, (float)tmp[2] / 255.0f));
			}

			return (null);
		}
	}








	public class NotEditablePropertyCollection<T> : PropertyDescriptor, IList<T>, ICustomTypeDescriptor where T : IParameterUI
	{
		private List<T> m_List = new List<T>();

		public NotEditablePropertyCollection()
			: base(Path.GetRandomFileName(), new Attribute[0])
		{
		}

		#region ICustomTypeDescriptor Members

		public PropertyDescriptorCollection GetProperties()
		{
			PropertyDescriptorCollection pdc = new PropertyDescriptorCollection(null);

			for (int i = 0; i < this.Count; i++)
			{
				CustomPropertyDescriptor<T> pd = new CustomPropertyDescriptor<T>(this, i);
				pdc.Add(pd);
			}
			return (pdc);
		}

		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			return (GetProperties());
		}

		#region Useless Methods

		public AttributeCollection GetAttributes()
		{
			return (TypeDescriptor.GetAttributes(this, true));
		}

		public string GetClassName()
		{
			return (TypeDescriptor.GetClassName(this, true));
		}

		public string GetComponentName()
		{
			return (TypeDescriptor.GetComponentName(this, true));
		}

		public TypeConverter GetConverter()
		{
			return (TypeDescriptor.GetConverter(this, true));
		}

		public EventDescriptor GetDefaultEvent()
		{
			return (TypeDescriptor.GetDefaultEvent(this, true));
		}

		public PropertyDescriptor GetDefaultProperty()
		{
			return (TypeDescriptor.GetDefaultProperty(this, true));
		}

		public new object GetEditor(Type editorBaseType)
		{
			return (TypeDescriptor.GetEditor(this, editorBaseType, true));
		}

		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return (TypeDescriptor.GetEvents(this, attributes, true));
		}

		public EventDescriptorCollection GetEvents()
		{
			return (TypeDescriptor.GetEvents(this, true));
		}

		public object GetPropertyOwner(PropertyDescriptor pd)
		{
			return (this);
		}

		#endregion

		#endregion

		public void AddRange(IEnumerable<T> collection)
		{
			m_List.AddRange(collection);
		}

		#region IList<T> Members

		public int IndexOf(T item)
		{
			return (m_List.IndexOf(item));
		}

		public void Insert(int index, T item)
		{
			m_List.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			m_List.RemoveAt(index);
		}

		public T this[int index]
		{
			get
			{
				return (m_List[index]);
			}
			set
			{
				m_List[index] = value;
			}
		}

		#endregion

		#region ICollection<T> Members

		public void Add(T item)
		{
			m_List.Add(item);
		}

		public void Clear()
		{
			m_List.Clear();
		}

		public bool Contains(T item)
		{
			return (m_List.Contains(item));
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			m_List.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get
			{
				return (m_List.Count);
			}
		}

		bool ICollection<T>.IsReadOnly
		{
		    get
		    {
		        return (((ICollection<T>)m_List).IsReadOnly);
		    }
		}

		public bool Remove(T item)
		{
			return (m_List.Remove(item));
		}

		#endregion

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			return (m_List.GetEnumerator());
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (m_List.GetEnumerator());
		}

		#endregion

		public override bool IsReadOnly
		{
			get
			{
				return (false);
			}
		}

		/*
		public override string DisplayName
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				foreach (T item in m_List)
				{
					if (typeof(T) == typeof(FloatSlider))
						sb.AppendFormat("{0:.2f}, ", ((item as FloatSlider).Value));
					else if (typeof(T) == typeof(NotEditablePropertyCollection<T>))
						sb.AppendFormat("{0}, ", (item as NotEditablePropertyCollection<T>).DisplayName);
				}

				sb.Remove(sb.Length - 1, 1);
				return (sb.ToString());
			}
		}
		*/

		public override bool CanResetValue(object component)
		{
			return (false);
		}

		public override Type ComponentType
		{
			get
			{
				return (m_List.GetType());
			}
		}

		public override object GetValue(object component)
		{
			return (m_List);
		}

		public override Type PropertyType
		{
			get
			{
				return (typeof(T));
			}
		}

		public override void ResetValue(object component)
		{
		}

		public override void SetValue(object component, object value)
		{
		}

		public override bool ShouldSerializeValue(object component)
		{
			return (true);
		}
	}




	public class CustomPropertyDescriptor<T> : PropertyDescriptor where T : IParameterUI
	{
		private NotEditablePropertyCollection<T> m_Collection = null;
		private int m_Index = -1;

		public CustomPropertyDescriptor(NotEditablePropertyCollection<T> collection, int idx)
			: base(string.Format("#{0}", idx), null)
		{
			m_Collection = collection;
			m_Index = idx;
		}

		public override bool SupportsChangeEvents
		{
			get
			{
				return (true);
			}
		}

		public override AttributeCollection Attributes
		{
			get
			{
				return (new AttributeCollection(null));
			}
		}

		public override bool CanResetValue(object component)
		{
			return (false);
		}

		public override Type ComponentType
		{
			get
			{
				return (m_Collection.GetType());
			}
		}

		public override string DisplayName
		{
			get
			{
				if (m_Index >= m_Collection.Count)
					return ("");

				return (((IParameterUI)m_Collection[m_Index]).Name);
			}
		}

		public override string Description
		{
			get
			{
				return (null);
			}
		}

		public override object GetValue(object component)
		{
			if (m_Index >= m_Collection.Count)
				return (null);

			return (m_Collection[m_Index]);
		}

		public override bool IsReadOnly
		{
			get
			{
				return (false);
			}
		}

		public override string Name
		{
			get
			{
				//return (((INamable)m_Collection[m_Index]).Name);
				return ("");
			}
		}

		public override Type PropertyType
		{
			get
			{
				if (m_Index >= m_Collection.Count)
					return (null);

				//if (typeof(T) == typeof(FloatSlider))
				//	return (typeof(float));

				return (m_Collection[m_Index].GetType());
			}
		}

		public override void ResetValue(object component) { }

		public override bool ShouldSerializeValue(object component)
		{
			return (true);
		}

		public override void SetValue(object component, object value)
		{
			if (m_Collection[m_Index] is FloatSlider)
				(m_Collection[m_Index] as FloatSlider).Value = (float)value;
			else if (m_Collection[m_Index] is IntegerSlider)
				(m_Collection[m_Index] as IntegerSlider).Value = (int)value;
			else if (m_Collection[m_Index] is BooleanSelector)
				(m_Collection[m_Index] as BooleanSelector).Value = (bool)value;
			else if (m_Collection[m_Index] is ColorSelector)
				(m_Collection[m_Index] as ColorSelector).Value = (ColorF)value;
			/*
			else if (m_Collection[m_Index] is MousePadSelector)
				(m_Collection[m_Index] as MousePadSelector).Value = (float[])value;
			*/

			//m_Collection[m_Index] = (Effect)value;
		}
	}
}
