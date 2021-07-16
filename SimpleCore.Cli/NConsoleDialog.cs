using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace SimpleCore.Cli
{
	public class NConsoleDialog
	{
		public IList<NConsoleOption> Options { get; init; }

		public bool SelectMultiple { get; init; }

		public string Header { get; set; }

		[CanBeNull]
		public string Status { get; set; }

		[CanBeNull]
		public string Description { get; set; }


		protected bool Equals(NConsoleDialog other)
		{
			return Equals(Options, other.Options)
			       && SelectMultiple == other.SelectMultiple
			       && Header         == other.Header
			       && Status         == other.Status
			       && Description    == other.Description;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((NConsoleDialog) obj);
		}

		public override int GetHashCode()
		{
			var h=new HashCode();
			
			var hx=Options.Select(o => (o.GetHashCode()));

			foreach (int i in hx) {
				h.Add(i);
			}
			h.Add(Status?.GetHashCode());
			h.Add(Description?.GetHashCode());
			h.Add(Header?.GetHashCode());
			return h.ToHashCode();
		}
	}
}