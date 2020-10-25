using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using SimpleCore.CommandLine;
using SimpleCore.Win32;

namespace SimpleCore.Utilities
{
	/// <summary>
	/// Utilities for working with image files.
	/// </summary>
	public static class Images
	{
		public static (int Width, int Height) GetImageDimensions(string img)
		{
			var bmp = new Bitmap(img);

			return (bmp.Width, bmp.Height);
		}
	}
}
