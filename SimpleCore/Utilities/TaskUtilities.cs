using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace SimpleCore.Utilities
{
	public static class TaskUtilities
	{
		public static Task ForEachAsync<T>(this IEnumerable<T> sequence, Func<T, Task> action)
		{
			return Task.WhenAll(sequence.Select(action));
		}
	}
}
