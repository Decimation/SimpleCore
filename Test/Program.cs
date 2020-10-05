using System;
using System.Drawing;
using System.Text;
using System.Threading;
using Pastel;
using SimpleCore.CommandLine;
using SimpleCore.CommandLine.Shell;
using SimpleCore.Diagnostics;
using SimpleCore.Utilities;


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
			Console.WriteLine();
		}
	}
}