using JetBrains.Annotations;
using SimpleCore.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static SimpleCore.Internal.Common;

// ReSharper disable SwitchStatementMissingSomeEnumCasesNoDefault

// ReSharper disable InvocationIsSkipped
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable ParameterTypeCanBeEnumerable.Local
// ReSharper disable UnusedVariable
// ReSharper disable ParameterTypeCanBeEnumerable.Global

#pragma warning disable 8602, CA1416, CS8604
#nullable enable

namespace SimpleCore.Cli
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
	///     
	/// </list>
	public static class NConsole
	{
		#region Main

		/// <summary>
		///     Attempts to resize the console window
		/// </summary>
		/// <returns><c>true</c> if the operation succeeded</returns>
		public static bool Resize(int cww, int cwh)
		{
			bool canResize = Console.LargestWindowWidth  >= cww &&
			                 Console.LargestWindowHeight >= cwh;

			if (canResize) {
				Console.SetWindowSize(cww, cwh);
			}

			return canResize;
		}

		public static int BufferLimit { get; set; } = Console.BufferWidth - 10;

		/// <summary>
		///     Root write method.
		/// </summary>
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void Write(bool newLine, string msg, params object[] args)
		{
			string? fmt = String.Format(msg, args);

			if (newLine) {
				Console.WriteLine(fmt);
			}
			else {
				Console.Write(fmt);
			}
		}

		/// <summary>
		///     Root formatting function.
		/// </summary>
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static string FormatString(string c, string s)
		{
			string[]? srg = s.Split(Formatting.NativeNewLine);

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

			string s2 = String.Join(Formatting.NativeNewLine, srg);

			return s2;
		}

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static string FormatString(char c, string s) => FormatString(c.ToString(), s);

		public static void Init()
		{
			Console.OutputEncoding = Encoding.Unicode;
		}

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void Write(string msg, params object[] args) => Write(true, msg, args);

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteOnCurrentLine(string msg, params object[] args)
		{
			msg = String.Format(msg, args);

			string clear = new('\b', msg.Length);
			Console.Write(clear);
			Write(msg);
		}

		public static void Write(params object[] args)
		{
			string? s = args.QuickJoin();
			Write($"{s}");
		}

		#endregion Main

		#region IO

		#region Keys

		/// <summary>
		///     Exits <see cref="ReadOptions" />
		/// </summary>
		public const ConsoleKey NC_GLOBAL_EXIT_KEY = ConsoleKey.Escape;


		/// <summary>
		///     <see cref="Refresh" />
		/// </summary>
		public const ConsoleKey NC_GLOBAL_REFRESH_KEY = ConsoleKey.F5;

		/// <summary>
		///     Return
		/// </summary>
		public const ConsoleKey NC_GLOBAL_RETURN_KEY = ConsoleKey.F1;

		/// <summary>
		///     <see cref="NConsoleOption.AltFunction" />
		/// </summary>
		public const ConsoleModifiers NC_ALT_FUNC_MODIFIER = ConsoleModifiers.Alt;

		/// <summary>
		///     <see cref="NConsoleOption.CtrlFunction" />
		/// </summary>
		public const ConsoleModifiers NC_CTRL_FUNC_MODIFIER = ConsoleModifiers.Control;

		/// <summary>
		///     <see cref="NConsoleOption.ComboFunction" />
		/// </summary>
		public const ConsoleModifiers NC_COMBO_FUNC_MODIFIER = NC_ALT_FUNC_MODIFIER | NC_CTRL_FUNC_MODIFIER;

		#endregion Keys

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

		/*static void listen(Action x, Action<ConsoleKeyInfo> ck)
		{
			ConsoleKeyInfo cki;

			do {
				//io
				Console.Clear();
				x();

				// Block until input is entered.
				while (!Console.KeyAvailable) {
					// HACK: hacky
				}

				// Key was read

				cki = Console.ReadKey(true);

				// Handle special keys

				ck(cki);
			} while (cki.Key != ConsoleKey.Escape);
		}*/

		/*public static ConsoleKeyInfo Block()
		{
			ConsoleKeyInfo cki;

			do {
				// Block until input is entered.
				while (!Console.KeyAvailable) {
					// HACK: hacky
				}

				// Key was read

				cki = Console.ReadKey(true);
			} while (cki.Key != ConsoleKey.Escape);

			return cki;
		}*/

		/// <summary>
		///     Handles user input and options
		/// </summary>
		/// <param name="options">Array of <see cref="NConsoleOption" /></param>
		/// <param name="selectMultiple">Whether to return selected options as a <see cref="HashSet{T}" /></param>
		public static HashSet<object> ReadOptions(IList<NConsoleOption> options, bool selectMultiple)
		{
			// HACK: very hacky

			var selectedOptions = new HashSet<object>();

			/*
			 * Handle input
			 */

			ConsoleKeyInfo cki;

			do {
				DisplayInterface(options, selectedOptions, selectMultiple);

				// Block until input is entered.
				while (!Console.KeyAvailable) {
					// HACK: hacky

					if (Interlocked.Exchange(ref Status, STATUS_OK) == STATUS_REFRESH) {
						DisplayInterface(options, selectedOptions, selectMultiple);
					}
				}

				// Key was read

				cki = Console.ReadKey(true);

				// Handle special keys

				switch (cki.Key) {
					case NC_GLOBAL_REFRESH_KEY:
						Refresh();
						break;

					case NC_GLOBAL_RETURN_KEY:
						//todo
						return new HashSet<object> {true};
				}

				char keyChar = (char) (int) cki.Key;

				if (!Char.IsLetterOrDigit(keyChar)) {
					continue;
				}

				var modifiers = cki.Modifiers;

				bool altModifier  = modifiers.HasFlag(NC_ALT_FUNC_MODIFIER);
				bool ctrlModifier = modifiers.HasFlag(NC_CTRL_FUNC_MODIFIER);

				// Handle option

				int idx = GetIndexFromDisplayOption(keyChar);

				if (idx < options.Count && idx >= 0) {
					var option = options[idx];

					bool useAltFunc  = altModifier  && option.AltFunction  != null;
					bool useCtrlFunc = ctrlModifier && option.CtrlFunction != null;

					bool useComboFunc = altModifier && ctrlModifier && option.ComboFunction != null;

					if (useComboFunc) {
						object? comboFunc = option.ComboFunction();

						//
					}
					else if (useCtrlFunc) {
						object? ctrlFunc = option.CtrlFunction();

						//
					}
					else if (useAltFunc) {
						object? altFunc = option.AltFunction();

						//
					}
					else {
						object? funcResult = option.Function();

						if (funcResult != null) {
							//
							if (selectMultiple) {
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

		public static string? ReadInput(string? prompt = null)
		{
			if (prompt != null) {
				string? str = $"{prompt}: ";

				Console.Write(str);
			}

			string? i = Console.ReadLine();

			return String.IsNullOrWhiteSpace(i) ? null : i;
		}

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static bool ReadConfirmation(string msg, params object[] args)
		{
			Write($"{Formatting.ASTERISK} {String.Format(msg, args)} ({OPTION_Y}/{OPTION_N}): ");

			char key = Char.ToUpper(Console.ReadKey().KeyChar);

			Console.WriteLine();

			return key switch
			{
				OPTION_N => false,
				OPTION_Y => true,
				_        => ReadConfirmation(msg, args)
			};
		}

		public static void Refresh() => Interlocked.Exchange(ref Status, STATUS_REFRESH);

		public static void WaitForInput()
		{
			Console.WriteLine();
			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}

		public static void WaitForTimeSpan(TimeSpan span) => Thread.Sleep(span);

		public static void WaitForSecond() => WaitForTimeSpan(TimeSpan.FromSeconds(1));

		/// <summary>
		///     Display interface
		/// </summary>
		private static void DisplayInterface(IList<NConsoleOption> options, HashSet<object> selectedOptions,
		                                     bool selectMultiple)
		{
			Console.Clear();

			for (int i = 0; i < options.Count; i++) {
				var option = options[i];

				string s = FormatOption(option, i);

				Write(false, s);
			}

			Console.WriteLine();

			// Show options
			if (selectMultiple) {
				string optionsStr = selectedOptions.QuickJoin();

				Write(true, optionsStr);
			}

			// Handle key reading

			/*
			 * Auto resizing
			 */

			TryAutoResize(() => DisplayInterface(options, selectedOptions, selectMultiple));
		}

		private static bool TryAutoResize(Action write)
		{
			//todo: inline?
			if (AutoResizeHeight) {
				int correction = Console.CursorTop + AutoResizeMargin;

				if (Console.WindowHeight        != correction && !(correction < AutoResizeMinimumHeight) &&
				    Console.LargestWindowHeight >= correction) {
					//Console.SetWindowPosition(0, Console.CursorTop);
					Console.WindowHeight = correction;
					write();

					return true;
				}
			}

			return false;
		}

		public static int AutoResizeMargin { get; set; } = 1;

		public static int AutoResizeMinimumHeight { get; set; } = 20;

		public static bool AutoResizeHeight { get; set; } = false;

		#region Options

		public const char OPTION_N = 'N';
		public const char OPTION_Y = 'Y';

		private const int MAX_OPTION_N = 10;

		private const char OPTION_LETTER_START = 'A';

		public const int MAX_DISPLAY_OPTIONS = 36;

		#endregion Options

		private static string FormatOption(NConsoleOption option, int i)
		{
			var  sb = new StringBuilder();
			char c  = GetDisplayOptionFromIndex(i);

			string name = option.Name;
			sb.Append($"[{c}]: {name} ");

			if (option.Data != null) {
				sb.Append(option.Data);
			}

			if (!sb.ToString().EndsWith(Formatting.NativeNewLine)) {
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

		#endregion IO
	}
}