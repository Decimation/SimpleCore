using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using JetBrains.Annotations;
using Pastel;
using SimpleCore.Utilities;
using static SimpleCore.Internal.Common;

// ReSharper disable InvocationIsSkipped
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable ParameterTypeCanBeEnumerable.Local
// ReSharper disable UnusedVariable
// ReSharper disable ParameterTypeCanBeEnumerable.Global

#pragma warning disable 8602, CA1416
#nullable enable

namespace SimpleCore.Console.CommandLine
{
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
		#region Main

		internal static readonly string NativeNewLine = '\n'.ToString();

		public static void NewLine() => System.Console.WriteLine();

		/// <summary>
		/// Attempts to resize the console window
		/// </summary>
		/// <returns><c>true</c> if the operation succeeded</returns>
		public static bool Resize(int cww, int cwh)
		{
			bool canResize = System.Console.LargestWindowWidth  >= cww &&
			                 System.Console.LargestWindowHeight >= cwh;

			if (canResize) {
				System.Console.SetWindowSize(cww, cwh);
			}

			return canResize;
		}

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


		public static void ResetOverrideColors()
		{
			OverrideForegroundColor = null;
			OverrideBackgroundColor = null;
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
						_             => Color.White,
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
					s = AddColor(s, buf);
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

		#endregion

		#region IO

		/// <summary>
		///     Exits <see cref="ReadOptions{T}"/>
		/// </summary>
		public const ConsoleKey NC_GLOBAL_EXIT_KEY = ConsoleKey.Escape;

		/// <summary>
		/// <see cref="Refresh"/>
		/// </summary>
		public const ConsoleKey NC_GLOBAL_REFRESH_KEY = ConsoleKey.F5;

		public const char OPTION_N = 'N';
		public const char OPTION_Y = 'Y';


		private const int  MAX_OPTION_N        = 10;
		private const char OPTION_LETTER_START = 'A';

		/// <summary>
		///     Signals to continue displaying current interface
		/// </summary>
		private const int STATUS_OK = 0;

		/// <summary>
		///     Signals to reload interface
		/// </summary>
		private const int STATUS_REFRESH = 1;

		/// <summary>
		///     Interface status
		/// </summary>
		private static int Status;

		public static string? ReadInput(string prompt)
		{
			System.Console.Write("{0}: ", prompt);
			string? i = System.Console.ReadLine();

			return String.IsNullOrWhiteSpace(i) ? null : i;
		}

		/// <summary>
		///     Handles user input and options
		/// </summary>
		/// <param name="options">Array of <see cref="NConsoleOption" /></param>
		/// <param name="selectMultiple">Whether to return selected options as a <see cref="HashSet{T}" /></param>
		public static HashSet<object> ReadOptions<T>(IEnumerable<T> options, bool selectMultiple = false)
			where T : NConsoleOption
		{
			var i = new NConsoleInterface(options, null, null, selectMultiple, null);

			return ReadOptions(i);
		}

		/// <summary>
		///     Handles user input and options
		/// </summary>
		/// <param name="ui">
		///     <see cref="NConsoleInterface" />
		/// </param>
		public static HashSet<object> ReadOptions(NConsoleInterface ui)
		{
			// HACK: very hacky

			var selectedOptions = new HashSet<object>();


			/*
			 * Handle input
			 */


			ConsoleKeyInfo cki;

			do {
				System.Console.Clear();
				DisplayInterface(ui, selectedOptions);


				// Block until input is entered.
				while (!System.Console.KeyAvailable) {

					// HACK: hacky

					if (Interlocked.Exchange(ref Status, STATUS_OK) == STATUS_REFRESH) {
						System.Console.Clear();
						DisplayInterface(ui, selectedOptions);
					}
				}


				// Key was read

				cki = System.Console.ReadKey(true);

				if (cki.Key == NC_GLOBAL_REFRESH_KEY) {
					Refresh();
				}

				char keyChar = (char) (int) cki.Key;

				if (!Char.IsLetterOrDigit(keyChar)) {
					continue;
				}

				var modifiers = cki.Modifiers;


				bool altModifier  = modifiers.HasFlag(NConsoleOption.NC_ALT_FUNC_MODIFIER);
				bool ctrlModifier = modifiers.HasFlag(NConsoleOption.NC_CTRL_FUNC_MODIFIER);


				// Handle option

				int idx = GetIndexFromDisplayOption(keyChar);

				if (idx < ui.Length && idx >= 0) {

					var option = ui[idx];

					bool useAltFunc  = altModifier  && option.AltFunction  != null;
					bool useCtrlFunc = ctrlModifier && option.CtrlFunction != null;

					bool useComboFunc = altModifier && ctrlModifier && option.ComboFunction != null;

					if (useComboFunc) {
						var comboFunc = option.ComboFunction();

						//
					}
					else if (useCtrlFunc) {
						var ctrlFunc = option.CtrlFunction();

						//
					}
					else if (useAltFunc) {
						var altFunc = option.AltFunction();

						//
					}
					else {
						var funcResult = option.Function();

						if (funcResult != null) {
							//
							if (ui.SelectMultiple) {
								selectedOptions.Add(funcResult);
							}
							else {
								return new HashSet<object> {funcResult};
							}
						}
					}


				}


			} while (cki.Key != NC_GLOBAL_EXIT_KEY);

			return selectedOptions;
		}

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static bool ReadConfirmation(string msg, params object[] args)
		{
			WriteColor(Color.DeepSkyBlue, false,
				$"{Formatting.ASTERISK} {String.Format(msg, args)} ({OPTION_Y}/{OPTION_N}): ");

			char key = Char.ToUpper(System.Console.ReadKey().KeyChar);

			System.Console.WriteLine();

			return key switch
			{
				OPTION_N => false,
				OPTION_Y => true,
				_        => ReadConfirmation(msg, args)
			};

		}

		public static void Refresh()
		{
			Interlocked.Exchange(ref Status, STATUS_REFRESH);
		}

		public static void WaitForInput()
		{
			System.Console.WriteLine();
			System.Console.WriteLine("Press any key to continue...");
			System.Console.ReadKey();
		}

		public static void WaitForSecond()
		{
			Thread.Sleep(TimeSpan.FromSeconds(1));
		}

		/// <summary>
		/// Display interface <paramref name="ui"/>.
		/// </summary>
		/// <remarks>Used by <see cref="ReadOptions{T}"/></remarks>
		private static void DisplayInterface(NConsoleInterface ui, HashSet<object> selectedOptions)
		{
			if (ui.Name != null) {
				WriteColor(Color.Red, true, ui.Name);
			}


			for (int i = 0; i < ui.Length; i++) {
				var option = ui[i];

				string s = FormatOption(option, i);

				WriteColor(option.Color, false, s);
			}

			System.Console.WriteLine();

			// Show options
			if (ui.SelectMultiple) {
				string optionsStr = selectedOptions.QuickJoin();

				WriteColor(Color.LightBlue, true, optionsStr);
			}

			// Handle key reading

			if (ui.Prompt != null) {
				WriteSuccess(ui.Prompt);
			}

			if (ui.Status != null) {
				WriteInfo(ui.Status);
			}

			if (ui.SelectMultiple) {
				NewLine();
				WriteInfo($"Press {NC_GLOBAL_EXIT_KEY} to save selected values.");
			}
		}

		private static string FormatOption(NConsoleOption option, int i)
		{
			var  sb = new StringBuilder();
			char c  = GetDisplayOptionFromIndex(i);

			string name = option.Name;
			sb.Append($"[{c}]: {name} ");


			if (option.Data != null) {
				sb.Append(option.Data);
			}

			if (!sb.ToString().EndsWith(NativeNewLine)) {
				sb.AppendLine();
			}

			return FormatString(Formatting.ASTERISK, sb.ToString());
		}

		private static char GetDisplayOptionFromIndex(int i)
		{
			if (i < MAX_OPTION_N) {
				return Char.Parse(i.ToString());
			}

			int d = OPTION_LETTER_START + (i - MAX_OPTION_N);

			return (char) d;
		}

		private static int GetIndexFromDisplayOption(char c)
		{
			if (Char.IsNumber(c)) {
				return (int) Char.GetNumericValue(c);
			}

			if (Char.IsLetter(c)) {
				c = Char.ToUpper(c);
				return MAX_OPTION_N + (c - OPTION_LETTER_START);
			}

			return INVALID;
		}

		#endregion
	}
}