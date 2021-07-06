using BenchmarkDotNet.Attributes;

namespace TestBenchmark
{
	public class Benchmarks2
	{

		/*
		 * | Method |      Mean |     Error |    StdDev | Median |
		 * |------- |----------:|----------:|----------:|-------:|
		 * |      a | 0.0002 ns | 0.0007 ns | 0.0006 ns | 0.0 ns |
		 * |      b | 0.0158 ns | 0.0226 ns | 0.0242 ns | 0.0 ns |
		 */


		public const int i = 3;

		[Benchmark]
		public bool a()
		{
			return (i & 1) ==1;
		}

		[Benchmark]
		public bool b()
		{
			return i % 2 == 1;
		}	
	}
}