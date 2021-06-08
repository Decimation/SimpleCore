using RestSharp;
using SimpleCore.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

// ReSharper disable SwitchStatementHandlesSomeKnownEnumValuesWithDefault

// ReSharper disable UnusedMember.Global
#nullable enable

namespace SimpleCore.Net
{
	public static class Network
	{
		public static bool IsUri(string uriName, out Uri? uriResult)
		{
			bool result = Uri.TryCreate(uriName, UriKind.Absolute, out uriResult)
			              && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

			return result;
		}


		public static bool IsUriAlive(Uri u)
		{
			/*var request = (HttpWebRequest)WebRequest.Create(u);

			var response = (HttpWebResponse)request.GetResponse();

			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return false;
			}

			return true;*/

			/*var rc  = new RestClient();
			var res = rc.Execute(new RestRequest(u));
			return res.IsSuccessful;*/

			try {
				var request  = (HttpWebRequest) WebRequest.Create(u);
				var response = (HttpWebResponse) request.GetResponse();

				return true;
			}
			catch (WebException e) {
				return false;
			}


			//var req = (HttpWebRequest) WebRequest.Create(u);
			//req.Method            = "HEAD";
			//req.AllowAutoRedirect = false;
			//var resp = (HttpWebResponse) req.GetResponse();

			//return resp.StatusCode == HttpStatusCode.OK;
		}

		public static string? GetFinalRedirect(string url)
		{
			// https://stackoverflow.com/questions/704956/getting-the-redirected-url-from-the-original-url

			if (String.IsNullOrWhiteSpace(url))
				return url;

			const int MAX_REDIR = 8;

			int maxRedirCount = MAX_REDIR; // prevent infinite loops

			string? newUrl = url;

			do {
				HttpWebResponse? resp = null;

				try {
					var req = (HttpWebRequest) WebRequest.Create(url);
					req.Method            = "HEAD";
					req.AllowAutoRedirect = false;
					resp                  = (HttpWebResponse) req.GetResponse();

					switch (resp.StatusCode) {
						case HttpStatusCode.OK:
							return newUrl;

						case HttpStatusCode.Redirect:
						case HttpStatusCode.MovedPermanently:
						case HttpStatusCode.RedirectKeepVerb:
						case HttpStatusCode.RedirectMethod:
							newUrl = resp.Headers["Location"];

							if (newUrl == null)
								return url;

							if (newUrl.IndexOf("://", System.StringComparison.Ordinal) == -1) {
								// Doesn't have a URL Schema, meaning it's a relative or absolute URL
								Uri u = new(new Uri(url), newUrl);
								newUrl = u.ToString();
							}

							break;

						default:
							return newUrl;
					}

					url = newUrl;
				}
				catch (WebException) {
					// Return the last known good URL
					return newUrl;
				}
				catch (Exception) {
					return null;
				}
				finally {
					resp?.Close();
				}
			} while (maxRedirCount-- > 0);

			return newUrl;
		}

		public static string Download(string url, string folder)
		{
			string fileName = Path.GetFileName(url);

			using WebClient client = new();
			client.Headers.Add("User-Agent: Other");

			string? dir = Path.Combine(folder, fileName);

			client.DownloadFile(url, dir);

			return dir;
		}

		public static string? GetExclusiveText(this INode node)
		{
			return node.ChildNodes.OfType<IText>().Select(m => m.Text).FirstOrDefault();
		}

		public static string TryGetAttribute(this INode n, string s)
		{
			return ((IHtmlElement) n).GetAttribute(s);
		}

		public static string Download(string url)
		{
			string? folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

			return Download(url, folder);
		}

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
					Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") {CreateNoWindow = true});
				}
				else {
					throw;
				}
			}
		}

		public static Stream GetStream(string url)
		{
			using var wc = new WebClient();

			byte[]? imageData = wc.DownloadData(url);

			return new MemoryStream(imageData);
		}

		public static IRestResponse GetSimpleResponse(string url)
		{
			var restReq = new RestRequest(url);
			var client  = new RestClient();
			var restRes = client.Execute(restReq);

			return restRes;
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

		public static void DumpResponse(IRestResponse response)
		{
			var ct = new ConsoleTable("-", "Value");

			ct.AddRow("Uri", response.ResponseUri);
			ct.AddRow("Successful", response.IsSuccessful);
			ct.AddRow("Status code", response.StatusCode);
			ct.AddRow("Error message", response.ErrorMessage);

			var str = ct.ToString();

			Trace.WriteLine(str);
		}
	}
}