using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using SimpleCore.Internal;
using static SimpleCore.Internal.Common;

// ReSharper disable UnusedMember.Global


namespace SimpleCore.Utilities
{
	/// <summary>
	/// Utilities for collections (<see cref="IEnumerable{T}"/>) and associated types.
	/// </summary>
	public static class Collections
	{
		/// <summary>
		/// Determines whether <paramref name="list"/> ends with <paramref name="sequence"/>.
		/// </summary>
		/// <typeparam name="T"><see cref="List{T}"/> type</typeparam>
		/// <param name="list">Larger <see cref="List{T}"/></param>
		/// <param name="sequence">Smaller <see cref="List{T}"/></param>
		/// <returns><c>true</c> if <paramref name="list"/> ends with <paramref name="sequence"/>; <c>false</c> otherwise</returns>
		public static bool EndsWith<T>(this IList<T> list, IList<T> sequence)
		{
			return list.TakeLast(sequence.Count).SequenceEqual(sequence);
		}

		/// <summary>
		/// Determines whether <paramref name="list"/> starts with <paramref name="sequence"/>.
		/// </summary>
		/// <typeparam name="T"><see cref="List{T}"/> type</typeparam>
		/// <param name="list">Larger <see cref="List{T}"/></param>
		/// <param name="sequence">Smaller <see cref="List{T}"/></param>
		/// <returns><c>true</c> if <paramref name="list"/> starts with <paramref name="sequence"/>; <c>false</c> otherwise</returns>
		public static bool StartsWith<T>(this IList<T> list, IList<T> sequence)
		{
			return list.Take(sequence.Count).SequenceEqual(sequence);
		}

		/// <summary>
		/// Retrieves a random element from <paramref name="list"/>.
		/// </summary>
		/// <typeparam name="T"><see cref="List{T}"/> type</typeparam>
		/// <param name="list"><see cref="List{T}"/> from which to retrieve a random element</param>
		/// <returns>A random element</returns>
		public static T GetRandomElement<T>(this IList<T> list)
		{
			var i = RandomInstance.Next(0, list.Count);

			return list[i];
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