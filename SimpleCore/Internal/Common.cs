﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// ReSharper disable UnusedMember.Global

[assembly: InternalsVisibleTo("Novus")]
[assembly: InternalsVisibleTo("SimpleCore.Net")]
[assembly: InternalsVisibleTo("SimpleCore.Cli")]

namespace SimpleCore.Internal
{
	/// <summary>
	/// Internal library common utilities, values, etc.
	/// </summary>
	internal static class Common
	{
		internal static readonly Random RandomInstance = new();

		internal const string DEBUG_COND = "DEBUG";

		internal const string TRACE_COND = "TRACE";

		internal const string STRING_FORMAT_ARG = "msg";

		/// <summary>
		/// Common integer value representing an invalid value, error, etc.
		/// </summary>
		internal const int INVALID = -1;
	}
}