using System;
using System.Diagnostics;
using SimpleCore.Win32;

namespace SimpleCore.CommandLine.Shell
{
	/// <summary>
	/// Represents an executable <see cref="Process"/> run with the command prompt.
	/// </summary>
	public abstract class BaseCommand : IDisposable
	{
		public Process CommandProcess { get; protected set; }

		protected BaseCommand()
		{
		}

		public virtual void Dispose()
		{
			CommandProcess.ForceKill();
		}

		public virtual void Start()
		{
			CommandProcess.Start();
		}
	}
}
