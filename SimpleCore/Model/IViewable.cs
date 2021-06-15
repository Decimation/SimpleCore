using System.Collections.Generic;
// ReSharper disable UnusedMember.Global

namespace SimpleCore.Model
{
	public interface IViewable
	{
		public Dictionary<string, object> View { get; }
	}
}