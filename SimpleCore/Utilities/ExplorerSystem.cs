using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SimpleCore.Utilities
{
	public static class ExplorerSystem
	{
		private const string PATH_ENV = "PATH";

		public const char PATH_DELIM = ';';

		public static string[] PathDirectories {
			get { return ExplorerSystem.EnvironmentPath?.Split(PATH_DELIM); }
		}

		public static void RemoveFromPath(string f)
		{
			var oldValue = ExplorerSystem.EnvironmentPath;


			var newValue = oldValue.Replace(PATH_DELIM + f, String.Empty);


			ExplorerSystem.EnvironmentPath = newValue;
		}

		public static bool IsFolderInPath(string f)
		{
			string dir = PathDirectories.FirstOrDefault(s => s == f);

			return !String.IsNullOrWhiteSpace(dir);
		}

		public static string EnvironmentPath {
			get {
				var env = Environment.GetEnvironmentVariable(PATH_ENV, EnvironmentVariableTarget.User);
			
				if (env == null) {
					throw new NullReferenceException();
				}

				return env;
			}
			set {
				Environment.SetEnvironmentVariable(PATH_ENV, value, EnvironmentVariableTarget.User);
			}
		}

		public static void KillProc(Process p)
		{
			p.WaitForExit();
			p.Dispose();

			try {
				if (!p.HasExited) {
					p.Kill();
				}
			}
			catch (InvalidOperationException e) {
				// todo
			}
		}
		
		public static void WriteMap(IDictionary<string, string> d, string filename)
		{
			string[] lines = d.Select(kvp => kvp.Key + "=" + kvp.Value).ToArray();
			File.WriteAllLines(filename, lines);
		}

		public static IDictionary<string, string> ReadMap(string filename)
		{
			string[] lines = File.ReadAllLines(filename);
			var      dict  = lines.Select(l => l.Split('=')).ToDictionary(a => a[0], a => a[1]);

			return dict;
		}


		public static string FindExectableInPath(string exe)
		{
			string dir = ExplorerSystem.PathDirectories.FirstOrDefault(s => File.Exists(Path.Combine(s, exe)));

			if (!String.IsNullOrWhiteSpace(dir)) {
				return Path.Combine(dir, exe);
			}

			return null;
		}
	}
}