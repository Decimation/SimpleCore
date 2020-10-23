#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// ReSharper disable InconsistentNaming

namespace SimpleCore.CommandLine
{
	public class NConsoleUI
	{
		public static string DefaultName { get; set; } = Console.Title;

		public NConsoleUI(IEnumerable<NConsoleOption> options, string? name = null, string? prompt = null, bool selectMultiple = false)
		{
			Options = options;
			SelectMultiple = selectMultiple;
			Name = name ?? DefaultName;
			Prompt = prompt;
		}

		public IEnumerable<NConsoleOption> Options { get; }

		public bool SelectMultiple { get; }

		public string? Name { get; }

		public string? Prompt { get; }

		public NConsoleOption this[int i]
		{
			get
			{
				var o = Options.ElementAt(i);

				NConsoleOption.EnsureOption(ref o);

				return o;
			}
		}

		public int Length => Options.Count();
	}
}