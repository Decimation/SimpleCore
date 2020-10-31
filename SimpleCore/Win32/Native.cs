
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using SimpleCore.Utilities;

// ReSharper disable UnusedMember.Global

#nullable enable
#pragma warning disable HAA0501 //
#pragma warning disable HAA0502 //
#pragma warning disable HAA0301 //
#pragma warning disable HAA0302 //

namespace SimpleCore.Win32
{
	/// <summary>
	///     Utilities for working with Windows, native interop, file system, and other related things.
	/// </summary>
	public static partial class Native
	{
		/// <summary>
		/// Reads a <see cref="byte"/> array as a <see cref="string"/> delimited by spaces in
		/// hex number format
		/// </summary>
		public static byte[] ReadBinaryString(string s)
		{
			var rg = new List<byte>();

			var bytes = s.Split(Formatting.SPACE);

			foreach (string b in bytes) {
				byte n = Byte.Parse(b, NumberStyles.HexNumber);

				rg.Add(n);
			}

			return rg.ToArray();
		}

		/// <summary>
		/// SI
		/// </summary>
		public const double MAGNITUDE = 1000D;

		/// <summary>
		/// ISO/IEC 80000
		/// </summary>
		public const double MAGNITUDE2 = 1024D;

		/// <summary>
		/// Function to convert the given bytes to <see cref="BinaryUnit"/>
		/// </summary>
		/// <param name="bytes">Value in bytes to be converted</param>
		/// <param name="type">Unit to convert to</param>
		/// <returns>Converted bytes</returns>
		public static double ConvertToUnit(double bytes, BinaryUnit type)
		{
			// var rg  = new[] { "k","M","G","T","P","E","Z","Y"};
			// var pow = rg.ToList().IndexOf(type) +1;

			int pow = (int) type;
			return bytes / Math.Pow(MAGNITUDE, pow);
		}


		public static float Distance(byte[] first, byte[] second)
		{
			int sum = 0;

			// We'll use which ever array is shorter.
			int length = first.Length > second.Length ? second.Length : first.Length;

			for (int x = 0; x < length; x++) {
				sum += (int) Math.Pow((first[x] - second[x]), 2);
			}

			return sum / (float) length;
		}

		/// <summary>
		///     Forcefully kills a <see cref="Process" /> and ensures the process has exited.
		/// </summary>
		/// <param name="p"><see cref="Process" /> to forcefully kill.</param>
		/// <returns><c>true</c> if <paramref name="p" /> was killed; <c>false</c> otherwise</returns>
		public static bool ForceKill(this Process p)
		{
			p.WaitForExit();
			p.Dispose();

			try {
				if (!p.HasExited) {
					p.Kill();
				}

				return true;
			}
			catch (Exception) {

				return false;
			}
		}
	}
}