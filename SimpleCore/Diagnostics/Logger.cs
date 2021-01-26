using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SimpleCore.Utilities;
using static SimpleCore.Internal.Common;

// ReSharper disable UnusedMember.Global
namespace SimpleCore.Diagnostics
{
	public static class Logger
	{
		public enum LogLevel
		{
			None,
			Info,
			Debug,
		}


		[Conditional(DEBUG_COND)]
		[StringFormatMethod(STRING_FORMAT_ARG)]
		public static void WriteLine(LogLevel ll, string msg, params object[] args)
		{
			var sb = new StringBuilder();

			string str = String.Format(msg, args);

			string label = ll switch
			{
				LogLevel.None  => Strings.Empty,
				LogLevel.Info  => "[info]",
				LogLevel.Debug => "[debug]",
				_              => throw new ArgumentOutOfRangeException(nameof(ll), ll, null)
			};

			sb.Append(label);
			sb.Append(Formatting.SPACE);
			sb.Append(str);

			string fullStr = sb.ToString();

			Trace.WriteLine(fullStr);
		}
	}
}