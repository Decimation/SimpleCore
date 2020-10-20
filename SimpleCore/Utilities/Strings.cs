using System;
using System.Linq;
using System.Text.RegularExpressions;
using SimpleCore.Internal;

// ReSharper disable UnusedMember.Global

namespace SimpleCore.Utilities
{
	/// <summary>
	///     Utilities for strings (<see cref="string" />).
	/// </summary>
	public static class Strings
	{
		public const string Alphanumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

		public static string SelectOnlyDigits(this string s)
		{
			string r = String.Empty;

			for (int i = 0; i < s.Length; i++) {
				if (Char.IsDigit(s[i])) {
					r += s[i];
				}
			}

			return r;

		}

		public static string CreateSeparator(string s)
		{
			string sx = new string('-', 10);
			return sx + s + sx;

		}

		public static string CleanString(string s)
		{
			s = s.Replace("\"", String.Empty);

			return s;
		}


		public static string Truncate(this string value, int maxLength)
		{
			if (String.IsNullOrEmpty(value)) {
				return value;
			}

			return value.Length <= maxLength ? value : value.Substring(0, maxLength);
		}

		/// <summary>Convert a word that is formatted in pascal case to have splits (by space) at each upper case letter.</summary>
		public static string SplitPascalCase(string convert)
		{
			return Regex.Replace(Regex.Replace(convert, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"),
				@"(\p{Ll})(\P{Ll})", "$1 $2");
		}

		public static string CreateRandom(int length)
		{
			return new string(Enumerable.Repeat(Alphanumeric, length)
				.Select(s => s[Common.RandomInstance.Next(s.Length)])
				.ToArray());
		}

		/// <summary>
		///     Simulates Java substring function
		/// </summary>
		public static string JSubstring(this string s, int beginIndex)
		{
			return s.Substring(beginIndex, s.Length);
		}

		/// <summary>
		///     Simulates Java substring function
		/// </summary>
		public static string JSubstring(this string s, int beginIndex, int endIndex)
		{
			return s.Substring(beginIndex, endIndex - beginIndex);
		}

		/// <summary>
		///     <returns>String value after [last] <paramref name="a" /></returns>
		/// </summary>
		public static string SubstringAfter(this string value, string a)
		{
			int posA = value.LastIndexOf(a, StringComparison.Ordinal);

			if (posA == Common.INVALID) {
				return String.Empty;
			}

			int adjustedPosA = posA + a.Length;
			return adjustedPosA >= value.Length ? String.Empty : value.Substring(adjustedPosA);
		}

		/// <summary>
		///     <returns>String value after [first] <paramref name="a" /></returns>
		/// </summary>
		public static string SubstringBefore(this string value, string a)
		{
			int posA = value.IndexOf(a, StringComparison.Ordinal);
			return posA == Common.INVALID ? String.Empty : value.Substring(0, posA);
		}

		/// <summary>
		///     <returns>String value between [first] <paramref name="a" /> and [last] <paramref name="b" /></returns>
		/// </summary>
		public static string SubstringBetween(this string value, string a, string b)
		{
			int posA = value.IndexOf(a, StringComparison.Ordinal);
			int posB = value.LastIndexOf(b, StringComparison.Ordinal);

			if (posA == Common.INVALID || posB == Common.INVALID) {
				return String.Empty;
			}


			int adjustedPosA = posA + a.Length;
			return adjustedPosA >= posB ? String.Empty : value.Substring(adjustedPosA, posB - adjustedPosA);
		}
	}
}