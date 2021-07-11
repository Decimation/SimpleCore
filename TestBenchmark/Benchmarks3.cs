using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace TestBenchmark
{
	public unsafe class Benchmarks3
	{
		[Benchmark]
		public int a()
		{
			return rg[1];
		}

		//[Benchmark]
		//public int b()
		//{
		//	return p[1];
		//}

		[Benchmark]
		public int b()
		{
			fixed (int* x = rg) {
				return x[1];
			}
		}

		[Benchmark]
		public int c()
		{
			return *(int*) Marshal.UnsafeAddrOfPinnedArrayElement(rg, 1);
		}

		public int[] rg = new[] {1, 2, 3};
	}
}