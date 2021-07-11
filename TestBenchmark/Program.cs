using System;
using BenchmarkDotNet.Running;

namespace TestBenchmark
{
	public static class Program
	{
		private static void Main(string[] args)
		{
			//dotnet build -c Release & dotnet run -c Release
			BenchmarkRunner.Run<Benchmarks3>();
		}
	}
}