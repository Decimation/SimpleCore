using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local


namespace SimpleCore.Model
{
	public abstract class Enumeration : IComparable
	{
		public string Name { get; private init; }

		public int Id { get; private init; }

		protected Enumeration(int id, string name)
		{
			Id   = id;
			Name = name;
		}

		

		public static int GetNextId<T>() where T : Enumeration
		{
			var t = GetAll<T>().Last();
			return t.Id + 1;
		}

		public override string ToString() => $"{Name} ({Id})";

		public static IEnumerable<T> GetAll<T>() where T : Enumeration
		{
			var fields = typeof(T).GetFields(BindingFlags.Public |
			                                 BindingFlags.Static |
			                                 BindingFlags.DeclaredOnly)
				.Where(r => r.FieldType == typeof(T));

			return fields.Select(f => f.GetValue(null)).Cast<T>();
		}

		public override bool Equals(object obj)
		{
			var otherValue = obj as Enumeration;

			if (otherValue == null)
				return false;

			var typeMatches  = GetType() == obj.GetType();
			var valueMatches = Id.Equals(otherValue.Id);

			return typeMatches && valueMatches;
		}

		protected bool Equals(Enumeration other)
		{
			return Name == other.Name && Id == other.Id;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name, Id);
		}

		public int CompareTo(object other) => Id.CompareTo(((Enumeration) other).Id);

		// Other utility methods ...
	}
}