using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable UnusedMember.Global

namespace SimpleCore.Utilities
{
	/// <summary>
	/// Contains information about the currently executing program / calling assembly
	/// </summary>
	public static class AppInfo
	{
		public static AssemblyName[] GetDependencies()
		{
			return Assembly.GetReferencedAssemblies()
			               .Where(x => !x.Name.StartsWith("System"))
			               .ToArray();
		}

		public static Assembly Assembly => Assembly.GetCallingAssembly();

		//public static Version Version => Assembly.GetName().Version;

		public static string ExeFolder => Path.GetDirectoryName(ExeLocation);

		public static string ExeLocation => Process.GetCurrentProcess().MainModule.FileName;
	}
}