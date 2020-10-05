using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SimpleCore.Internal;

// ReSharper disable UnusedMember.Global

namespace SimpleCore.Utilities
{
	/// <summary>
	/// Utilities for strings (<see cref="string"/>).
	/// </summary>
	public static class Strings
	{
		public const string Alphanumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

		public static string SelectOnlyDigits(this string s)
		{
			string r = string.Empty;

			for (int i = 0; i < s.Length; i++) {
				if (char.IsDigit(s[i])) {
					r += s[i];
				}
			}

			return r;

		}

		public static string CreateSeparator(string s)
		{
			var sx = new string('-', 10);
			return sx + s + sx;

		}

		public static string CleanString(string s)
		{
			s = s.Replace("\"", String.Empty);

			return s;
		}

		public static string CreateRandomName() => Path.GetFileNameWithoutExtension(Path.GetRandomFileName());


		public static string Truncate(this string value, int maxLength)
		{
			if (String.IsNullOrEmpty(value)) return value;
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
				.Select(s => s[Common.Random.Next(s.Length)])
				.ToArray());
		}
	}
}