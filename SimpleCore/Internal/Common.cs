using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SimpleCore.Console")]
namespace SimpleCore.Internal
{
	/// <summary>
	/// Internal library common utilities, values, etc.
	/// </summary>
	internal static class Common
	{
		internal static readonly Random RandomInstance = new Random();

		internal const string DEBUG_COND = "DEBUG";

		internal const string STRING_FORMAT_ARG = "msg";

		/// <summary>
		/// Common integer value representing an invalid value, error, etc.
		/// </summary>
		internal const int INVALID = -1;
	}
}