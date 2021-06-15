#nullable enable
using System;
using System.Collections;
using System.Text;
using SimpleCore.Utilities;

// ReSharper disable UnusedMember.Global

namespace SimpleCore.Model
{
	public class ExtendedStringBuilder
	{
		public StringBuilder Builder { get; init; }


		public ExtendedStringBuilder() : this(new StringBuilder()) { }

		public ExtendedStringBuilder(StringBuilder builder)
		{
			Builder = builder;
		}

		public static implicit operator ExtendedStringBuilder(StringBuilder sb)
		{
			return new(sb);
		}

		public ExtendedStringBuilder Append(string value)
		{
			Builder.Append(value);
			return this;
		}

		public ExtendedStringBuilder AppendLine(string value)
		{
			Builder.AppendLine(value);
			return this;
		}

		public override string ToString() => Builder.ToString();


		public ExtendedStringBuilder Append(string name, object? val, string? valStr = null, bool newLine = true)
		{
			if (val != null) {

				// Patterns are so epic

				switch (val) {
					case IList {Count: 0}:
					case string s when String.IsNullOrWhiteSpace(s):
						return this;

					default:
					{
						valStr ??= val.ToString();


						string fs = $"{name}: {valStr}".Truncate();

						if (newLine) {
							fs += "\n";
						}

						Builder.Append(fs);
						break;
					}
				}

			}

			return this;
		}
	}
}