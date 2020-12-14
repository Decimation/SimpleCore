using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
		public static bool EndsWith<T>(this IList<T> list, IList<T> sequence) =>
			list.TakeLast(sequence.Count).SequenceEqual(sequence);

		/// <summary>
		/// Determines whether <paramref name="list"/> starts with <paramref name="sequence"/>.
		/// </summary>
		/// <typeparam name="T"><see cref="List{T}"/> type</typeparam>
		/// <param name="list">Larger <see cref="List{T}"/></param>
		/// <param name="sequence">Smaller <see cref="List{T}"/></param>
		/// <returns><c>true</c> if <paramref name="list"/> starts with <paramref name="sequence"/>; <c>false</c> otherwise</returns>
		public static bool StartsWith<T>(this IList<T> list, IList<T> sequence) =>
			list.Take(sequence.Count).SequenceEqual(sequence);

		/// <summary>
		/// Retrieves a random element from <paramref name="list"/>.
		/// </summary>
		/// <typeparam name="T"><see cref="List{T}"/> type</typeparam>
		/// <param name="list"><see cref="List{T}"/> from which to retrieve a random element</param>
		/// <returns>A random element</returns>
		public static T GetRandomElement<T>(this IList<T> list)
		{
			var i = RandomInstance.Next(list.Count);

			return list[i];
		}

		private const string DICT_DELIM = "=";

		public static bool IndexOutOfBounds<T>(this IList<T> rg, int idx)
		{
			//idx < io.Length && idx >= 0
			//(idx < rg.Count && idx >= 0)
			//!(idx > rg.Count || idx < 0)

			return idx < rg.Count && idx >= 0;
		}

		/// <summary>
		/// Writes a <see cref="Dictionary{TKey,TValue}"/> to file <paramref name="filename"/>.
		/// </summary>
		public static void WriteDictionary(IDictionary<string, string> dictionary, string filename)
		{
			string[] lines = dictionary.Select(kvp => kvp.Key + DICT_DELIM + kvp.Value).ToArray();
			File.WriteAllLines(filename, lines);
		}

		/// <summary>
		/// Reads a <see cref="Dictionary{TKey,TValue}"/> written by <see cref="WriteDictionary"/> to <paramref name="filename"/>.
		/// </summary>
		public static IDictionary<string, string> ReadDictionary(string filename)
		{
			string[] lines = File.ReadAllLines(filename);

			var dict = lines.Select(l => l.Split(DICT_DELIM))
				.ToDictionary(a => a[0], a => a[1]);

			return dict;
		}

		public static float Distance(byte[] first, byte[] second)
		{
			int sum = 0;

			// We'll use which ever array is shorter.
			int length = first.Length > second.Length ? second.Length : first.Length;

			for (int x = 0; x < length; x++) {
				sum += (int) Math.Pow((first[x] - second[x]), 2);
			}

			return sum / (float) length;
		}

		public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dic,
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