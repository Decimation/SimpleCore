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
			/*ConsoleKeyInfo cki;

			do
			{
				while (!Console.KeyAvailable)
				{
					// Do something
				}

				cki = Console.ReadKey(true);
				Console.WriteLine("{0} {1} {2} {3}",cki.Key,(int) cki.KeyChar, cki.Modifiers, (char) (int)cki.Key);
			} while (cki.Key != ConsoleKey.Escape);*/

			var op = new NConsoleOption()
			{
				Name = "test",
				Function = () =>
				{
					Console.WriteLine("-");
					NConsole.IO.WaitForSecond();
					return null;
				},
				AltFunction = () =>
				{
					Console.WriteLine("alt");
					NConsole.IO.WaitForSecond();
					return null;
				},
				CtrlFunction = () =>
				{
					Console.WriteLine("ctrl");
					NConsole.IO.WaitForSecond();
					return null;
				},
			};

			NConsole.IO.HandleOptions(new[] {op});
		}
	}
}