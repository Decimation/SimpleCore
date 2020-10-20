using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using JetBrains.Annotations;
using Pastel;
using SimpleCore.Utilities;
using static SimpleCore.Internal.Common;

// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

#pragma warning disable HAA0501, HAA0502, HAA0601

namespace SimpleCore.CommandLine
{
	// todo


	/// <summary>
	/// Extended console.
	/// </summary>
	public static partial class NConsole
	{
		public enum Level
		{
			None,
			Info,
			Error,
			Debug,
			Success
		}

		private const char HEAVY_BALLOT_X = '\u2718';

		private const char HEAVY_CHECK_MARK = '\u2714';

		public const char MUL_SIGN = '\u00D7';

		public const char RAD_SIGN = '\u221A';

		private const char ASTERISK = '*';

		private const string ELLIPSES = "...";

		private const char NULL_CHAR = '\0';

		private const char EXCLAMATION = '!';


		private static readonly string NewLine = '\n'.ToString();

		public static int BufferLimit { get; set; } = Console.BufferWidth - 10;


		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static string FormatString(char c, string s)
		{
			var srg = s.Split(NewLine);

			for (int i = 0; i < srg.Length; i++) {
				string y = " " + srg[i];

				string x;

				if (String.IsNullOrWhiteSpace(y)) {
					x = String.Empty;
				}
				else {
					x = c + y;
				}

				string x2 = x.Truncate(BufferLimit);

				if (x2.Length < x.Length) {
					x2 += ELLIPSES;
				}

				srg[i] = x2;
			}

			string s2 = String.Join(NewLine, srg);

			return s2;
		}

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void Write(string msg, params object[] args) => Write(Level.None, null, null, true, msg, args);

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void Write(Level lvl, string msg, params object[] args) => Write(lvl, null, null, true, msg, args);


		/// <summary>
		///     Root write method.
		/// </summary>
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void Write(Level lvl, Color? fg, Color? bg, bool newLine, string msg, params object[] args)
		{
			char sym = lvl switch
			{
				Level.None => NULL_CHAR,
				Level.Info => ASTERISK,
				Level.Error => EXCLAMATION,
				Level.Debug => MUL_SIGN,
				Level.Success => RAD_SIGN,
				_ => throw new ArgumentOutOfRangeException(nameof(lvl), lvl, null)
			};

			//var oldFg = Console.ForegroundColor;
			//var oldBg = Console.BackgroundColor;
			string s = FormatString(sym, String.Format(msg, args));

			if (fg.HasValue) {
				AddColor(ref s, fg.Value);


			}
			else {
				var autoFgColor = lvl switch
				{
					Level.Info => Color.White,
					Level.Error => Color.Red,
					Level.Debug => Color.DarkGray,
					Level.Success => Color.LawnGreen,
					_ => Color.White
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

		[Conditional(DEBUG_COND)]
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteDebug(string msg, params object[] args) => Write(Level.Debug, msg, args);

		[Conditional(DEBUG_COND)]
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteDebug(object obj) => WriteDebug(obj.ToString());


		public static void Init()
		{
			Console.OutputEncoding = Encoding.Unicode;
		}


		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteError(string msg, params object[] args) => Write(Level.Error, msg, args);

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteError(object obj) => WriteError(obj.ToString());

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteInfo(string msg, params object[] args) => Write(Level.Info, msg, args);

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteInfo(object obj) => WriteInfo(obj.ToString());


		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteSuccess(object obj) => WriteSuccess(obj.ToString());

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteSuccess(string msg, params object[] args) => Write(Level.Success, msg, args);

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteOnCurrentLine(Color color, string msg, params object[] args)
		{
			msg = String.Format(msg, args);

			string clear = new string('\b', msg.Length);
			Console.Write(clear);

			WriteColor(color, false, msg);
		}

		public static void RunWithColor(ConsoleColor fgColor, Action func) => RunWithColor(fgColor, Console.BackgroundColor, func);

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

		public static void AddColorBG(ref string s, Color c)
		{
			s = s.PastelBg(c);
		}

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

		public static void AddColor(ref string s, Color c)
		{
			s = s.Pastel(c);
		}
	}
}