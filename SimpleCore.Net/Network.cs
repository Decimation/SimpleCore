using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using RestSharp;

// ReSharper disable UnusedMember.Global
#nullable enable


#pragma warning disable HAA0601 // Value type to reference type conversion causing boxing allocation
#pragma warning disable HAA0602 // Delegate on struct instance caused a boxing allocation
#pragma warning disable HAA0603 // Delegate allocation from a method group
#pragma warning disable HAA0604 // Delegate allocation from a method group

#pragma warning disable HAA0501 // Explicit new array type allocation
#pragma warning disable HAA0502 // Explicit new reference type allocation
#pragma warning disable HAA0503 // Explicit new reference type allocation
#pragma warning disable HAA0504 // Implicit new array creation allocation
#pragma warning disable HAA0505 // Initializer reference type allocation
#pragma warning disable HAA0506 // Let clause induced allocation

#pragma warning disable HAA0301 // Closure Allocation Source
#pragma warning disable HAA0302 // Display class allocation to capture closure
#pragma warning disable HAA0303 // Lambda or anonymous method in a generic method allocates a delegate instance



namespace SimpleCore.Net
{
	public static class Network
	{
		public static void AssertResponse(IRestResponse response)
		{
			// todo

			if (!response.IsSuccessful) {
				var sb = new StringBuilder();
				sb.AppendFormat("Uri: {0}\n", response.ResponseUri);
				sb.AppendFormat("Code: {0}\n", response.StatusCode);

				Console.WriteLine("\n\n{0}", sb);
			}
		}


		/// <summary>
		/// Identifies the MIME type of <paramref name="url"/>
		/// </summary>
		public static string? IdentifyType(string url)
		{
			//var u =new Uri(url);

			var        req    = new RestRequest(url, Method.HEAD);
			RestClient client = new();

			var res = client.Execute(req);


			foreach (var h in res.Headers) {
				if (h.Name == "Content-Type") {
					var t = h.Value;

					return (string?) t;
				}
			}


			return null;
		}

		public static string DownloadUrl(string url)
		{
			string          fileName = Path.GetFileName(url);
			using WebClient client   = new();
			client.Headers.Add("User-Agent: Other");

			var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
				"Desktop", fileName);

			client.DownloadFile(url, dir);

			return dir;
		}

		public static void OpenUrl(string url)
		{
			// https://stackoverflow.com/questions/4580263/how-to-open-in-default-browser-in-c-sharp

			try {
				Process.Start(url);
			}
			catch {
				// hack because of this: https://github.com/dotnet/corefx/issues/10361
				if (OperatingSystem.IsWindows()) {
					url = url.Replace("&", "^&");
					Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") {CreateNoWindow = true});
				}
				else {
					throw;
				}
			}
		}

		private static readonly RestClient Client = new();

		public static IRestResponse GetSimpleResponse(string link)
		{
			var restReq = new RestRequest(link);
			var restRes = Client.Execute(restReq);

			return restRes;
		}

		public static string GetString(string url)
		{
			using var wc = new WebClient();
			return wc.DownloadString(url);
		}

		/// <summary>
		/// Whether the MIME type <paramref name="type"/> is an image type.
		/// </summary>
		public static bool IsImage(string? type)
		{
			var notImage = type == null || type.Split("/")[0] != "image";

			return !notImage;
		}
	}
}