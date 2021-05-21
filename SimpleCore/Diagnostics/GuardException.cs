#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

// ReSharper disable UnusedMember.Global

namespace SimpleCore.Diagnostics
{
	public sealed class GuardException : Exception
	{
		public GuardException() { }
		public GuardException([CanBeNull] string? message) : base(message) { }
	}
}