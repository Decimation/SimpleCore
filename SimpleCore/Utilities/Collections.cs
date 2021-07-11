using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SimpleCore.Diagnostics;
using static SimpleCore.Internal.Common;
using Map= System.Collections.Generic.Dictionary<object, object>;
// ReSharper disable AssignNullToNotNullAttribute

// ReSharper disable PossibleMultipleEnumeration

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

		public static object[] CastObjectArray(this Array r)
		{
			var rg = new object[r.Length];

			for (int i = 0; i < r.Length; i++) {
				rg[i] = r.GetValue(i);
			}

			return rg;
		}

		public static IEnumerable<int> AllIndexesOf<T>(this List<T> list, T search)
		{
			int minIndex = list.IndexOf(search);

			while (minIndex != -1) {
				yield return minIndex;
				minIndex = list.IndexOf(search, minIndex + 1);
			}
		}

		/// <summary>
		/// Replaces all occurrences of sequence <paramref name="sequence"/> within <paramref name="rg"/> with <paramref name="replace"/>.
		/// </summary>
		/// <param name="rg">Original <see cref="List{T}"/></param>
		/// <param name="sequence">Sequence to search for</param>
		/// <param name="replace">Replacement sequence</param>
		public static IList<T> ReplaceAllSequences<T>(this List<T> rg, IList<T> sequence, IList<T> replace)
		{
			int i = 0;

			do {
				//i = rg.IndexOf(sequence[0], i);

				var b = rg.GetRange(i, sequence.Count).SequenceEqual(sequence);

				if (b) {
					rg.RemoveRange(i, sequence.Count);
					rg.InsertRange(i, replace);
					i += sequence.Count;
				}
				


			} while (!(++i >= rg.Count));

			return rg;
		}

		/*public static bool IndexOutOfBounds<T>(this IList<T> rg, int idx)
		{
			//idx < io.Length && idx >= 0
			//(idx < rg.Count && idx >= 0)
			//!(idx > rg.Count || idx < 0)

			return idx < rg.Count && idx >= 0;
		}*/

		public static IEnumerable<T> Difference<T>(this IEnumerable<T> a, IEnumerable<T> b)
		{
			return b.Where(c => !a.Contains(c));
		}


		/// <summary>
		/// Break a list of items into chunks of a specific size
		/// </summary>
		public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int size)
		{
			while (source.Any()) {
				yield return source.Take(size);
				source = source.Skip(size);
			}
		}

		public static void Replace<T>(this List<T> list, Predicate<T> oldItemSelector, T newItem)
		{
			//check for different situations here and throw exception
			//if list contains multiple items that match the predicate
			//or check for nullability of list and etc ...
			int oldItemIndex = list.FindIndex(oldItemSelector);
			list[oldItemIndex] = newItem;
		}

		#region Dictionary

		#region Serialize

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

		private const string DICT_DELIM = "=";

		#endregion

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

		public static bool TryCastDictionary(object obj, out Map buf)
		{
			bool condition = obj.GetType().GetInterface(nameof(IDictionary)) != null;

			if (!condition) {
				buf = null;
				return false;
			}

			var ex = ((IDictionary) obj).GetEnumerator();

			buf = new Map();

			while (ex.MoveNext()) {
				buf.Add(ex.Key, ex.Value);

			}

			return true;
		}

		#endregion
	}
}