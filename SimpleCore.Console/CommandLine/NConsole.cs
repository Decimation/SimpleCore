using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using JetBrains.Annotations;
using Pastel;
using SimpleCore.Utilities;
using static SimpleCore.Internal.Common;

// ReSharper disable InvocationIsSkipped
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
using Console = global::System.Console;

namespace SimpleCore.Console.CommandLine
{
	// todo

	/// <summary>
	///     Extended console.
	/// </summary>
	/// <list type="bullet">
	///     <item>
	///         <description>
	///             <see cref="NConsole" />
	///         </description>
	///     </item>
	///     <item>
	///         <description>
	///             <see cref="NConsoleIO" />
	///         </description>
	///     </item>
	///     <item>
	///         <description>
	///             <see cref="NConsoleOption" />
	///         </description>
	///     </item>
	///     <item>
	///         <description>
	///             <see cref="NConsoleInterface" />
	///         </description>
	///     </item>
	/// </list>
	public static class NConsole
	{
		internal static readonly string NativeNewLine = '\n'.ToString();

		public static void NewLine() => System.Console.WriteLine();

		public static int BufferLimit { get; set; } = System.Console.BufferWidth - 10;

		public enum Level
		{
			None,
			Info,
			Error,
			Debug,
			Success
		}

		public static string AddColor(string s, Color c)
		{
			s = s.Pastel(c);
			return s;
		}

		public static string AddColorBG(string s, Color c)
		{
			s = s.PastelBg(c);
			return s;
		}

		public static string AddUnderline(string s)
		{
			//\x1b[36mTEST\x1b[0m

			s = $"\x1b[4m{s}\x1b[0m";
			return s;
		}

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static string FormatString(string c, string s)
		{
			var srg = s.Split(NativeNewLine);

			for (int i = 0; i < srg.Length; i++) {
				string y = Formatting.SPACE + srg[i];

				string x;

				if (String.IsNullOrWhiteSpace(y)) {
					x = String.Empty;
				}
				else {
					x = c + y;
				}

				string x2 = x.Truncate(BufferLimit);

				if (x2.Length < x.Length) {
					x2 += Formatting.ELLIPSES;
				}

				srg[i] = x2;
			}

			string s2 = String.Join(NativeNewLine, srg);

			return s2;
		}

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static string FormatString(char c, string s)
		{
			return FormatString(c.ToString(), s);
		}


		public static void Init()
		{
			System.Console.OutputEncoding = Encoding.Unicode;
		}

		public static void RunWithColor(ConsoleColor fgColor, Action func) =>
			RunWithColor(fgColor, System.Console.BackgroundColor, func);

		public static void RunWithColor(ConsoleColor fgColor, ConsoleColor bgColor, Action func)
		{
			var oldFgColor = System.Console.ForegroundColor;
			var oldBgColor = System.Console.BackgroundColor;

			System.Console.ForegroundColor = fgColor;
			System.Console.BackgroundColor = bgColor;

			func();

			System.Console.ForegroundColor = oldFgColor;
			System.Console.BackgroundColor = oldBgColor;
		}


		public static Color? OverrideForegroundColor { get; set; } = null;
		
		public static Color? OverrideBackgroundColor { get; set; } = null;
		
		
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void Write(string msg, params object[] args) => Write(Level.None, null, null, true, msg, args);

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void Write(Level lvl, string msg, params object[] args) =>
			Write(lvl, null, null, true, msg, args);

		/// <summary>
		///     Root write method.
		/// </summary>
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void Write(Level lvl, Color? fg, Color? bg, bool newLine, string msg, params object[] args)
		{
			string sym = lvl switch
			{
				Level.None    => string.Empty,
				Level.Info    => Formatting.ASTERISK.ToString(),
				Level.Error   => Formatting.EXCLAMATION.ToString(),
				Level.Debug   => Formatting.MUL_SIGN.ToString(),
				Level.Success => Formatting.CHECK_MARK.ToString(),
				_             => throw new ArgumentOutOfRangeException(nameof(lvl), lvl, null)
			};

			var fmt = String.Format(msg, args);

			string s = lvl == Level.None ? fmt : FormatString(sym, fmt);

			/*
			 * Foreground color
			 */
			
			if (fg.HasValue) {
				s = AddColor(s, fg.Value);
			}
			else {

				Color buf;

				if (OverrideForegroundColor.HasValue) {
					buf = OverrideForegroundColor.Value;
				}
				else {
					Color? autoFgColor = lvl switch
					{
						Level.Info    => Color.White,
						Level.Error   => Color.Red,
						Level.Debug   => Color.DarkGray,
						Level.Success => Color.LawnGreen,
						_ => Color.White,
						//_             => null,
					};

					buf = autoFgColor.Value;
				}
				
				

				//Color buf = autoFgColor ?? (CurrentForegroundColor ?? Color.White);
				
				
				s = AddColor(s, buf);
			}
			
			/*
			 * Background color
			 */
			
			if (bg.HasValue) {
				s = AddColorBG(s, bg.Value);
			}
			else {

				if (OverrideBackgroundColor.HasValue) {
					var buf = OverrideBackgroundColor.Value;
					s   = AddColor(s, buf);
				}
				
				
			}


			if (newLine) {
				System.Console.WriteLine(s);
			}
			else {
				System.Console.Write(s);
			}
		}

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteColor(Color fgColor, string msg, params object[] args) =>
			WriteColor(fgColor, true, msg, args);

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteColor(Color fgColor, bool newLine, string msg, params object[] args)
		{
			if (newLine) {
				System.Console.WriteLine(msg.Pastel(fgColor), args);
			}
			else {
				System.Console.Write(msg.Pastel(fgColor), args);
			}
		}

		[Conditional(DEBUG_COND)]
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteDebug(string msg, params object[] args) => Write(Level.Debug, msg, args);

		[Conditional(DEBUG_COND)]
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteDebug(object obj) => WriteDebug(obj.ToString());

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteError(string msg, params object[] args) => Write(Level.Error, msg, args);

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteError(object obj) => WriteError(obj.ToString());

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteInfo(string msg, params object[] args) => Write(Level.Info, msg, args);

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteInfo(object obj) => WriteInfo(obj.ToString());

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteOnCurrentLine(Color color, string msg, params object[] args)
		{
			msg = String.Format(msg, args);

			string clear = new string('\b', msg.Length);
			System.Console.Write(clear);
			WriteColor(color, false, msg);
		}

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteSuccess(object obj) => WriteSuccess(obj.ToString());

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteSuccess(string msg, params object[] args) => Write(Level.Success, msg, args);

		public static void Write(params object[] args)
		{
			var s = args.QuickJoin();
			Write($"{s}");
		}
	}
}