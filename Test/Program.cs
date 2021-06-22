﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
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
using Console = System.Console;
using MathHelper = SimpleCore.Numeric.MathHelper;
using Timer = System.Timers.Timer;

#pragma warning disable IDE0060

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
			var i = new int[] { 1, 2, 3 };
			var o = NConsoleOption.FromArray(i);

			o[0].ShiftFunction = () =>
			{
				Debug.WriteLine("shift");
				return null;
			};
			o[0].ComboFunction = () =>
			{
				Debug.WriteLine("combo");
				return null;
			};
			o[0].CtrlFunction = () =>
			{
				Debug.WriteLine("ctrl");
				return null;
			};
			o[0].AltFunction = () =>
			{
				Debug.WriteLine("alt");
				return null;
			};
			o[0].Function = () =>
			{
				Debug.WriteLine("main");
				return null;
			};
			var d = new NConsoleDialog() { Options = o, SelectMultiple = true };
			var x = NConsole.ReadOptions(d);

			Console.WriteLine(x);
			//NConsole.ReadInput("hi", (a) => a != "g");


			//Task t = Task.Run(() => { Console.WriteLine("hello world"); Thread.Sleep(5000); });
			//NConsoleProgress.ForTask(t);

			// var k = Console.ReadKey();
			// Console.WriteLine($"{k.KeyChar} {(int)k.KeyChar} {k.Key} {(int)k.Key} {k.Modifiers}");
			//
			// k = Console.ReadKey();
			// Console.WriteLine($"{k.KeyChar} {(int)k.KeyChar} {k.Key} {(int)k.Key} {k.Modifiers}");


			//var p = new HtmlParser();
			//var s = WebUtilities.GetString("https://www.zerochan.net/2750747");
			//Console.WriteLine(s.Length);
			//var d = p.ParseDocument(s);
			//var q = d.QuerySelectorAll("img");
			//Console.WriteLine(q.Length);

			//foreach (var g in q) {
			//	Console.WriteLine(
			//		$"{g.GetAttribute("src")} | {MediaTypes.IsDirect(g.GetAttribute("src"), MimeType.Image)}");
			//}

			//var q2 = d.QuerySelectorAll("a");

			//Console.WriteLine(q2.Length);

			//foreach (var g in q2) {
			//	Console.WriteLine(
			//		$"{g.GetAttribute("href")} | {MediaTypes.IsDirect(g.GetAttribute("href"), MimeType.Image)}");
			//}



		}
	}
}