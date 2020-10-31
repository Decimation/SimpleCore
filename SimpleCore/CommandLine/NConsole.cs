using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using JetBrains.Annotations;
using Pastel;
using SimpleCore.Utilities;
using SimpleCore.Win32;
using static SimpleCore.Internal.Common;

// ReSharper disable InvocationIsSkipped
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

#pragma warning disable HAA0601 //
#pragma warning disable HAA0501 //
#pragma warning disable HAA0502 //
#pragma warning disable HAA0301 //
#pragma warning disable HAA0302 //


namespace SimpleCore.CommandLine
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
	///             <see cref="NConsoleUI" />
	///         </description>
	///     </item>
	/// </list>
	public static class NConsole
	{
		public const int STD_ERROR_HANDLE  = -12;
		public const int STD_INPUT_HANDLE  = -10;
		public const int STD_OUTPUT_HANDLE = -11;


		internal static readonly string NewLine = '\n'.ToString();

		public static int BufferLimit { get; set; } = Console.BufferWidth - 10;

		public enum Level
		{
			None,
			Info,
			Error,
			Debug,
			Success
		}

		public static void AddColor(ref string s, Color c)
		{
			s = s.Pastel(c);
		}

		public static void AddColorBG(ref string s, Color c)
		{
			s = s.PastelBg(c);
		}

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static string FormatString(char c, string s)
		{
			var srg = s.Split(NewLine);

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

			string s2 = String.Join(NewLine, srg);

			return s2;
		}

		/// <param name="nStdHandle">
		///     <see cref="STD_INPUT_HANDLE" />,
		///     <see cref="STD_OUTPUT_HANDLE" />,
		///     <see cref="STD_ERROR_HANDLE" />
		/// </param>
		[DllImport(Native.OS.KERNEL32_DLL, SetLastError = true)]
		public static extern IntPtr GetStdHandle(int nStdHandle);

		public static void Init()
		{
			Console.OutputEncoding = Encoding.Unicode;
		}

		public static void RunWithColor(ConsoleColor fgColor, Action func) =>
			RunWithColor(fgColor, Console.BackgroundColor, func);

		public static void RunWithColor(ConsoleColor fgColor, ConsoleColor bgColor, Action func)
		{
			var oldFgColor = Console.ForegroundColor;
			var oldBgColor = Console.BackgroundColor;

			Console.ForegroundColor = fgColor;
			Console.BackgroundColor = bgColor;

			func();

			Console.ForegroundColor = oldFgColor;
			Console.BackgroundColor = oldBgColor;
		}

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
			char sym = lvl switch
			{
				Level.None    => Formatting.NULL_CHAR,
				Level.Info    => Formatting.ASTERISK,
				Level.Error   => Formatting.EXCLAMATION,
				Level.Debug   => Formatting.MUL_SIGN,
				Level.Success => Formatting.CHECK_MARK,
				_             => throw new ArgumentOutOfRangeException(nameof(lvl), lvl, null)
			};

			string s = FormatString(sym, String.Format(msg, args));

			if (fg.HasValue) {
				AddColor(ref s, fg.Value);
			}
			else {
				var autoFgColor = lvl switch
				{
					Level.Info    => Color.White,
					Level.Error   => Color.Red,
					Level.Debug   => Color.DarkGray,
					Level.Success => Color.LawnGreen,
					_             => Color.White
				};

				AddColor(ref s, autoFgColor);
			}

			if (bg.HasValue) {
				AddColorBG(ref s, bg.Value);
			}

			if (newLine) {
				Console.WriteLine(s);
			}
			else {
				Console.Write(s);
			}
		}

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteColor(Color fgColor, string msg, params object[] args) =>
			WriteColor(fgColor, true, msg, args);

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteColor(Color fgColor, bool newLine, string msg, params object[] args)
		{
			if (newLine) {
				Console.WriteLine(msg.Pastel(fgColor), args);
			}
			else {
				Console.Write(msg.Pastel(fgColor), args);
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
			Console.Write(clear);
			WriteColor(color, false, msg);
		}

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteSuccess(object obj) => WriteSuccess(obj.ToString());

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteSuccess(string msg, params object[] args) => Write(Level.Success, msg, args);
	}
}