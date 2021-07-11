using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using static SimpleCore.Internal.Common;
// ReSharper disable UnusedMember.Global

#nullable enable
namespace SimpleCore.Diagnostics
{
	public static class Log
	{
		[Conditional(TRACE_COND)]
		public static void WriteLine(string msg, string category, [CallerMemberName] string? caller = null)
		{
			Trace.WriteLine($"({caller}): {msg}", category);
		}


		[Conditional(TRACE_COND)]
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteLine(string msg, params object[] args)
		{
			Trace.WriteLine(String.Format(msg, args));
		}
	}
}