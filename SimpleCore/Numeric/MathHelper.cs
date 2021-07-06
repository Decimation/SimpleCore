using System;
using System.Linq.Expressions;
using BE = System.Linq.Expressions.BinaryExpression;
using PE = System.Linq.Expressions.ParameterExpression;

// ReSharper disable FieldCanBeMadeReadOnly.Local

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
			string result = $"{len:0.##} {Sizes[order]}";


			return result;
		}

		private static readonly string[] Sizes = {"B", "KB", "MB", "GB", "TB"};

		public static T Add<T>(T a, T b) => MathImplementation<T>.Add(a, b);

		public static T Subtract<T>(T a, T b) => MathImplementation<T>.Sub(a, b);

		public static T Multiply<T>(T a, T b) => MathImplementation<T>.Mul(a, b);

		public static T Divide<T>(T a, T b) => MathImplementation<T>.Div(a, b);

		private static class MathImplementation<T>
		{
			internal static readonly Func<T, T, T> Add;
			internal static readonly Func<T, T, T> Sub;
			internal static readonly Func<T, T, T> Mul;
			internal static readonly Func<T, T, T> Div;

			static MathImplementation()
			{
				Add = Create(Expression.Add);
				Sub = Create(Expression.Subtract);
				Mul = Create(Expression.Multiply);
				Div = Create(Expression.Divide);
			}

			private static Func<T, T, T> Create(Func<ParameterExpression, ParameterExpression, BinaryExpression> fx)
			{
				var paramA = Expression.Parameter(typeof(T));
				var paramB = Expression.Parameter(typeof(T));
				var body   = fx(paramA, paramB);
				return Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();

			}
		}

		public static bool IsPrime(int number)
		{
			switch (number) {
				case <= 1:
					return false;
				case 2:
					return true;
			}

			if (number % 2 == 0)
				return false;

			int boundary = (int) Math.Floor(Math.Sqrt(number));

			for (int i = 3; i <= boundary; i += 2)
				if (number % i == 0)
					return false;

			return true;

			/*
			 *	| Method   |     Mean |     Error |    StdDev |
				|---------:|---------:|----------:|----------:|
				| IsPrime  | 2.098 ns | 0.0211 ns | 0.0187 ns |
				| IsPrime2 | 2.568 ns | 0.0074 ns | 0.0061 ns |
			 */

			/*
			 *	if (number == 1) return false;
				if (number == 2) return true;

				double limit = Math.Ceiling(Math.Sqrt(number)); //hoisting the loop limit

				for (int i = 2; i <= limit; ++i)
					if (number % i == 0)
						return false;
				return true;
			 */

		}

		public static float Distance(byte[] first, byte[] second)
		{
			int sum = 0;

			// We'll use which ever array is shorter.
			int length = first.Length > second.Length ? second.Length : first.Length;

			for (int x = 0; x < length; x++) {
				sum += (int) Math.Pow((first[x] - second[x]), 2);
			}

			return sum / (float) length;
		}
	}
}