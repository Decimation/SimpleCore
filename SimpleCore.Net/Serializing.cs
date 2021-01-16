using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace SimpleCore.Net
{
	public static class Serializing
	{
		public static JsonValue TryGetKeyValue(this JsonValue value, string k)
		{
			return value.ContainsKey(k) ? value[k] : null;
		}
	}
}
