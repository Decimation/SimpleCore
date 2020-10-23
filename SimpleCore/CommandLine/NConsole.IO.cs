using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using JetBrains.Annotations;
using SimpleCore.Utilities;
using static SimpleCore.Internal.Common;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable UseStringInterpolation
// ReSharper disable ParameterTypeCanBeEnumerable.Global

#pragma warning disable HAA0601, HAA0502

namespace SimpleCore.CommandLine
{
	public static partial class NConsole
	{
		/// <summary>
		///     Program functionality, IO, console interaction, console UI
		/// </summary>
		public static class IO
		{
			private const int MAX_OPTION_N = 10;

			private const char OPTION_LETTER_START = 'A';


			/// <summary>
			///     Escape -> quit
			/// </summary>
			public const ConsoleKey ESC_EXIT = ConsoleKey.Escape;

			/// <summary>
			///     Alt modifier -> <see cref="NConsoleOption.AltFunction"/>
			/// </summary>
			public const ConsoleModifiers ALT_FUNC_MODIFIER = ConsoleModifiers.Alt;

			/// <summary>
			///     Ctrl modifier -> <see cref="NConsoleOption.CtrlFunction"/>
			/// </summary>
			public const ConsoleModifiers CTRL_FUNC_MODIFIER = ConsoleModifiers.Control;


			// todo

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

			public static string GetInput(string prompt)
			{
				Console.Write("{0}: ", prompt);
				string i = Console.ReadLine();

				return String.IsNullOrWhiteSpace(i) ? null : i;

			}


			public static void WaitForInput()
			{
				Console.WriteLine();
				Console.WriteLine("Press any key to continue...");
				Console.ReadLine();
			}

			public static void WaitForSecond()
			{
				Thread.Sleep(TimeSpan.FromSeconds(1));
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
					int idx = (int) Char.GetNumericValue(c);
					return idx;
				}

				if (Char.IsLetter(c)) {
					c = Char.ToUpper(c);
					int d = MAX_OPTION_N + (c - OPTION_LETTER_START);

					return d;
				}

				return INVALID;
			}


			private static char Parse(ConsoleKey key)
			{
				return (char) (int) key;
			}

			private static string FormatOption(NConsoleOption option, int i)
			{
				var sb = new StringBuilder();
				char c = GetDisplayOptionFromIndex(i);

				//todo

				string name = option.Name;
				sb.AppendFormat("[{0}]: {1} ", c, name);


				if (option.Data != null) {
					sb.Append(option.Data);
				}

				if (!sb.ToString().EndsWith("\n")) {
					sb.AppendLine();
				}

				string s = FormatString(CLI_CHAR, sb.ToString());

				return s;
			}

			public static void Refresh()
			{
				Interlocked.Exchange(ref Status, STATUS_REFRESH);
			}

			/// <summary>
			///     Handles user input and options
			/// </summary>
			/// <param name="options">Array of <see cref="NConsoleOption" /></param>
			/// <param name="selectMultiple">Whether to return selected options as a <see cref="HashSet{T}" /></param>
			public static HashSet<object> HandleOptions<T>(IEnumerable<T> options,
				bool selectMultiple = false) where T : NConsoleOption
			{
				var i = new NConsoleUI(options, null, null, selectMultiple);

				return HandleOptions(i);
			}

			/// <summary>
			///     Handles user input and options
			/// </summary>
			/// <param name="io">
			///     <see cref="NConsoleUI" />
			/// </param>
			public static HashSet<object> HandleOptions(NConsoleUI io)
			{
				// todo: very hacky

				var selectedOptions = new HashSet<object>();

				void DisplayInterface()
				{
					//NConsole.Extended.WriteColor(Color.Red, true, RuntimeInfo.NAME_BANNER);
					WriteColor(Color.Red, true, io.Name);

					for (int i = 0; i < io.Length; i++) {
						var option = io[i];

						string s = FormatOption(option, i);


						WriteColor(option.Color, false, s);

					}

					Console.WriteLine();

					// Show options
					if (io.SelectMultiple) {
						string optionsStr = selectedOptions.QuickJoin();

						WriteColor(Color.LightBlue, true, optionsStr);
					}

					// Handle key reading


					// @formatter:off disable formatter after this line

					// string prompt = String.Format("Enter the option number to open or {0} to exit.\n", ESC_EXIT) +
					// 				String.Format("Hold down {0} while entering the option number to show more info.\n", ALT_FUNC_MODIFIER) +
					// 				String.Format("Options with expanded information are denoted with {0}.", ALT_DENOTE);
					//
					// WriteSuccess(prompt);

					if (io.Prompt != null)
					{
						WriteSuccess(io.Prompt);
					}

					// @formatter:on enable formatter after this line

				}


				/*
				 * Handle input
				 */


				ConsoleKeyInfo cki;

				do {
					Console.Clear();


					DisplayInterface();

					while (!Console.KeyAvailable) {
						// Block until input is entered.

						//

						// todo: hacky
						if (Interlocked.Exchange(ref Status, STATUS_OK) == STATUS_REFRESH) {
							Console.Clear();
							DisplayInterface();
						}
					}


					// Key was read

					// todo: use cki.Key?

					cki = Console.ReadKey(true);
					//char keyChar = cki.KeyChar;
					char keyChar = Parse(cki.Key);
					var modifiers = cki.Modifiers;

					bool altModifier = (modifiers & ALT_FUNC_MODIFIER) != 0;
					bool ctrlModifier = (modifiers & CTRL_FUNC_MODIFIER) != 0;

					// Handle option

					int idx = GetIndexFromDisplayOption(keyChar);

					if (idx < io.Length && idx >= 0) {

						var option = io[idx];

						bool useAltFunc = altModifier && option.AltFunction != null;
						bool useCtrlFunc = ctrlModifier && option.CtrlFunction != null;

						if (useAltFunc) {

							var altFunc = option.AltFunction()!;


							//
						}
						else if (useCtrlFunc) {
							var ctrlFunc = option.CtrlFunction()!;

							//
						}
						else {
							var funcResult = option.Function()!;

							if (funcResult != null) {
								//
								if (io.SelectMultiple) {
									selectedOptions.Add(funcResult);
								}
								else {
									return new HashSet<object> {funcResult};
								}
							}
						}


					}


				} while (cki.Key != ESC_EXIT);

				return selectedOptions;
			}

			public const char OPTION_Y = 'Y';
			public const char OPTION_N = 'N';

			[StringFormatMethod(STRING_FORMAT_ARG)]
			public static bool ReadConfirm(string msg, params object[] args)
			{
				WriteInfo("{0} ({1}/{2})", String.Format(msg, args),OPTION_Y, OPTION_N);

				Console.WriteLine();

				char key = Char.ToUpper(Console.ReadKey().KeyChar);

				Console.WriteLine();

				if (key == OPTION_N) {
					return false;
				}

				if (key == OPTION_Y) {
					return true;
				}

				return ReadConfirm(msg, args);
			}
		}
	}
}