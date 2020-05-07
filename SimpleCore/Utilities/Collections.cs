using System.Collections.Generic;

namespace SimpleCore.Utilities
{
	public static class Collections
	{
		public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dic,
		                                                     TKey                          k,
		                                                     TValue                        d = default(TValue))
		{
			if (!dic.ContainsKey(k)) {
				dic.Add(k, d);

				// for performance
				return d;
			}

			return dic[k];
		}
	}
}