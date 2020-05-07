using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SimpleCore.Utilities
{
	// nuget pack -Prop Configuration=Release

	// C:\Library\Nuget
	// dotnet pack -c Release -o %cd%
	// dotnet nuget push "*.nupkg"
	// del *.nupkg & dotnet pack -c Release -o %cd% & dotnet nuget push "*.nupkg"

	public static class Cli
	{
		public static void CreateRunBatchFile(string name, string[] code)
		{
			var file = CreateBatchFile(name, code);
			RunBatchFile(file);
		}
		
		public static string CreateBatchFile(string name, string[] code)
		{
			var file = Path.Combine(Directory.GetCurrentDirectory(), name);

			File.WriteAllLines(file, code);


			return file;
		}

		/// <summary>
		/// Creates a <see cref="Process"/> to execute <paramref name="cmd"/>
		/// </summary>
		/// <param name="cmd">Command to run</param>
		/// <param name="autoStart">Whether to automatically start the <c>cmd.exe</c> process</param>
		/// <returns><c>cmd.exe</c> process</returns>
		public static Process Shell(string cmd, bool autoStart = false)
		{
			var startInfo = new ProcessStartInfo
			{
				FileName               = "cmd.exe",
				Arguments              = String.Format("/C {0}", cmd),
				RedirectStandardOutput = true,
				RedirectStandardError  = true,
				UseShellExecute        = false,
				CreateNoWindow         = true
			};

			var process = new Process
			{
				StartInfo           = startInfo,
				EnableRaisingEvents = true
			};

			if (autoStart)
				process.Start();


			return process;
		}


		public static string[] ShellOutput(string         cmd,
		                                   StandardStream ss = StandardStream.StdOut, bool waitForExit = true)
		{
			var proc = Shell(cmd, true);

			var stream = ss switch
			{
				StandardStream.StdOut => proc.StandardOutput,
				StandardStream.StdError => proc.StandardError,
				_ => throw new ArgumentOutOfRangeException(nameof(ss), ss, null)
			};

			var list = ReadAllLines(stream);

			if (waitForExit) {
				ExplorerSystem.KillProc(proc);
			}


			return list;
		}

		public static string[] ReadAllLines(StreamReader stream)
		{
			var list = new List<string>();

			while (!stream.EndOfStream) {
				string? line = stream.ReadLine();

				if (line != null) {
					list.Add(line);
				}
			}

			return list.ToArray();
		}

		public static void RunBatchFile(string file)
		{
			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					WindowStyle     = ProcessWindowStyle.Hidden,
					FileName        = "cmd.exe",
					Arguments       = "/C \"" + file + "\"",
					Verb            = "runas",
					UseShellExecute = true
				}
			};


			process.Start();


			ExplorerSystem.KillProc(process);

			File.Delete(file);
		}
	}
}