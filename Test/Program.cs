﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Common;
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
using SimpleCore.Utilities.Configuration;
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
		public static async Task Main(string[] args)
		{
			//var i = new int[] { 1, 2, 3 };
			//var o = NConsoleOption.FromArray(i);

			//o[0].ShiftFunction = () =>
			//{
			//	Debug.WriteLine("shift");
			//	return null;
			//};
			//o[0].ComboFunction = () =>
			//{
			//	Debug.WriteLine("combo");
			//	return null;
			//};
			//o[0].CtrlFunction = () =>
			//{
			//	Debug.WriteLine("ctrl");
			//	return null;
			//};
			//o[0].AltFunction = () =>
			//{
			//	Debug.WriteLine("alt");
			//	return null;
			//};
			//// o[0].Function = () =>
			//// {
			//// 	Debug.WriteLine("main");
			//// 	return null;
			//// };
			//var d = new NConsoleDialog() { Options = o, SelectMultiple = true };
			//NConsole.Init();
			//var x =  NConsole.ReadOptions(d);

			//Console.WriteLine(x);

			var s       = "http://tidder.xyz/?imagelink=https://i.imgur.com/QtCausw.png";
			//var hostUri = Network.GetHostUri(new Uri(s));
			//Console.WriteLine(hostUri);
			//Console.WriteLine(Network.IsUriAlive2(new Uri(s)));
			//var sw2     = Stopwatch.StartNew();
			//Console.WriteLine(Network.IsUriAlive(hostUri));
			//sw2.Stop();
			//Console.WriteLine(sw2.Elapsed.TotalSeconds);
			//var component = Network.GetHostComponent(new Uri(s));
			//var address   = Dns.GetHostAddresses(component)[0];
			//Console.WriteLine(address);

			//var sw = Stopwatch.StartNew();
			//var p2 = new Ping();
			//var t2 =p2.SendPingAsync(address, (int)TimeSpan.FromSeconds(3).TotalMilliseconds);
			//await t2;
			//sw.Stop();
			//Console.WriteLine(sw.Elapsed.TotalSeconds);

			//var x = Network.Identify(address);
			//Console.WriteLine(x);
			
			//var p           = new Ping();
			//var component   = Network.GetHostComponent(new Uri(s));
			//var address = Dns.GetHostAddresses(component.ToString())[0];
			//Console.WriteLine(address);
			//var x           =p.SendPingAsync(address);
			//x.Wait();
			//Console.WriteLine(x.Result.Status);
			//Console.WriteLine(Network.IsUriAlive2((component)));

			var sw = Stopwatch.StartNew();
			var a=Network.IsAlive(new Uri((s)));
			sw.Stop();
			Console.WriteLine(sw.Elapsed.TotalSeconds);
			
			Console.WriteLine(a);

			var pingReply = Network.Ping(new Uri("https://i.imgur.com/QtCausw.png"));
			Console.WriteLine(pingReply.RoundtripTime);

			var address = Network.GetAddress((s));
			Console.WriteLine(address);
			Console.WriteLine(Network.GetAddress((address)));
			Console.WriteLine(Network.Identify(address));

			
		}
	}
}