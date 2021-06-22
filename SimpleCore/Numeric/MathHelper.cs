﻿using System;
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

		public static T Add<T>(T a, T b) => MathImplementation<T>.AddFunction(a, b);

		public static T Subtract<T>(T a, T b) => MathImplementation<T>.SubtractFunction(a, b);

		public static T Multiply<T>(T a, T b) => MathImplementation<T>.MultiplyFunction(a, b);

		public static T Divide<T>(T a, T b) => MathImplementation<T>.DivideFunction(a, b);

		private static class MathImplementation<T>
		{
			private static Func<T, T, T> add;
			private static Func<T, T, T> sub;
			private static Func<T, T, T> mul;
			private static Func<T, T, T> div;

			static MathImplementation()
			{
				add = Create(Expression.Add);
				sub = Create(Expression.Subtract);
				mul = Create(Expression.Multiply);
				div = Create(Expression.Divide);
			}
			private static Func<T, T, T> Create(Func<PE, PE, BE> fx)
			{
				var paramA = Expression.Parameter(typeof(T));
				var paramB = Expression.Parameter(typeof(T));
				var body   = fx(paramA, paramB);
				return Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();

			}

			internal static Func<T, T, T> AddFunction = (a, b) =>
			{
				//var paramA = Expression.Parameter(typeof(T));
				//var paramB = Expression.Parameter(typeof(T));
				//var body   = Expression.Add(paramA, paramB);
				//AddFunction = Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();
				//return AddFunction(a, b);

				return add(a, b);
			};

			internal static Func<T, T, T> SubtractFunction = (a, b) =>
			{
				//var paramA = Expression.Parameter(typeof(T));
				//var paramB = Expression.Parameter(typeof(T));
				//var body   = Expression.Subtract(paramA, paramB);
				//SubtractFunction = Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();
				//return SubtractFunction(a, b);
				return sub(a, b);
			};

			internal static Func<T, T, T> MultiplyFunction = (a, b) =>
			{
				//var paramA = Expression.Parameter(typeof(T));
				//var paramB = Expression.Parameter(typeof(T));
				//var body   = Expression.Multiply(paramA, paramB);
				//MultiplyFunction = Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();
				//return MultiplyFunction(a, b);
				return mul(a, b);
			};

			internal static Func<T, T, T> DivideFunction = (a, b) =>
			{
				//var paramA = Expression.Parameter(typeof(T));
				//var paramB = Expression.Parameter(typeof(T));
				//var body   = Expression.Divide(paramA, paramB);
				//DivideFunction = Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();
				//return DivideFunction(a, b);
				return div(a, b);
			};
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
	}
}