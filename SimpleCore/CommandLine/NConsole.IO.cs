using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using JetBrains.Annotations;
using SimpleCore.Utilities;
using static SimpleCore.Internal.Common;
using static SimpleCore.CommandLine.NConsole;
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable UseStringInterpolation
// ReSharper disable ParameterTypeCanBeEnumerable.Global

#pragma warning disable HAA0601, HAA0502, HAA0101

namespace SimpleCore.CommandLine
{
	/// <summary>
	///     Program functionality, IO, console interaction, console UI
	/// </summary>
	public static class NConsoleIO
	{
		private const int MAX_OPTION_N = 10;

		private const char OPTION_LETTER_START = 'A';


		/// <summary>
		///     Exits <see cref="HandleOptions{T}"/>
		/// </summary>
		public const ConsoleKey NC_GLOBAL_EXIT_KEY = ConsoleKey.Escape;

		/// <summary>
		/// <see cref="Refresh"/>
		/// </summary>
		public const ConsoleKey NC_GLOBAL_REFRESH_KEY = ConsoleKey.F5;


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
			if (i < MAX_OPTION_N)
			{
				return Char.Parse(i.ToString());
			}

			int d = OPTION_LETTER_START + (i - MAX_OPTION_N);

			return (char)d;
		}

		private static int GetIndexFromDisplayOption(char c)
		{
			if (Char.IsNumber(c))
			{
				return (int)Char.GetNumericValue(c);
			}

			if (Char.IsLetter(c))
			{
				c = Char.ToUpper(c);
				return MAX_OPTION_N + (c - OPTION_LETTER_START);
			}

			return INVALID;
		}


		private static char Parse(ConsoleKey key)
		{
			return (char)(int)key;
		}

		private static string FormatOption(NConsoleOption option, int i)
		{
			var sb = new StringBuilder();
			char c = GetDisplayOptionFromIndex(i);

			//todo

			string name = option.Name;
			sb.AppendFormat("[{0}]: {1} ", c, name);


			if (option.Data != null)
			{
				sb.Append(option.Data);
			}

			if (!sb.ToString().EndsWith("\n"))
			{
				sb.AppendLine();
			}

			return NConsole.FormatString(NConsole.CLI_CHAR, sb.ToString());
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
			var i = new NConsoleUI(options, null, null, selectMultiple, null);

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

				for (int i = 0; i < io.Length; i++)
				{
					var option = io[i];

					string s = FormatOption(option, i);


					WriteColor(option.Color, false, s);

				}

				Console.WriteLine();

				// Show options
				if (io.SelectMultiple)
				{
					string optionsStr = selectedOptions.QuickJoin();

					WriteColor(Color.LightBlue, true, optionsStr);
				}

				// Handle key reading

				if (io.Prompt != null)
				{
					WriteSuccess(io.Prompt);
				}

				if (io.Status != null)
				{
					WriteInfo(io.Status);
				}
			}


			/*
			 * Handle input
			 */


			ConsoleKeyInfo cki;

			do
			{
				Console.Clear();


				DisplayInterface();

				while (!Console.KeyAvailable)
				{
					// Block until input is entered.

					//

					// todo: hacky
					if (Interlocked.Exchange(ref Status, STATUS_OK) == STATUS_REFRESH)
					{
						Console.Clear();
						DisplayInterface();
					}
				}


				// Key was read

				// todo: use cki.Key?

				cki = Console.ReadKey(true);

				if (cki.Key == NC_GLOBAL_REFRESH_KEY)
				{
					Refresh();
				}

				char keyChar = Parse(cki.Key);
				var modifiers = cki.Modifiers;

				bool altModifier = (modifiers & NConsoleOption.NC_ALT_FUNC_MODIFIER) != 0;
				bool ctrlModifier = (modifiers & NConsoleOption.NC_CTRL_FUNC_MODIFIER) != 0;

				// Handle option

				int idx = GetIndexFromDisplayOption(keyChar);

				if (idx < io.Length && idx >= 0)
				{

					var option = io[idx];

					bool useAltFunc = altModifier && option.AltFunction != null;
					bool useCtrlFunc = ctrlModifier && option.CtrlFunction != null;

					if (useAltFunc)
					{

						var altFunc = option.AltFunction()!;


						//
					}
					else if (useCtrlFunc)
					{
						var ctrlFunc = option.CtrlFunction()!;

						//
					}
					else
					{
						var funcResult = option.Function()!;

						if (funcResult != null)
						{
							//
							if (io.SelectMultiple)
							{
								selectedOptions.Add(funcResult);
							}
							else
							{
								return new HashSet<object> { funcResult };
							}
						}
					}


				}


			} while (cki.Key != NC_GLOBAL_EXIT_KEY);

			return selectedOptions;
		}

		public const char OPTION_Y = 'Y';

		public const char OPTION_N = 'N';

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static bool ReadConfirm(string msg, params object[] args)
		{
			WriteColor(Color.DeepSkyBlue, false, "{0} ({1}/{2}): ", String.Format(msg, args), OPTION_Y, OPTION_N);

			char key = Char.ToUpper(Console.ReadKey().KeyChar);

			Console.WriteLine();

			return key switch
			{
				OPTION_N => false,
				OPTION_Y => true,
				_ => ReadConfirm(msg, args)
			};

		}
	}
}