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

		public NConsoleUI(IEnumerable<NConsoleOption> options,
			string? name,
			string? prompt,
			bool selectMultiple,
			string? status)
		{
			Options = options;
			SelectMultiple = selectMultiple;
			Name = name ?? DefaultName;
			Prompt = prompt;
			Status = status;
		}

		public NConsoleUI(IEnumerable<NConsoleOption> options) : this(options, null, null, false, null)
		{

		}

		public IEnumerable<NConsoleOption> Options { get; set; }

		public bool SelectMultiple { get; set; }

		public string? Name { get; set; }

		public string? Prompt { get; set; }

		public string? Status { get; set; }

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