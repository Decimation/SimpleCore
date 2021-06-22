#nullable enable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using SimpleCore.Model;
using static SimpleCore.Internal.Common;

// ReSharper disable UnusedMember.Local

// ReSharper disable StringIndexOfIsCultureSpecific.1

// ReSharper disable UnusedMember.Global


namespace SimpleCore.Utilities
{
	[Flags]
	public enum HexOptions
	{
		None = 0,

		Prefix = 1,

		Lowercase = 1 << 1,

		Default = Prefix
	}

	/// <summary>
	///     Utilities for strings (<see cref="string" />).
	/// </summary>
	public static class Strings
	{
		public static string SelectOnlyDigits(this string s) => s.SelectOnly(Char.IsDigit);

		public static string SelectOnly(this string s, Func<char, bool> fn)
		{
			return s.Where(fn).Aggregate(String.Empty, (current, t) => current + t);
		}


		public static string CleanString(this string s)
		{
			//return s.Replace("\"", String.Empty);

			return s.Trim('\"');
		}

		public static string Truncate(this string value)
		{
			//return value.Truncate(Console.WindowWidth - 5);
			return value.Truncate(100);
		}

		public static string Truncate(this string value, int maxLength)
		{
			if (String.IsNullOrEmpty(value)) {
				return value;
			}

			return value.Length <= maxLength ? value : value[..maxLength];
		}

		public static string? NullIfNullOrWhiteSpace(string? str)
		{
			return String.IsNullOrWhiteSpace(str) ? null : str;

		}

		public static bool StringWraps(string s)
		{
			/*
			 * Assuming buffer width equals window width
			 *
			 * If 'Wrap text output on resize' is ticked, this is true
			 */

			return s.Length >= Console.WindowWidth;
		}

		/// <summary>Convert a word that is formatted in pascal case to have splits (by space) at each upper case letter.</summary>
		public static string SplitPascalCase(string convert)
		{
			return Regex.Replace(Regex.Replace(convert, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"),
			                     @"(\p{Ll})(\P{Ll})", "$1 $2");
		}

		public static string CreateRandom(int length)
		{
			return new(Enumerable.Repeat(StringConstants.Alphanumeric, length)
			                     .Select(s => s[RandomInstance.Next(s.Length)])
			                     .ToArray());
		}

		public static IEnumerable<int> AllIndexesOf(this string str, string search)
		{
			int minIndex = str.IndexOf(search);

			while (minIndex != -1) {
				yield return minIndex;
				minIndex = str.IndexOf(search, minIndex + search.Length, StringComparison.Ordinal);
			}
		}

		#region Substring

		/// <summary>
		///     Simulates Java substring function
		/// </summary>
		public static string JSubstring(this string s, int beginIndex) => s[beginIndex..];

		/// <summary>
		///     Simulates Java substring function
		/// </summary>
		public static string JSubstring(this string s, int beginIndex, int endIndex) =>
			s.Substring(beginIndex, endIndex - beginIndex + 1);

		/// <summary>
		///     Simulates Java substring function
		/// </summary>
		public static string JSubstring(this string s, Range r) => s.JSubstring(r.Start.Value, r.End.Value);

		/// <summary>
		///     Simulates Java substring function
		/// </summary>
		public static string JSubstring(this string s, Index i) => s.JSubstring(i.Value);


		/// <summary>
		///     <returns>String value after [last] <paramref name="a" /></returns>
		/// </summary>
		public static string SubstringAfter(this string value, string a)
		{
			int posA = value.LastIndexOf(a, StringComparison.Ordinal);

			if (posA == INVALID) {
				return String.Empty;
			}

			int adjustedPosA = posA + a.Length;
			return adjustedPosA >= value.Length ? String.Empty : value[adjustedPosA..];
		}

		/// <summary>
		///     <returns>String value after [first] <paramref name="a" /></returns>
		/// </summary>
		public static string SubstringBefore(this string value, string a)
		{
			int posA = value.IndexOf(a, StringComparison.Ordinal);
			return posA == INVALID ? String.Empty : value[..posA];
		}

		/// <summary>
		///     <returns>String value between [first] <paramref name="a" /> and [last] <paramref name="b" /></returns>
		/// </summary>
		public static string SubstringBetween(this string value, string a, string b)
		{
			int posA = value.IndexOf(a, StringComparison.Ordinal);
			int posB = value.LastIndexOf(b, StringComparison.Ordinal);

			if (posA == INVALID || posB == INVALID) {
				return String.Empty;
			}

			int adjustedPosA = posA + a.Length;
			return adjustedPosA >= posB ? String.Empty : value.Substring(adjustedPosA, posB - adjustedPosA);
		}

		#endregion

		public static string RemoveLastOccurrence(this string s, string s2) =>
			s.Remove(s.LastIndexOf(s2, StringComparison.Ordinal));

		/// <summary>
		///     Compute the Levenshtein distance (approximate string matching) between <paramref name="s"/> and <paramref name="t"/>
		/// </summary>
		public static int Compute(string s, string t)
		{
			int    n = s.Length;
			int    m = t.Length;
			int[,] d = new int[n + 1, m + 1];

			// Step 1
			if (n == 0)
				return m;

			if (m == 0)
				return n;

			// Step 2
			for (int i = 0; i <= n; d[i, 0] = i++) { }

			for (int j = 0; j <= m; d[0, j] = j++) { }

			// Step 3
			for (int i = 1; i <= n; i++) //Step 4
			for (int j = 1; j <= m; j++) {
				// Step 5
				int cost = t[j - 1] == s[i - 1] ? 0 : 1;

				// Step 6
				d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
			}

			// Step 7
			return d[n, m];
		}


		public static string Separator { get; set; } = new('-', 20);

		public static string Indentation { get; set; } = new(' ', 5);

		public static string Indent(string s) => Indent(s, Indentation);

		public static string Indent(string s, string indent)
		{
			//return s.Replace("\n", "\n" + Indent);

			string[] split = s.Split('\n');

			string j = String.Join($"\n{indent}", split);

			return indent + j;
		}


		public static string ViewString(IViewable view)
		{
			var esb = new ExtendedStringBuilder();


			foreach (var (key, value) in view.View) {
				switch (value) {
					case null:
						continue;
					case IViewable view2:
						esb.Append(ViewString(view2));
						break;
					default:
						esb.Append(key, value);
						break;
				}

			}

			return esb.ToString();
		}


		#region Hex

		public const string HEX_FORMAT_SPECIFIER = "X";

		public const string HEX_PREFIX = "0x";

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

		#endregion

		#region Join

		public static string FormatJoin<T>(this IEnumerable<T> values,
		                                   string format, IFormatProvider? provider = null,
		                                   string delim = StringConstants.JOIN_COMMA) where T : IFormattable =>
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
		                                 Func<T, string> toString, string delim = StringConstants.JOIN_COMMA) =>
			String.Join(delim, values.Select(toString));

		public static string QuickJoin<T>(this IEnumerable<T> enumerable, string delim = StringConstants.JOIN_COMMA) =>
			String.Join(delim, enumerable);

		public static string SimpleJoin<T>(this IEnumerable<T> values, string delim = StringConstants.JOIN_COMMA) =>
			String.Join(delim, values);

		#endregion

		public static string ToString<T>(T[] rg)
		{
			if (rg is byte[] byteArray) {
				return byteArray.FormatJoin(HEX_FORMAT_SPECIFIER);
			}

			return rg.SimpleJoin();
		}
	}
}