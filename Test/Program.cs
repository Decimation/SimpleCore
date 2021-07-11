using System;
using System.Collections;
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
			
			MyClass m = new MyClass();
			ConfigComponents.UpdateFields(m);

		}

		class MyClass:IConfig
		{
			/// <summary>
			///     Engines to use for searching
			/// </summary>
			[field: ConfigComponent("search_engines", "--search-engines", 1, true)]
			public int SearchEngines { get; set; }

			/// <summary>
			///     Engines whose results should be opened in the browser
			/// </summary>
			[field: ConfigComponent("priority_engines", "--priority-engines", 2, true)]
			public int PriorityEngines { get; set; }

			/// <summary>
			///     <see cref="ImgurClient" /> API key
			/// </summary>
			[field: ConfigComponent("imgur_client_id", "--saucenao-auth")]
			public string ImgurAuth { get; set; }

			/// <summary>
			///     <see cref="SauceNaoEngine" /> API key
			/// </summary>
			[field: ConfigComponent("saucenao_key", "--imgur-auth")]
			public string SauceNaoAuth { get; set; }

			/// <summary>
			///     Does not open results from priority engines if the result similarity (if available) is below a certain threshold,
			/// or there are no relevant results.
			/// <see cref="BasicSearchResult.Filter"/> is <c>true</c> if <see cref="ISearchEngine.FilterThreshold"/> is less than <see cref="BasicSearchResult.Similarity"/>
			/// </summary>
			[field: ConfigComponent("filter_results", "--filter-results", true, true)]
			public bool FilterResults { get; set; }

			public string FileLocation
			{
				get => @"C:\Users\Deci\Desktop\cf.cfg";
			}
		}
	}
}