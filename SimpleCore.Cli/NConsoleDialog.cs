using System.Collections.Generic;

namespace SimpleCore.Cli
{
	public class NConsoleDialog
	{
		public IList<NConsoleOption> Options { get; init; }

		public bool SelectMultiple { get; init; }

		public string Header { get; init; }
	}
}