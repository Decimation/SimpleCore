using System;

namespace SimpleCore.Utilities
{
	public static class Enums
	{
		public static TEnum SafeParse<TEnum>(string s) where TEnum:Enum
		{
			if (String.IsNullOrWhiteSpace(s)) {
				return default;
			}

			Enum.TryParse(typeof(TEnum), s, out var e);
			return (TEnum) e;
		}
	}
}