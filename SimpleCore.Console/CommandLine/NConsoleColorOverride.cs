using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCore.Console.CommandLine
{
	public sealed class NConsoleColorOverride : IDisposable
	{
		public Color? Foreground { get; }
		
		public Color? Background { get; }

		public NConsoleColorOverride(Color? foreground, Color? background)
		{
			if (!foreground.HasValue && !background.HasValue) {
				throw new Exception();
			}
			
			Foreground = foreground;
			Background = background;
		}

		public NConsoleColorOverride(Color? foreground) : this(foreground, null)
		{
			
		}

		private void SetColors()
		{
			NConsole.OverrideForegroundColor = Foreground;
			NConsole.OverrideBackgroundColor = Background;
		}

		public void Dispose()
		{
			
		}
	}
}
