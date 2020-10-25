#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

// ReSharper disable UnusedMember.Global

#pragma warning disable HAA0502, HAA0301, HAA0302

namespace SimpleCore.Win32
{
	/// <summary>
	///     Utilities for working with Windows, native interop, file system, and other related things.
	/// </summary>
	public static class Native
	{
		/// <summary>
		///     Environment variable PATH
		/// </summary>
		public const string PATH_ENV = "PATH";

		/// <summary>
		///     Delimiter of environment variable <see cref="PATH_ENV" />
		/// </summary>
		public const char PATH_DELIM = ';';

		public const string KERNEL32_DLL = "kernel32.dll";

		/// <summary>
		///     Environment variable target
		/// </summary>
		public static EnvironmentVariableTarget Target { get; set; } = EnvironmentVariableTarget.User;

		/// <summary>
		///     Directories of <see cref="EnvironmentPath" /> with environment variable target <see cref="Target" />
		/// </summary>
		public static string[] PathDirectories => EnvironmentPath.Split(PATH_DELIM);

		/// <summary>
		///     Environment variable <see cref="PATH_ENV" /> with target <see cref="Target" />
		/// </summary>
		public static string EnvironmentPath
		{
			get
			{
				string? env = Environment.GetEnvironmentVariable(PATH_ENV, Target);

				if (env == null) {
					throw new NullReferenceException();
				}

				return env;
			}
			set => Environment.SetEnvironmentVariable(PATH_ENV, value, Target);
		}

		/// <summary>
		///     Removes <paramref name="location" /> from <see cref="PathDirectories" />
		/// </summary>
		public static void RemoveFromPath(string location)
		{
			string oldValue = EnvironmentPath;

			EnvironmentPath = oldValue.Replace(PATH_DELIM + location, String.Empty);
		}

		/// <summary>
		///     Determines whether <paramref name="location" /> is in <see cref="PathDirectories" />
		/// </summary>
		public static bool IsFolderInPath(string location)
		{
			string? dir = Array.Find(PathDirectories, s => s == location);

			return !String.IsNullOrWhiteSpace(dir);
		}

		/// <summary>
		/// Reads a <see cref="byte"/> array as a <see cref="string"/> delimited by spaces in
		/// hex number format
		/// </summary>
		public static byte[] ReadBinaryString(string s)
		{
			var rg = new List<byte>();

			var bytes = s.Split(' ');

			foreach (string b in bytes) {
				var n = byte.Parse(b, NumberStyles.HexNumber);

				rg.Add(n);
			}

			return rg.ToArray();
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