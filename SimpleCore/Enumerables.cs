using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCore
{
	public static class Enumerables
	{
		public static bool IsNullOrEmpty<T>(IEnumerable<T> values)
		{
			return values == null || !values.Any();
		}
	}
}