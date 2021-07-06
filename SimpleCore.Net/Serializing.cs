using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

// ReSharper disable UnusedMember.Global

namespace SimpleCore.Net
{
	public static class Serializing
	{
		public static JsonValue TryGetKeyValue(this JsonValue value, string k)
		{
			return value.ContainsKey(k) ? value[k] : null;
		}

		public static string GetExclusiveText(this INode node)
		{
			return node.ChildNodes.OfType<IText>().Select(m => m.Text).FirstOrDefault();
		}

		public static IEnumerable<string> QuerySelectorAttributes(this IHtmlDocument document, string s, string a)
		{
			return document.QuerySelectorAll(s).Select(s => s.GetAttribute(a));

		}

		public static string TryGetAttribute(this INode n, string s)
		{
			return ((IHtmlElement) n).GetAttribute(s);
		}
	}
}