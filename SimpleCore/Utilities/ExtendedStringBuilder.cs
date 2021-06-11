#nullable enable
using System;
using System.Collections;
using System.Text;
// ReSharper disable UnusedMember.Global

namespace SimpleCore.Utilities
{
	public class ExtendedStringBuilder
	{
		public StringBuilder Builder { get; init; }

		//public Color? Primary   { get; init; }
		//public Color? Secondary { get; init; }

		public ExtendedStringBuilder() : this(new StringBuilder()) { }

		public ExtendedStringBuilder(StringBuilder builder)
		{
			Builder = builder;
		}

		public static implicit operator ExtendedStringBuilder(StringBuilder sb)
		{
			return new ExtendedStringBuilder(sb);
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

		public string IndentFields(string s) => Strings.Indent(s);


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


						//if (Primary.HasValue) {
						//	name = name.AddColor(Primary.Value);
						//}

						//if (Secondary.HasValue) {
						//	valStr = valStr.AddColor(Secondary.Value);
						//}

						string? fs = $"{name}: {valStr}".Truncate();

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