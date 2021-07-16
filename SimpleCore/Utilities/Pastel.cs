using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

// ReSharper disable InconsistentNaming

#nullable enable
// ReSharper disable UnusedMember.Global
// ReSharper disable ParameterTypeCanBeEnumerable.Global
// ReSharper disable StringCompareToIsCultureSpecific

namespace SimpleCore.Utilities
{
	public static class Pastel
	{
		//https://github.com/silkfire/Pastel/blob/master/src/ConsoleExtensions.cs

		private const int STD_OUTPUT_HANDLE = -11;

		private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

		private const string K32 = "kernel32.dll";

		private const string FORMAT_STRING_START   = "\u001b[{0};2;";
		private const string FORMAT_STRING_COLOR   = "{1};{2};{3}m";
		private const string FORMAT_STRING_CONTENT = "{4}";
		private const string FORMAT_STRING_END     = "\u001b[0m";


		private static bool _enabled;

		private static readonly string FormatStringFull =
			$"{FORMAT_STRING_START}{FORMAT_STRING_COLOR}{FORMAT_STRING_CONTENT}{FORMAT_STRING_END}";


		private static readonly ReadOnlyDictionary<ColorPlane, string> PlaneFormatModifiers =
			new(new Dictionary<ColorPlane, string>
			{
				[ColorPlane.Foreground] = "38",
				[ColorPlane.Background] = "48"
			});


		private static readonly Regex CloseNestedPastelStringRegex1 =
			new($"({FORMAT_STRING_END.Replace("[", @"\[")})+", RegexOptions.Compiled);

		private static readonly Regex CloseNestedPastelStringRegex2 = new(
			$"(?<!^)(?<!{FORMAT_STRING_END.Replace("[", @"\[")})(?<!{String.Format($"{FORMAT_STRING_START.Replace("[", @"\[")}{FORMAT_STRING_COLOR}", new[] {$"(?:{PlaneFormatModifiers[ColorPlane.Foreground]}|{PlaneFormatModifiers[ColorPlane.Background]})"}.Concat(Enumerable.Repeat(@"\d{1,3}", 3)).Cast<object>().ToArray())})(?:{String.Format(FORMAT_STRING_START.Replace("[", @"\["), $"(?:{PlaneFormatModifiers[ColorPlane.Foreground]}|{PlaneFormatModifiers[ColorPlane.Background]})")})"
			,
			RegexOptions.Compiled);

		private static readonly ReadOnlyDictionary<ColorPlane, Regex> CloseNestedPastelStringRegex3 =
			new(new Dictionary<ColorPlane, Regex>
			{
				[ColorPlane.Foreground] =
					new(
						$"(?:{FORMAT_STRING_END.Replace("[", @"\[")})(?!{String.Format(FORMAT_STRING_START.Replace("[", @"\["), PlaneFormatModifiers[ColorPlane.Foreground])})(?!$)"
						,
						RegexOptions.Compiled),
				[ColorPlane.Background] =
					new(
						$"(?:{FORMAT_STRING_END.Replace("[", @"\[")})(?!{String.Format(FORMAT_STRING_START.Replace("[", @"\["), PlaneFormatModifiers[ColorPlane.Background])})(?!$)"
						,
						RegexOptions.Compiled)
			});


		private static readonly Func<string, int> ParseHexColor =
			hc => Int32.Parse(hc.Replace("#", ""), NumberStyles.HexNumber);

		private static readonly Func<string, Color, ColorPlane, string> ColorFormat = (i, c, p) =>
			String.Format(FormatStringFull, PlaneFormatModifiers[p], c.R, c.G, c.B, CloseNestedPastelStrings(i, c, p));

		private static readonly Func<string, string, ColorPlane, string> ColorHexFormat =
			(i, c, p) => ColorFormat(i, Color.FromArgb(ParseHexColor(c)), p);

		private static readonly ColorFormatFunction NoColorOutputFormat = (i, _) => i;

		private static readonly HexColorFormatFunction NoHexColorOutputFormat = (i, _) => i;

		private static readonly ColorFormatFunction
			ForegroundColorFormat = (i, c) => ColorFormat(i, c, ColorPlane.Foreground);

		private static readonly HexColorFormatFunction ForegroundHexColorFormat =
			(i, c) => ColorHexFormat(i, c, ColorPlane.Foreground);

		private static readonly ColorFormatFunction
			BackgroundColorFormat = (i, c) => ColorFormat(i, c, ColorPlane.Background);

		private static readonly HexColorFormatFunction BackgroundHexColorFormat =
			(i, c) => ColorHexFormat(i, c, ColorPlane.Background);


		private static readonly ReadOnlyDictionary<bool, ReadOnlyDictionary<ColorPlane, ColorFormatFunction>>
			ColorFormatFunctions
				= new(
					new Dictionary<bool, ReadOnlyDictionary<ColorPlane, ColorFormatFunction>>
					{
						[false] = new(
							new Dictionary<ColorPlane, ColorFormatFunction>
							{
								[ColorPlane.Foreground] = NoColorOutputFormat,
								[ColorPlane.Background] = NoColorOutputFormat
							}),
						[true] = new(
							new Dictionary<ColorPlane, ColorFormatFunction>
							{
								[ColorPlane.Foreground] = ForegroundColorFormat,
								[ColorPlane.Background] = BackgroundColorFormat
							})
					});

		private static readonly ReadOnlyDictionary<bool, ReadOnlyDictionary<ColorPlane, HexColorFormatFunction>>
			HexColorFormatFunctions = new(
				new Dictionary<bool, ReadOnlyDictionary<ColorPlane, HexColorFormatFunction>>
				{
					[false] = new(
						new Dictionary<ColorPlane, HexColorFormatFunction>
						{
							[ColorPlane.Foreground] = NoHexColorOutputFormat,
							[ColorPlane.Background] = NoHexColorOutputFormat
						}),
					[true] = new(
						new Dictionary<ColorPlane, HexColorFormatFunction>
						{
							[ColorPlane.Foreground] = ForegroundHexColorFormat,
							[ColorPlane.Background] = BackgroundHexColorFormat
						})
				});


		static Pastel()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
				var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);

				bool enable = GetConsoleMode(iStdOut, out uint outConsoleMode)
				              && SetConsoleMode(iStdOut, outConsoleMode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
			}


			if (Environment.GetEnvironmentVariable("NO_COLOR") == null) {
				Enable();
			}
			else {
				Disable();
			}
		}


		[DllImport(K32)]
		private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

		[DllImport(K32)]
		private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

		[DllImport(K32, SetLastError = true)]
		private static extern IntPtr GetStdHandle(int nStdHandle);


		/// <summary>
		///     Enables any future console color output produced by Pastel.
		/// </summary>
		public static void Enable()
		{
			_enabled = true;
		}

		/// <summary>
		///     Disables any future console color output produced by Pastel.
		/// </summary>
		public static void Disable()
		{
			_enabled = false;
		}


		private static string CloseNestedPastelStrings(string input, Color color, ColorPlane colorPlane)
		{
			string closedString = CloseNestedPastelStringRegex1.Replace(input, FORMAT_STRING_END);

			closedString = CloseNestedPastelStringRegex2.Replace(closedString, $"{FORMAT_STRING_END}$0");

			closedString = CloseNestedPastelStringRegex3[colorPlane].Replace(closedString,
			                                                                 $"$0{String.Format($"{FORMAT_STRING_START}{FORMAT_STRING_COLOR}", PlaneFormatModifiers[colorPlane], color.R, color.G, color.B)}");

			return closedString;
		}

		private delegate string ColorFormatFunction(string input, Color color);

		private delegate string HexColorFormatFunction(string input, string hexColor);

		private enum ColorPlane : byte
		{
			Foreground,
			Background
		}

		/// <summary>
		///     Returns a string wrapped in an ANSI foreground color code using the specified color.
		/// </summary>
		/// <param name="input">The string to color.</param>
		/// <param name="color">The color to use on the specified string.</param>
		public static string AddColor(this string input, Color color)
		{
			return ColorFormatFunctions[_enabled][ColorPlane.Foreground](input, color);
		}

		/// <summary>
		///     Returns a string wrapped in an ANSI foreground color code using the specified color.
		/// </summary>
		/// <param name="input">The string to color.</param>
		/// <param name="hexColor">The color to use on the specified string.
		///     <para>Supported format: [#]RRGGBB.</para>
		/// </param>
		public static string AddColor(this string input, string hexColor)
		{
			return HexColorFormatFunctions[_enabled][ColorPlane.Foreground](input, hexColor);
		}


		/// <summary>
		///     Returns a string wrapped in an ANSI background color code using the specified color.
		/// </summary>
		/// <param name="input">The string to color.</param>
		/// <param name="color">The color to use on the specified string.</param>
		public static string AddColorBG(this string input, Color color)
		{
			return ColorFormatFunctions[_enabled][ColorPlane.Background](input, color);
		}

		/// <summary>
		///     Returns a string wrapped in an ANSI background color code using the specified color.
		/// </summary>
		/// <param name="input">The string to color.</param>
		/// <param name="hexColor">The color to use on the specified string.
		///     <para>Supported format: [#]RRGGBB.</para>
		/// </param>
		public static string AddColorBG(this string input, string hexColor)
		{
			return HexColorFormatFunctions[_enabled][ColorPlane.Background](input, hexColor);
		}


		public static string AddUnderline(this string s)
		{
			//\x1b[36mTEST\x1b[0m

			s = $"\x1b[4m{s}{ANSI_RESET}";
			return s;
		}

		private const string ANSI_RESET = "\u001b[0m";
	}
}