using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable enable
// ReSharper disable UnusedMember.Global
// ReSharper disable ParameterTypeCanBeEnumerable.Global
// ReSharper disable StringCompareToIsCultureSpecific
#pragma warning disable HAA0302, HAA0303, HAA0301, HAA0502

namespace SimpleCore.Utilities
{
	// https://github.com/khalidabuhakmeh/ConsoleTables

	[Flags]
	public enum HexOptions
	{
		None = 0,

		Prefix = 1,

		Lowercase = 1 << 1,

		Default = Prefix
	}

	/// <summary>
	///     Utilities for formatting.
	/// </summary>
	public static class Formatting
	{
		public const string JOIN_COMMA = ", ";

		/// <summary>
		///     Scope resolution operator
		/// </summary>
		public const string JOIN_SCOPE = "::";

		public const string JOIN_SPACE = " ";


		private const string HEX_FORMAT_SPECIFIER = "X";

		private const string HEX_PREFIX = "0x";

		public const char   PERIOD      = '.';
		public const char   ASTERISK    = '*';
		public const char   EXCLAMATION = '!';
		public const char   SPACE       = ' ';
		public const string ELLIPSES    = "...";


		public const char ARROW_DOWN       = '\u2193';
		public const char ARROW_LEFT       = '\u2190';
		public const char ARROW_LEFT_RIGHT = '\u2194';
		public const char ARROW_RIGHT      = '\u2192';
		public const char ARROW_UP         = '\u2191';
		public const char ARROW_UP_DOWN    = '\u2195';


		public const char BALLOT_X         = '\u2717';
		public const char HEAVY_BALLOT_X   = '\u2718';
		public const char CHECK_MARK       = '\u2713';
		public const char HEAVY_CHECK_MARK = '\u2714';
		public const char LOZENGE          = '\u25ca';
		public const char MUL_SIGN         = '\u00D7';
		public const char NULL_CHAR        = '\0';
		public const char RAD_SIGN         = '\u221A';
		public const char RELOAD           = '\u21bb';
		public const char SUN              = '\u263c';

		public static string FormatJoin<T>(this IEnumerable<T> values,
			string format, IFormatProvider? provider = null, string delim = JOIN_COMMA) where T : IFormattable =>
			String.Join(delim, values.Select(v => v.ToString(format, provider)));

		/// <summary>
		///     Concatenates the strings returned by <paramref name="toString" />
		///     using the specified separator between each element or member.
		/// </summary>
		/// <param name="values">Collection of values</param>
		/// <param name="toString">
		///     Function which returns a <see cref="string" /> given a member of <paramref name="values" />
		/// </param>
		/// <param name="delim">Delimiter</param>
		/// <typeparam name="T">Element type</typeparam>
		public static string FuncJoin<T>(this IEnumerable<T> values,
			Func<T, string> toString, string delim = JOIN_COMMA) =>
			String.Join(delim, values.Select(toString));

		public static string QuickJoin<T>(this IEnumerable<T> enumerable, string delim = JOIN_COMMA) =>
			String.Join(delim, enumerable);

		public static string SimpleJoin<T>(this IEnumerable<T> values, string delim = JOIN_COMMA) =>
			String.Join(delim, values);

		public static string ToHexString<T>(T value, HexOptions options = HexOptions.Default)
		{
			var sb = new StringBuilder();

			if (options.HasFlagFast(HexOptions.Prefix)) {
				sb.Append(HEX_PREFIX);
			}

			string? hexStr;

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

		public static unsafe string ToHexString(void* value, HexOptions options = HexOptions.Default) =>
			ToHexString((long) value, options);

		public static string ToHexString(IntPtr value, HexOptions options = HexOptions.Default) =>
			ToHexString((long) value, options);

		public static string ToString<T>(T[] rg)
		{
			if (typeof(T) == typeof(byte)) {
				var byteArray = rg as byte[];
				return FormatJoin(byteArray!, HEX_FORMAT_SPECIFIER);
			}

			return SimpleJoin(rg);
		}
	}
}