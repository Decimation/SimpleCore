using System;
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
	// https://github.com/khalidabuhakmeh/ConsoleTables

	[Flags]
	public enum HexOptions
	{
		None = 0,

		Prefix = 1,

		Lowercase = 1 << 1,

		Default = Prefix
	}

	/// <summary>
	///     Utilities for formatting.
	/// </summary>
	public static class Formatting
	{
		#region Constants

		public const string JOIN_COMMA = ", ";

		/// <summary>
		///     Scope resolution operator
		/// </summary>
		public const string JOIN_SCOPE = "::";

		public const string JOIN_SPACE = " ";


		private const string HEX_FORMAT_SPECIFIER = "X";

		private const string HEX_PREFIX = "0x";

		public const char PERIOD      = '.';
		public const char ASTERISK    = '*';
		public const char EXCLAMATION = '!';
		public const char SPACE       = ' ';

		public const string ELLIPSES = "...";


		public const char ARROW_DOWN       = '\u2193';
		public const char ARROW_LEFT       = '\u2190';
		public const char ARROW_LEFT_RIGHT = '\u2194';
		public const char ARROW_RIGHT      = '\u2192';
		public const char ARROW_UP         = '\u2191';
		public const char ARROW_UP_DOWN    = '\u2195';


		public const char BALLOT_X = '\u2717';

		public const char HEAVY_BALLOT_X = '\u2718';

		public const char CHECK_MARK = '\u2713';

		public const char HEAVY_CHECK_MARK = '\u2714';
		public const char LOZENGE          = '\u25ca';

		public const char MUL_SIGN = '\u00D7';

		public const char MUL_SIGN2 = '\u2715';


		public const           char   NULL_CHAR     = '\0';
		public const           char   RAD_SIGN      = '\u221A';
		public const           char   RELOAD        = '\u21bb';
		public const           char   SUN           = '\u263c';
		public static readonly string NativeNewLine = '\n'.ToString();

		#endregion

		#region Join

		public static string FormatJoin<T>(this IEnumerable<T> values,
			string format, IFormatProvider? provider = null, string delim = JOIN_COMMA) where T : IFormattable =>
			String.Join(delim, values.Select(v => v.ToString(format, provider)));

		/// <summary>
		///     Concatenates the strings returned by <paramref name="toString" />
		///     using the specified separator between each element or member.
		/// </summary>
		/// <param name="values">Collection of values</param>
		/// <param name="toString">
		///     Function which returns a <see cref="string" /> given a member of <paramref name="values" />
		/// </param>
		/// <param name="delim">Delimiter</param>
		/// <typeparam name="T">Element type</typeparam>
		public static string FuncJoin<T>(this IEnumerable<T> values,
			Func<T, string> toString, string delim = JOIN_COMMA) =>
			String.Join(delim, values.Select(toString));

		public static string QuickJoin<T>(this IEnumerable<T> enumerable, string delim = JOIN_COMMA) =>
			String.Join(delim, enumerable);

		public static string SimpleJoin<T>(this IEnumerable<T> values, string delim = JOIN_COMMA) =>
			String.Join(delim, values);

		#endregion

		#region Hex

		public static string ToHexString<T>(T value, HexOptions options = HexOptions.Default)
		{
			var sb = new StringBuilder();

			if (options.HasFlagFast(HexOptions.Prefix)) {
				sb.Append(HEX_PREFIX);
			}

			string? hexStr;

			if (value is IFormattable fmt) {
				hexStr = fmt.ToString(HEX_FORMAT_SPECIFIER, null);
			}
			else {
				throw new NotImplementedException();
			}

			if (options.HasFlagFast(HexOptions.Lowercase)) {
				hexStr = hexStr.ToLower();
			}

			sb.Append(hexStr);

			return sb.ToString();
		}

		public static unsafe string ToHexString(void* value, HexOptions options = HexOptions.Default) =>
			ToHexString((long) value, options);

		public static string ToHexString(IntPtr value, HexOptions options = HexOptions.Default) =>
			ToHexString((long) value, options);

		#endregion

		#region Pastel

		//https://github.com/silkfire/Pastel/blob/master/src/ConsoleExtensions.cs

		private const int  STD_OUTPUT_HANDLE                  = -11;
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
			$"(?<!^)(?<!{FORMAT_STRING_END.Replace("[", @"\[")})(?<!{String.Format($"{FORMAT_STRING_START.Replace("[", @"\[")}{FORMAT_STRING_COLOR}", new[] {$"(?:{PlaneFormatModifiers[ColorPlane.Foreground]}|{PlaneFormatModifiers[ColorPlane.Background]})"}.Concat(Enumerable.Repeat(@"\d{1,3}", 3)).Cast<object>().ToArray())})(?:{String.Format(FORMAT_STRING_START.Replace("[", @"\["), $"(?:{PlaneFormatModifiers[ColorPlane.Foreground]}|{PlaneFormatModifiers[ColorPlane.Background]})")})",
			RegexOptions.Compiled);

		private static readonly ReadOnlyDictionary<ColorPlane, Regex> CloseNestedPastelStringRegex3 =
			new(new Dictionary<ColorPlane, Regex>
			{
				[ColorPlane.Foreground] =
					new(
						$"(?:{FORMAT_STRING_END.Replace("[", @"\[")})(?!{String.Format(FORMAT_STRING_START.Replace("[", @"\["), PlaneFormatModifiers[ColorPlane.Foreground])})(?!$)",
						RegexOptions.Compiled),
				[ColorPlane.Background] =
					new(
						$"(?:{FORMAT_STRING_END.Replace("[", @"\[")})(?!{String.Format(FORMAT_STRING_START.Replace("[", @"\["), PlaneFormatModifiers[ColorPlane.Background])})(?!$)",
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


		private static readonly ReadOnlyDictionary<bool, ReadOnlyDictionary<ColorPlane, ColorFormatFunction>> ColorFormatFunctions
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

		private static readonly ReadOnlyDictionary<bool, ReadOnlyDictionary<ColorPlane, HexColorFormatFunction>> HexColorFormatFunctions = new(
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


		static Formatting()
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

			s = $"\x1b[4m{s}\x1b[0m";
			return s;
		}

		public static StringBuilder AppendColor(this StringBuilder sb, Color c, object value)
		{
			return sb.Append(value.ToString()!.AddColor(c));
		}

		public static StringBuilder AppendLabelWithColor(this StringBuilder sb, 
			Color ck, string k, Color cv, object v)
		{
			return sb.AppendColor(ck, k + ": ").AppendColor(cv, v);
		}

		#endregion

		public static string ToString<T>(T[] rg)
		{
			if (typeof(T) == typeof(byte)) {
				byte[]? byteArray = rg as byte[];
				return FormatJoin(byteArray!, HEX_FORMAT_SPECIFIER);
			}

			return SimpleJoin(rg);
		}

		public const string ANSI_RESET = "\u001b[0m";
	}
}