using System;
using System.Drawing;

namespace SimpleCore.Utilities
{
	public static class ColorUtilities
	{
		/// <summary>
		/// Creates color with corrected brightness.
		/// </summary>
		/// <param name="color">Color to correct.</param>
		/// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1. 
		/// Negative values produce darker colors.</param>
		/// <returns>
		/// Corrected <see cref="Color"/> structure.
		/// </returns>
		public static Color ChangeColorBrightness(Color color, float correctionFactor)
		{
			// Adapted from https://gist.github.com/zihotki/09fc41d52981fb6f93a81ebf20b35cd5

			float red   = color.R;
			float green = color.G;
			float blue  = color.B;

			if (correctionFactor < 0) {
				correctionFactor =  1 + correctionFactor;
				red              *= correctionFactor;
				green            *= correctionFactor;
				blue             *= correctionFactor;
			}
			else {
				red   = (Byte.MaxValue - red)   * correctionFactor + red;
				green = (Byte.MaxValue - green) * correctionFactor + green;
				blue  = (Byte.MaxValue - blue)  * correctionFactor + blue;
			}

			return Color.FromArgb(color.A, (int) red, (int) green, (int) blue);
		}
	}
}