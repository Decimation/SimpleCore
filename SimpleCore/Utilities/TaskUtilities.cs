using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

		/*
		 * https://stackoverflow.com/questions/35645899/awaiting-task-with-timeout
		 * https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.run?view=net-5.0
		 * https://devblogs.microsoft.com/pfxteam/crafting-a-task-timeoutafter-method/
		 * https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/cancel-async-tasks-after-a-period-of-time
		 * https://stackoverflow.com/questions/20638952/cancellationtoken-and-cancellationtokensource-how-to-use-it
		 * https://stackoverflow.com/questions/22637642/using-cancellationtoken-for-timeout-in-task-run-does-not-work
		 * https://stackoverflow.com/questions/32520612/how-to-handle-task-cancellation-in-the-tpl
		 */

		public static async Task AwaitWithTimeout(this Task task, int timeout, Action success, Action error)
		{
			if (await Task.WhenAny(task, Task.Delay(timeout)) == task) {
				success();
			}
			else {
				error();
			}
		}
	}
}