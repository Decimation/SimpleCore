using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using JetBrains.Annotations;
using SimpleCore.Utilities;
using static SimpleCore.Internal.Common;

// ReSharper disable SwitchStatementMissingSomeEnumCasesNoDefault

// ReSharper disable InvocationIsSkipped
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable ParameterTypeCanBeEnumerable.Local
// ReSharper disable UnusedVariable
// ReSharper disable ParameterTypeCanBeEnumerable.Global

#pragma warning disable 8602, CA1416, CS8604, IDE0059
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
	///     <item>
	///         <description>
	///             <see cref="NConsoleDialog" />
	///         </description>
	///     </item>
	/// <item>
	///         <description>
	///             <see cref="NConsoleProgress" />
	///         </description>
	///     </item>
	/// </list>
	public static class NConsole
	{
		/*
		 * https://github.com/Decimation/SimpleCore/blob/2d6009cfc498de07d5f507192c3cbe1983ff1a11/SimpleCore.Cli/NConsole.cs
		 */

		public static void Init()
		{
			//Console.OutputEncoding = Encoding.Unicode;
		}

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

		private static readonly Color ColorHeader  = Color.Red;
		private static readonly Color ColorOptions = Color.Aquamarine;

		#region Write

		/// <summary>
		///     Root write method.
		/// </summary>
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void Write(bool newLine, string msg, params object[] args)
		{
			string fmt = String.Format(msg, args);

			if (newLine) {
				Console.WriteLine(fmt);
			}
			else {
				Console.Write(fmt);
			}
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

		#endregion

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
		///     <see cref="NConsoleOption.ShiftFunction" />
		/// </summary>
		public const ConsoleModifiers NC_SHIFT_FUNC_MODIFIER = ConsoleModifiers.Shift;

		/// <summary>
		///     <see cref="NConsoleOption.ComboFunction" />
		/// </summary>
		public const ConsoleModifiers NC_COMBO_FUNC_MODIFIER = NC_ALT_FUNC_MODIFIER | NC_CTRL_FUNC_MODIFIER;

		#endregion Keys

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


		#region Display/formatting

		/// <summary>
		///     Root formatting function.
		/// </summary>
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static string FormatString(string delim, string s)
		{
			string[] split = s.Split(StringConstants.NativeNewLine);

			for (int i = 0; i < split.Length; i++) {
				string a = StringConstants.SPACE + split[i];

				string b;

				if (String.IsNullOrWhiteSpace(a)) {
					b = String.Empty;
				}
				else {
					b = delim + a;
				}

				string c = b.Truncate(BufferLimit);

				if (c.Length < b.Length) {
					c += StringConstants.ELLIPSES;
				}

				split[i] = c;
			}

			return String.Join(StringConstants.NativeNewLine, split);
		}

		private static string FormatOption(NConsoleOption option, int i)
		{
			var  sb = new StringBuilder();
			char c  = GetDisplayOptionFromIndex(i);

			string? name = option.Name;

			if (option.Color.HasValue) {
				name = name.AddColor(option.Color.Value);
			}

			sb.Append($"[{c}]: ");

			if (name != null) {
				sb.Append($"{name} ");
			}

			if (option.Data != null) {
				// if (!sb.ToString().EndsWith("\n") || !option.Data.StartsWith("\n")) {
				// 	sb.Append("\n");
				// }

				sb.AppendLine();
				sb.Append($"{option.Data}");
			}

			if (!sb.ToString().EndsWith(StringConstants.NativeNewLine)) {
				sb.AppendLine();
			}

			return FormatString(StringConstants.ASTERISK.ToString(), sb.ToString());
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

		/// <summary>
		///     Display dialog
		/// </summary>
		private static void DisplayDialog(NConsoleDialog dialog, HashSet<object> selectedOptions)
		{
			Console.Clear();

			if (dialog.Header is { }) {
				Write(false, dialog.Header.AddColor(ColorHeader));
			}

			var clamp = Math.Clamp(dialog.Options.Count, 0, MAX_DISPLAY_OPTIONS);

			for (int i = 0; i < clamp; i++) {
				var option = dialog.Options[i];

				string s = FormatOption(option, i);

				Write(false, s);
			}

			Console.WriteLine();

			if (dialog.Status != null) {
				Write(dialog.Status);
			}

			// Show options
			if (dialog.SelectMultiple) {
				string optionsStr = $">> {selectedOptions.QuickJoin()}".AddColor(ColorOptions);

				Write(true, optionsStr);
			}

			if (dialog.SelectMultiple) {
				Console.WriteLine();
				Write($"Press {NC_GLOBAL_EXIT_KEY.ToString().AddUnderline()} to save selected values.");
			}
		}

		#endregion

		/// <summary>
		///     Handles user input and options
		/// </summary>
		/// <remarks>Returns when <see cref="NConsoleOption.Function"/> returns a non-null value</remarks>
		public static HashSet<object> ReadOptions(NConsoleDialog dialog)
		{
			var selectedOptions = new HashSet<object>();

			/*
			 * Handle input
			 */

			ConsoleKeyInfo cki;

			do {
				DisplayDialog(dialog, selectedOptions);

				// Block until input is entered.
				while (!Console.KeyAvailable) {

					// Handle signals from other threads
					if (Atomic.Exchange(ref Status, ConsoleStatus.Ok) == ConsoleStatus.Refresh) {
						DisplayDialog(dialog, selectedOptions);
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

				// KeyChar can't be used as modifiers are not applicable
				char keyChar = (char) (int) cki.Key;


				if (!Char.IsLetterOrDigit(keyChar)) {
					continue;
				}

				var modifiers = cki.Modifiers;

				bool altModifier   = modifiers.HasFlag(NC_ALT_FUNC_MODIFIER);
				bool ctrlModifier  = modifiers.HasFlag(NC_CTRL_FUNC_MODIFIER);
				bool shiftModifier = modifiers.HasFlag(NC_SHIFT_FUNC_MODIFIER);

				// Handle option

				int idx = GetIndexFromDisplayOption(keyChar);

				if (idx < dialog.Options.Count && idx >= 0) {
					var option = dialog.Options[idx];

					bool useAltFunc   = altModifier   && option.AltFunction   != null;
					bool useCtrlFunc  = ctrlModifier  && option.CtrlFunction  != null;
					bool useShiftFunc = shiftModifier && option.ShiftFunction != null;

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
					else if (useShiftFunc) {
						object? shiftFunc = option.ShiftFunction();

						//
					}
					else {
						object? funcResult = option.Function();

						if (funcResult != null) {
							//
							if (dialog.SelectMultiple) {
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


		public static string ReadInput(string? prompt = null, Func<string, bool>? invalid = null)
		{
			invalid ??= String.IsNullOrWhiteSpace;

			string? input;
			bool    isInvalid;


			do {
				//https://stackoverflow.com/questions/8946808/can-console-clear-be-used-to-only-clear-a-line-instead-of-whole-console

				Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");

				if (prompt != null) {
					string str = $"{prompt}: ";

					Console.Write(str);
				}

				input     = Console.ReadLine();
				isInvalid = invalid(input);

				if (isInvalid) {
					Console.CursorTop--;

				}

			} while (isInvalid);


			return input;
		}


		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static bool ReadConfirmation(string msg, params object[] args)
		{
			Write($"{StringConstants.ASTERISK} {String.Format(msg, args)} ({OPTION_Y}/{OPTION_N}): ");

			char key = Char.ToUpper(Console.ReadKey().KeyChar);

			Console.WriteLine();

			return key switch
			{
				OPTION_N => false,
				OPTION_Y => true,
				_        => ReadConfirmation(msg, args)
			};
		}

		public static void Refresh() => Atomic.Exchange(ref Status, ConsoleStatus.Refresh);

		public static void WaitForInput()
		{
			Console.WriteLine();
			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}

		public static void WaitForTimeSpan(TimeSpan span) => Thread.Sleep(span);

		public static void WaitForSecond() => WaitForTimeSpan(TimeSpan.FromSeconds(1));

		#region Status

		/// <summary>
		///     Interface status
		/// </summary>
		private static ConsoleStatus Status;

		private enum ConsoleStatus
		{
			/// <summary>
			///     Signals to reload interface
			/// </summary>
			Refresh,

			/// <summary>
			///     Signals to continue displaying current interface
			/// </summary>
			Ok
		}

		#endregion

		#region Options

		public const char OPTION_N = 'N';

		public const char OPTION_Y = 'Y';

		private const int MAX_OPTION_N = 10;

		private const char OPTION_LETTER_START = 'A';

		public const int MAX_DISPLAY_OPTIONS = 36;

		#endregion Options

		#endregion IO


		/*
		 * https://github.com/sindresorhus/cli-spinners/blob/main/spinners.json
		 * https://github.com/sindresorhus/cli-spinners
		 * https://www.npmjs.com/package/cli-spinners
		 */
	}
}