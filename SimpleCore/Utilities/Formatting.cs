using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable enable
// ReSharper disable UnusedMember.Global

// ReSharper disable ParameterTypeCanBeEnumerable.Global

// ReSharper disable StringCompareToIsCultureSpecific

namespace SimpleCore.Utilities
{
	// https://github.com/khalidabuhakmeh/ConsoleTables

	/// <summary>
	/// Utilities for formatting.
	/// </summary>
	public static class Formatting
	{
		public const char SPACE = ' ';

		public const char PERIOD = '.';

		public const string JOIN_COMMA = ", ";

		public const string JOIN_SPACE = " ";

		/// <summary>
		/// Scope resolution operator
		/// </summary>
		public const string JOIN_SCOPE = "::";


		/// <summary>
		/// Concatenates the strings returned by <paramref name="toString"/>
		/// using the specified separator between each element or member.
		/// </summary>
		/// <param name="values">Collection of values</param>
		/// <param name="toString">Function which returns a <see cref="string"/> given a member of <paramref name="values"/></param>
		/// <param name="delim">Delimiter</param>
		/// <typeparam name="T">Element type</typeparam>
		public static string FuncJoin<T>(this IEnumerable<T> values,
			Func<T, string> toString, string delim = JOIN_COMMA)
		{
			return String.Join(delim, values.Select(toString));
		}

		public static string FormatJoin<T>(this IEnumerable<T> values, string format,
			IFormatProvider? provider = null, string delim = JOIN_COMMA)
			where T : IFormattable
		{
			return String.Join(delim, values.Select(v => v.ToString(format, provider)));
		}

		public static string SimpleJoin<T>(this IEnumerable<T> values, string delim = JOIN_COMMA) => String.Join(delim, values);

		public static string ToString<T>(T[] rg)
		{
			if (typeof(T) == typeof(byte)) {
				var byteArray = rg as byte[];
				return FormatJoin(byteArray!, HEX_FORMAT_SPECIFIER);
			}

			return SimpleJoin(rg);
		}

		private const string HEX_FORMAT_SPECIFIER = "X";

		private const string HEX_PREFIX = "0x";

		public static string QuickJoin<T>(this IEnumerable<T> enumerable, string delim = ", ") => String.Join(delim, enumerable);

		public static string ToHexString<T>(T value, HexOptions options = HexOptions.Default)
		{
			var sb = new StringBuilder();

			if (options.HasFlagFast(HexOptions.Prefix)) {
				sb.Append(HEX_PREFIX);
			}

			string? hexStr = null;

			if (value is IFormattable fmt) {
				hexStr = fmt.ToString(HEX_FORMAT_SPECIFIER, null);
			}
			else {
				throw new NotImplementedException();
			}


			if (options.HasFlagFast(HexOptions.Lowercase)) {
				hexStr = hexStr.ToLower();
			}

			sb.Append(hexStr);

			return sb.ToString();
		}

		public static unsafe string ToHexString(void* value, HexOptions options = HexOptions.Default) => ToHexString((long) value, options);

		public static unsafe string ToHexString(IntPtr value, HexOptions options = HexOptions.Default) => ToHexString((long) value, options);
	}


	[Flags]
	public enum HexOptions
	{
		None = 0,

		Prefix = 1,

		Lowercase = 1 << 1,

		Default = Prefix
	}
}