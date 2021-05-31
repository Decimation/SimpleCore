using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Running;
using Newtonsoft.Json.Linq;
using SimpleCore.Cli;
using SimpleCore.Diagnostics;
using SimpleCore.Model;
using SimpleCore.Net;
using SimpleCore.Numeric;
using SimpleCore.Utilities;
using Formatting = SimpleCore.Utilities.Formatting;
using MathHelper = SimpleCore.Numeric.MathHelper;


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


			//Network.GetFinalRedirect("https://ascii2d.net/search/url/https://files.catbox.moe/txvi31.png");
			//Console.WriteLine( new UriBuilder(new Uri("https://ascii2d.net/search/url/").Host).Uri);
			//Console.WriteLine(new Uri("https://ascii2d.net/search/url/").GetComponents(UriComponents.NormalizedHost, UriFormat.Unescaped));
			//Console.WriteLine((new UriBuilder("ascii2d.net").Uri));
			//ServicePointManager.UseNagleAlgorithm = false;
			
			var b = MediaTypes.IsDirect("https://files.catbox.moe/txvi31.png", MimeType.Image);
			Console.WriteLine(b);
		}

	}
}