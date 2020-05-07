using System;
using System.Diagnostics;
using System.Text;
using JetBrains.Annotations;

namespace SimpleCore.Utilities
{
	public static class CliOutput
	{
		private const string STRING_FORMAT_ARG = "msg";

		private const char HEAVY_BALLOT_X   = '\u2718';
		private const char HEAVY_CHECK_MARK = '\u2714';

		public const char MUL_SIGN = '\u00D7';
		public const char RAD_SIGN = '\u221A';

		private const char GT       = '>';
		private const char ASTERISK = '*';

		private const string DEBUG_COND = "DEBUG";

		static CliOutput()
		{
			Name = "<unknown>";
		}

		public static string Name { get; private set; }

		[StringFormatMethod(STRING_FORMAT_ARG)]
		private static string Prepare(char c, string s)
		{
			var srg = s.Split('\n');

			for (int i = 0; i < srg.Length; i++) {
				srg[i] = c + " " + srg[i];
			}

			var s2 = string.Join('\n', srg);
			
			return s2;
		}
		// Single line
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void OnCurrentLine(ConsoleColor color, string msg, params object[] args)
		{
			msg = string.Format(msg, args);

			var clear = new string('\b', msg.Length);
			Console.Write(clear);

			WithColor(color, () =>
			{
				//
				Console.Write(msg);
			});
		}

		[Conditional(DEBUG_COND)]
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteDebug(string msg, params object[] args)
		{
			WithColor(ConsoleColor.Cyan, () =>
			{
				//
				string s = Prepare(MUL_SIGN, string.Format(msg, args));
				Console.WriteLine(s);
			});
		}

		[Conditional(DEBUG_COND)]
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteDebug(object obj) => WriteDebug(msg: obj.ToString());

		// Single line
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static bool ReadConfirm(string msg, params object[] args)
		{
			Console.Clear();
			CliOutput.WriteInfo("{0} (y/n)", string.Format(msg, args));

			Console.WriteLine();


			char key = char.ToLower(Console.ReadKey().KeyChar);

			Console.WriteLine();

			if (key == 'n') {
				return false;
			}
			else if (key == 'y') {
				return true;
			}
			else {
				return ReadConfirm(msg, args);
			}
		}

		public static void Init(string name, bool clear = true)
		{
			Name                   = name;
			Console.Title          = name;
			Console.OutputEncoding = Encoding.Unicode;
			if (clear) {
				Console.Clear();
			}
		}

		public static void WithColor(ConsoleColor color, Action func)
		{
			var oldColor = Console.ForegroundColor;
			Console.ForegroundColor = color;
			func();
			Console.ForegroundColor = oldColor;
		}

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteError(string msg, params object[] args)
		{
			WithColor(ConsoleColor.Red, () =>
			{
				//
				string s = Prepare(MUL_SIGN, string.Format(msg, args));
				Console.WriteLine(s);
			});
		}

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteError(object obj) => WriteError(obj.ToString());

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteInfo(string msg, params object[] args)
		{
			WithColor(ConsoleColor.White, () =>
			{
				//
				string s = Prepare(ASTERISK, string.Format(msg, args));
				Console.WriteLine(s);
			});
		}

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteInfo(object obj) => WriteInfo(obj.ToString());

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WritePretty(object obj) => WritePretty(obj.ToString());

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WritePretty(string msg, params object[] args)
		{
			if (args?.Length > 0) {
				if (args[0] is bool b) {
					args[0] = b ? RAD_SIGN : MUL_SIGN;
				}
			}


			WithColor(ConsoleColor.White, () =>
			{
				//
				Console.WriteLine("{0} {1}", ASTERISK, string.Format(msg, args));
			});
		}

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteSuccess(object obj) => WriteSuccess(obj.ToString());

		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteSuccess(string msg, params object[] args)
		{
			WithColor(ConsoleColor.Green, () =>
			{
				//
				string s = Prepare(RAD_SIGN, string.Format(msg, args));
				Console.WriteLine(s);
			});
		}
	}
}