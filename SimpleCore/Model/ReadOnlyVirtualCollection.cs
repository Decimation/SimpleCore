using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCore.Model
{
	/// <inheritdoc />
	/// <summary>
	///     Represents a collection implemented by delegates.
	/// </summary>
	public class ReadOnlyVirtualCollection<T> : IReadOnlyList<T>
	{
		/// <summary>
		///     Retrieves an item with the name <paramref name="name" />
		/// </summary>
		public delegate T GetItem(string name);

		/// <summary>
		///     Retrieves the items as an array.
		/// </summary>
		public delegate T[] GetItems();

		private readonly GetItem m_fnGetItem;

		private readonly GetItems m_fnGetItems;

		public ReadOnlyVirtualCollection(GetItem fnGetItem, GetItems fnGetItems)
		{
			m_fnGetItem  = fnGetItem;
			m_fnGetItems = fnGetItems;
		}

		/// <summary>
		/// Gets an item with the name <paramref name="name"/>
		/// </summary>
		/// <param name="name">Name of the item</param>
		public T this[string name] => m_fnGetItem(name);

		/// <summary>
		/// Gets an item at index <paramref name="index"/>
		/// </summary>
		/// <param name="index">Index to retrieve the item</param>
		public T this[int index] => m_fnGetItems()[index];

		public IEnumerator<T> GetEnumerator()
		{
			return ((IEnumerable<T>) ToArray()).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public T[] ToArray() => m_fnGetItems();

		public int Count => ToArray().Length;
	}
}