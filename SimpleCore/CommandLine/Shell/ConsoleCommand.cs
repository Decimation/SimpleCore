using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
// ReSharper disable UnusedMember.Global
#nullable enable
namespace SimpleCore.CommandLine.Shell
{
	/// <summary>
	/// Represents an executable console command.
	/// </summary>
	public class ConsoleCommand : BaseCommand
	{
		public string Value { get; }

		private ConsoleCommand(string value, Process commandProcess)
		{
			Value = value;
			CommandProcess = commandProcess;
		}

		public ConsoleCommand(string value) : this(value, Command.Shell(value)) {}
	}
}