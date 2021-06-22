using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SimpleCore.Numeric;

namespace TestBenchmark
{
	public class MyClass2
	{
		[Benchmark]
		public int Mul()
		{
			return MathHelper.Multiply(2, 2);
		}

		[Benchmark]
		public int Div()
		{
			return MathHelper.Divide(10, 5);
		}

		[Benchmark]
		public int Add()
		{
			return MathHelper.Add(1, 1);
		}

		[Benchmark]
		public int Sub()
		{
			return MathHelper.Subtract(1, 1);
		}
	}

	public static class Program
	{
		private static void Main(string[] args)
		{
			BenchmarkRunner.Run<MyClass2>();
		}
	}
}