using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Pastel;
using SimpleCore.CommandLine;
using SimpleCore.Diagnostics;
using SimpleCore.Model;
using SimpleCore.Utilities;
using SimpleCore.Win32;


namespace Test
{
	// nuget pack -Prop Configuration=Release

	// C:\Library\Nuget
	// dotnet pack -c Release -o %cd%
	// dotnet nuget push "*.nupkg"
	// del *.nupkg & dotnet pack -c Release -o %cd% & dotnet nuget push "*.nupkg"


	public static class Program
	{
		public static void Main(string[] args)
		{
			NConsole.Init();
			var s = "foo bar";

			Console.WriteLine(s.JSubstring(1..(s.Length - 1)));
			Console.WriteLine(s.JSubstring(^(s.Length   - 1)));

			var s2 = "bar123";
			Console.WriteLine(s2.SelectOnlyDigits());

			Console.WriteLine(s2.SelectOnly(char.IsLetter));
		}
	}
}