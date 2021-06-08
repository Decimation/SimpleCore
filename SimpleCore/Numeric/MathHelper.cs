using System;
using System.Linq;

// ReSharper disable UnusedMember.Global

namespace SimpleCore.Numeric
{
	public enum MetricUnit
	{
		// Bit,
		// Byte,

		Kilo = 1,
		Mega,
		Giga,
		Tera,
		Peta,
		Exa,
		Zetta,
		Yotta
	}

	public static class MathHelper
	{
		/// <summary>
		/// SI
		/// </summary>
		public const double MAGNITUDE = 1000D;

		/// <summary>
		/// ISO/IEC 80000
		/// </summary>
		public const double MAGNITUDE2 = 1024D;

		/// <summary>
		/// Convert the given bytes to <see cref="MetricUnit"/>
		/// </summary>
		/// <param name="bytes">Value in bytes to be converted</param>
		/// <param name="type">Unit to convert to</param>
		/// <returns>Converted bytes</returns>
		public static double ConvertToUnit(double bytes, MetricUnit type)
		{
			// var rg  = new[] { "k","M","G","T","P","E","Z","Y"};
			// var pow = rg.ToList().IndexOf(type) +1;


			int pow = (int) type;
			var v   = bytes / Math.Pow(MAGNITUDE, pow);


			return v;
		}

		private static readonly string[] Sizes = {"B", "KB", "MB", "GB", "TB"};

		public static string ConvertToUnit(double len)
		{
			//https://stackoverflow.com/questions/281640/how-do-i-get-a-human-readable-file-size-in-bytes-abbreviation-using-net


			int order = 0;

			while (len >= MAGNITUDE2 && order < Sizes.Length - 1) {
				order++;
				len /= MAGNITUDE2;
			}

			// Adjust the format string to your preferences. For example "{0:0.#}{1}" would
			// show a single decimal place, and no space.
			string result = String.Format("{0:0.##} {1}", len, Sizes[order]);


			return result;
		}

		public static bool IsPrime(int number)
		{
			if (number     <= 1) return false;
			if (number     == 2) return true;
			if (number % 2 == 0) return false;

			var boundary = (int) Math.Floor(Math.Sqrt(number));

			for (int i = 3; i <= boundary; i += 2)
				if (number % i == 0)
					return false;

			return true;
		}
	}
}