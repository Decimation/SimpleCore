using System;
using System.Collections.Generic;
using System.Drawing;

// ReSharper disable UnusedMember.Global

namespace SimpleCore.Utilities
{
	/// <summary>
	/// Color utilities.
	/// </summary>
	/// <seealso cref="Color"/>
	/// <seealso cref="KnownColor"/>
	/// <seealso cref="ConsoleColor"/>
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

		public static Color FromConsoleColor(ConsoleColor c)
		{
			int cInt = (int)c;

			int brightnessCoefficient = ((cInt & 8) > 0) ? 2 : 1;
			int r                     = ((cInt & 4) > 0) ? 64 * brightnessCoefficient : 0;
			int g                     = ((cInt & 2) > 0) ? 64 * brightnessCoefficient : 0;
			int b                     = ((cInt & 1) > 0) ? 64 * brightnessCoefficient : 0;

			return Color.FromArgb(r, g, b);
		}

		public static IEnumerable<Color> GetGradients(Color start, Color end, int steps)
		{
			int stepA = ((end.A - start.A) / (steps - 1));
			int stepR = ((end.R - start.R) / (steps - 1));
			int stepG = ((end.G - start.G) / (steps - 1));
			int stepB = ((end.B - start.B) / (steps - 1));

			for (int i = 0; i < steps; i++)
			{
				yield return Color.FromArgb(start.A + (stepA * i),
					start.R                         + (stepR * i),
					start.G                         + (stepG * i),
					start.B                         + (stepB * i));
			}
		}
	}
}