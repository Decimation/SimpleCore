#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using static SimpleCore.Internal.Common;

// ReSharper disable UnusedMember.Local

// ReSharper disable StringIndexOfIsCultureSpecific.1

// ReSharper disable UnusedMember.Global


namespace SimpleCore.Utilities
{
	/// <summary>
	///     Utilities for strings (<see cref="string" />).
	/// </summary>
	public static class Strings
	{
		public const string Alphanumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

		/// <summary>
		/// Constant <see cref="string.Empty"/>
		/// </summary>
		public const string Empty = "";


		public static string SelectOnlyDigits(this string s) => s.SelectOnly(Char.IsDigit);

		public static string SelectOnly(this string s, Func<char, bool> fn)
		{

			return s.Where(fn).Aggregate(String.Empty, (current, t) => current + t);
		}

		

		public static string CleanString(this string s)
		{

			return s.Replace("\"", String.Empty);
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
			return new(Enumerable.Repeat(Alphanumeric, length)
			                     .Select(s => s[RandomInstance.Next(s.Length)])
			                     .ToArray());
		}

		public static IEnumerable<int> AllIndexesOf(this string str, string searchstring)
		{
			int minIndex = str.IndexOf(searchstring);

			while (minIndex != -1) {
				yield return minIndex;
				minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
			}
		}

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
			return posA == INVALID ? String.Empty : value.Substring(0, posA);
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

		public static string RemoveLastOccurrence(this string s, string s2) =>
			s.Remove(s.LastIndexOf(s2, StringComparison.Ordinal));

		/// <summary>
		///     Compute the Levenshtein distance (approximate string matching) between <paramref name="s"/> and <paramref name="t"/>
		/// </summary>
		/// 
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


		public static string Separator { get; set; } = new string('-', 20);

		public static string Indent { get; set; } = new string(' ', 5);

		public static string IndentFields(string s) => IndentFields(s, Indent );

		public static string IndentFields(string s, string indent)
		{
			//return s.Replace("\n", "\n" + Indent);

			var split = s.Split('\n');

			var j = string.Join($"\n{indent}", split);

			return indent + j;
		}
	}

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

		public string IndentFields(string s) => Strings.IndentFields(s);


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