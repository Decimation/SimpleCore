using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

// ReSharper disable UnusedMember.Global

namespace SimpleCore.Utilities
{
	public static class EnumExtensions
	{
		public static bool HasFlagFast(this HexOptions value, HexOptions flag) => (value & flag) != 0;
	}

	/// <summary>
	/// Utilities for enums (<see cref="Enum"/>).
	/// </summary>
	public static class Enums
	{
		public static TEnum SafeParse<TEnum>(string s) where TEnum : Enum
		{
			if (String.IsNullOrWhiteSpace(s)) {
				return default;
			}

			Enum.TryParse(typeof(TEnum), s, out var e);
			return (TEnum) e;
		}

		public static TEnum ReadEnumFromSet<TEnum>(ISet<object> set) where TEnum : Enum
		{
			var t = typeof(TEnum);

			if (t.GetCustomAttribute<FlagsAttribute>() != null) {
				var sz = Formatting.QuickJoin(set);
				Enum.TryParse(typeof(TEnum), (string) sz, out var e);
				return (TEnum) e;
			}

			return default;
		}
	}
}