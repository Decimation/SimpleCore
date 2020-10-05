using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using SimpleCore.Internal;

// ReSharper disable UnusedMember.Global


namespace SimpleCore.Utilities
{
	/// <summary>
	/// Utilities for collections (<see cref="IEnumerable{T}"/>) and associated types.
	/// </summary>
	public static class Collections
	{
		public static T GetRandomElement<T>(this IList<T> rg)
		{
			var i = Common.Random.Next(0, rg.Count);

			return rg[i];
		}

		public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dic,
			TKey k, TValue d = default)
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