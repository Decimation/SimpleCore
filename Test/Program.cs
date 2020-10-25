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
			foreach (var f in FileFormatType.GetAll<FileFormatType>()) {
				Console.WriteLine(f);
			}

			Console.WriteLine(Images.GetImageDimensions(@"C:\Users\Deci\Desktop\fucking_epic.jpg"));
		}
	}
}