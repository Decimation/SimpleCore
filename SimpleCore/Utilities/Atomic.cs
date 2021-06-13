using System.Runtime.CompilerServices;
using System.Threading;

namespace SimpleCore.Utilities
{
	public static class Atomic
	{
		public static unsafe T Exchange<T>(ref T location1, T location2) where T : unmanaged
		{
			fixed (T* p = &location1) {
				var value = Interlocked.Exchange(ref *(int*) p, Unsafe.Read<int>(&location2));

				return Unsafe.Read<T>(&value);
			}

		}
	}
}