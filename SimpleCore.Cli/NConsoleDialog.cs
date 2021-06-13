using System.Collections.Generic;
using JetBrains.Annotations;

namespace SimpleCore.Cli
{
	public class NConsoleDialog
	{
		public IList<NConsoleOption> Options { get; init; }

		public bool SelectMultiple { get; init; }

		public string Header { get; set; }

		[CanBeNull]
		public string Status { get; set; }
	}
}