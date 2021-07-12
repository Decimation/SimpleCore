using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SimpleCore.Utilities;

namespace SimpleCore.Cli
{
	public static class NConsoleListener
	{
		public static ConsoleKeyInfo MonitorKeypress(CancellationTokenSource cancellationToken)
		{

			ConsoleKeyInfo cki;

			do {
				// true hides the pressed character from the console
				cki = Console.ReadKey(true);

				// Wait for an ESC
			} while (! Char.IsLetterOrDigit(cki.KeyChar));

			// Cancel the token
			cancellationToken.Cancel();
			return cki;
		}
	}
}