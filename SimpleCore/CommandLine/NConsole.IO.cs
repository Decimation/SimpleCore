﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using JetBrains.Annotations;
using SimpleCore.Internal;
using SimpleCore.Utilities;

// ReSharper disable InconsistentNaming

// ReSharper disable UseStringInterpolation

// ReSharper disable ParameterTypeCanBeEnumerable.Global
namespace SimpleCore.CommandLine
{
	public static partial class NConsole
	{
		/// <summary>
		///     Program functionality, IO, console interaction, console UI
		/// </summary>
		public static class IO
		{
			public const char CLI_CHAR = '*';

			public const string ALT_DENOTE = "[Alt]";

			private const int MAX_OPTION_N = 10;
			private const char OPTION_LETTER_START = 'A';
			private const int INVALID = -1;


			/// <summary>
			///     Escape -> quit
			/// </summary>
			private const ConsoleKey ESC_EXIT = ConsoleKey.Escape;

			/// <summary>
			///     Alt modifier -> View extra info
			/// </summary>
			private const ConsoleModifiers ALT_EXTRA = ConsoleModifiers.Alt;


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
			public static HashSet<object> HandleOptions(IEnumerable<NConsoleOption> options,
				bool selectMultiple = false)
			{
				var i = new NConsoleUI(options, null, selectMultiple);

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

					// @formatter:off � disable formatter after this line

					string prompt = String.Format("Enter the option number to open or {0} to exit.\n", ESC_EXIT) +
									String.Format("Hold down {0} while entering the option number to show more info.\n", ALT_EXTRA) +
									String.Format("Options with expanded information are denoted with {0}.", ALT_DENOTE);

					WriteSuccess(prompt);

					// @formatter:on � enable formatter after this line
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
						if (Interlocked.Exchange(ref Status, STATUS_OK) == STATUS_REFRESH) {
							Console.Clear();
							DisplayInterface();
						}
					}


					// Key was read

					cki = Console.ReadKey(true);
					char keyChar = cki.KeyChar;
					var modifiers = cki.Modifiers;
					bool altModifier = (modifiers & ALT_EXTRA) != 0;

					// Handle option

					int idx = GetIndexFromDisplayOption(keyChar);

					if (idx < io.Length && idx >= 0) {

						var option = io[idx];

						bool useAltFunc = altModifier && option.AltFunction != null;

						if (useAltFunc) {

							var altFunc = option.AltFunction()!;

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

			[StringFormatMethod(Common.STRING_FORMAT_ARG)]
			public static bool ReadConfirm(string msg, params object[] args)
			{
				Console.Clear();
				WriteInfo("{0} (y/n)", String.Format(msg, args));

				Console.WriteLine();


				char key = Char.ToLower(Console.ReadKey().KeyChar);

				Console.WriteLine();

				if (key == 'n') {
					return false;
				}

				if (key == 'y') {
					return true;
				}

				return ReadConfirm(msg, args);
			}
		}
	}
}