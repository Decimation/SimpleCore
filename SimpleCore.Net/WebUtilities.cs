#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable UnusedMember.Global

namespace SimpleCore.Net
{
	public static class WebUtilities
	{
		public static string Download(string url, string folder)
		{
			string fileName = Path.GetFileName(url);

			using WebClient client = new();
			client.Headers.Add("User-Agent: Other");

			string dir = Path.Combine(folder, fileName);

			client.DownloadFile(url, dir);

			return dir;
		}

		public static string Download(string url) =>
			Download(url, Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

		public static void OpenUrl(string url)
		{
			// https://stackoverflow.com/questions/4580263/how-to-open-in-default-browser-in-c-sharp
			// url must start with a protocol i.e. http://

			try {
				Process.Start(url);
			}
			catch {
				// hack because of this: https://github.com/dotnet/corefx/issues/10361
				if (OperatingSystem.IsWindows()) {
					url = url.Replace("&", "^&");

					Process.Start(new ProcessStartInfo("cmd", $"/c start {url}")
					{
						CreateNoWindow = true
					});
				}
				else {
					throw;
				}
			}
		}

		public static Stream GetStream(string url)
		{
			using var wc     = new WebClient();
			byte[]    buffer = wc.DownloadData(url);

			return new MemoryStream(buffer);
		}

		public static string GetString(string url)
		{
			using var wc = new WebClient();
			return wc.DownloadString(url);
		}

		public static bool TryGetString(string url, out string? e)
		{
			try {
				e = GetString(url);
				return true;
			}
			catch (Exception) {
				e = null;
				return false;
			}
		}
	}
}