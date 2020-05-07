using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SimpleCore.Utilities
{
	public static class Strings
	{
		private static readonly Random Random = new Random();

		public const string Alphanumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

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
			                            .Select(s => s[Random.Next(s.Length)]).ToArray());
		}
	}
}