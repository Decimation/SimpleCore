using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SimpleCore.Console.CommandLine;
using SimpleCore.Diagnostics;
using SimpleCore.Model;
using SimpleCore.Utilities;


namespace Test
{
	// nuget pack -Prop Configuration=Release

	// C:\Library\Nuget
	// dotnet pack -c Release -o %cd%
	// dotnet nuget push "*.nupkg"
	// del *.nupkg & dotnet pack -c Release -o %cd% & dotnet nuget push "*.nupkg"

	/*
	 * Novus				https://github.com/Decimation/Novus
	 * NeoCore				https://github.com/Decimation/NeoCore
	 * RazorSharp			https://github.com/Decimation/RazorSharp
	 * 
	 * SimpleCore			https://github.com/Decimation/SimpleCore
	 * SimpleSharp			https://github.com/Decimation/SimpleSharp
	 *
	 * Memkit				https://github.com/Decimation/Memkit
	 * 
	 */

	public static class Program
	{
		public static void Main(string[] args)
		{

			var n = new NConsoleOption[] {new() {Name = "a"}, new() {Name = "b"}};
			NConsoleIO.ReadOptions(n);

			var s = "foo";
			s = NConsole.AddUnderline(s);
			Console.WriteLine(s);
			Console.WriteLine("foo");

			NConsole.Write(1, 2, 3);
		}
	}
}