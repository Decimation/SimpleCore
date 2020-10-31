using System.Collections.Generic;
using System.Linq;
using System.Text;

// ReSharper disable UnusedMember.Global
#pragma warning disable HAA0601, HAA0502

namespace SimpleCore.Model
{
	/// <summary>
	/// Mutable extended string type.
	/// </summary>
	public class QString
	{
		private readonly StringBuilder m_value;

		public QString()
		{
			m_value = new StringBuilder();
		}

		public QString(StringBuilder value) : this()
		{
			Append(value.ToString());
		}

		public QString(string value) : this()
		{
			Append(value);
		}

		public char this[int i]
		{
			get => m_value[i];
			set => m_value[i] = value;
		}

		public QString this[int startIndex, int length]
		{
			get { return Value.Substring(startIndex, length); }
		}

		public void Clear() => m_value.Clear();

		public int Length => m_value.Length;

		public string Value
		{
			get => m_value.ToString();
			set
			{
				Clear();
				Append(value);
			}
		}

		public void Replace(QString oldValue, QString newValue)
		{
			m_value.Replace((string) oldValue, (string) newValue);
		}

		public void Remove(int startIndex, int length)
		{
			m_value.Remove(startIndex, length);
		}

		public void Append(string value)
		{
			m_value.Append(value);
		}

		public void Append(QString value)
		{
			m_value.Append(value);
		}

		public static QString operator +(QString a, QString b)
		{
			a.Append(b);
			return a;
		}

		public static /*implicit*/ explicit operator string(QString value) => value.Value;

		public static implicit operator QString(string value) => new QString(value);

		public QString Copy() => new QString(m_value);

		public override string ToString()
		{
			return Value;
		}
	}
}