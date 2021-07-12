using RestSharp;
using SimpleCore.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using SimpleCore.Model;

// ReSharper disable SwitchStatementHandlesSomeKnownEnumValuesWithDefault

// ReSharper disable UnusedMember.Global
#nullable enable

namespace SimpleCore.Net
{
	public static class Network
	{
		

		public static Uri GetHostUri(Uri u)
		{
			return new UriBuilder(u.Host).Uri;
		}

		public static string GetHostComponent(Uri u)
		{
			return u.GetComponents(UriComponents.NormalizedHost, UriFormat.Unescaped);
		}

		public static string StripScheme(Uri uri)
		{

			string uriWithoutScheme = uri.Host + uri.PathAndQuery + uri.Fragment;

			return uriWithoutScheme;

		}

		public static bool IsUri(string uriName, out Uri? uriResult)
		{
			bool result = Uri.TryCreate(uriName, UriKind.Absolute, out uriResult)
			              && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);


			// if (!result) {
			// 	var b = new UriBuilder(StripScheme(new Uri(uriName)))
			// 	{
			// 		Scheme = Uri.UriSchemeHttps, 
			// 		Port = -1
			// 	};
			// 	uriResult = b.Uri;
			//
			// 	//uriResult = new Uri(uriResult.ToString().Replace("file:", "http:"));
			// }


			return result;
		}

		public static bool IsUriAlive(Uri u) => IsUriAlive(u, TimeSpan.FromSeconds(3));

		public static bool IsUriAlive(Uri u, TimeSpan span)
		{
			try {
				var request  = (HttpWebRequest) WebRequest.Create(u);

				request.Timeout           = (int) span.TotalMilliseconds;
				request.AllowAutoRedirect = false; // find out if this site is up and don't follow a redirector
				request.Method            = "HEAD";

				var response = (HttpWebResponse) request.GetResponse();
				return true;
			}
			catch (WebException) {
				return false;
			}
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

							if (newUrl.Contains("://", System.StringComparison.Ordinal)) {
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

		public static IRestResponse GetResponse(string url)
		{
			var client  = new RestClient();
			var restReq = new RestRequest(url);
			var restRes = client.Execute(restReq);

			return restRes;
		}

		public static IRestResponse? GetQueryResponse(string s)
		{
			var req    = new RestRequest(s, Method.HEAD);
			var client = new RestClient();

			//client.FollowRedirects = true;

			var res = client.Execute(req);

			if (res.StatusCode == HttpStatusCode.NotFound)
			{
				return null;
			}

			return res;
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