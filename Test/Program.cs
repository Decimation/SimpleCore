using System;
using System.Drawing;
using System.Text;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Pastel;
using SimpleCore.CommandLine;
using SimpleCore.CommandLine.Shell;
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
			QString s = "foo bar";
			Console.WriteLine(s);
			s.Remove(0, 3);
			Console.WriteLine(s);
			s.Replace("bar", "baz");
			Console.WriteLine(s);
		}
	}
}