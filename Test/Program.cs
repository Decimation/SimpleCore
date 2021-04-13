﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Running;
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

			var s  = Network.GetStream("https://i.imgur.com/QtCausw.png");
			var ms = s as MemoryStream;
			var rg = new byte[256];
			ms.Read(rg,0,256);
			Console.WriteLine(MediaTypes.ResolveFromData(rg));
			Console.WriteLine(Network.IsUriAlive(new ("http://karmadecay.com/results/u14728299")));

			Console.WriteLine(Network.GetSimpleResponse("https://ascii2d.net/search/url/https://i.imgur.com/QtCausw.jpg").Content);
		}
	}
}