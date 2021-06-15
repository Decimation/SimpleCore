using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SimpleCore.Numeric;

namespace TestBenchmark
{
	public class MyClass
	{
		[Benchmark]
		public bool a()
		{
			return MathHelper.IsPrime(500);
		}
		
	}
	class Program
	{
		static void Main(string[] args)
		{
			BenchmarkRunner.Run<MyClass>();
		}
	}
}
