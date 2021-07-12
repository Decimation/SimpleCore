using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using RestSharp;
using SimpleCore.Model;

#pragma warning disable 8600
#pragma warning disable 8604

// ReSharper disable CognitiveComplexity

// ReSharper disable InconsistentNaming
#pragma warning disable 8618

// ReSharper disable SwitchStatementHandlesSomeKnownEnumValuesWithDefault

// ReSharper disable UnusedMember.Global
#nullable enable

namespace SimpleCore.Net
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="WebUtilities"/>
	/// <seealso cref="Dns"/>
	/// <seealso cref="IPAddress"/>
	public static class Network
	{
		private const long TimeoutMS = 3000;

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
			return uri.Host + uri.PathAndQuery + uri.Fragment;
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

		public static IPGeolocation Identify(IPAddress ip) => Identify(ip.ToString());

		public static IPGeolocation Identify(string hostOrIP)
		{
			var rc = new RestClient("https://freegeoip.app/{format}/{host}");
			var r  = new RestRequest();
			r.AddUrlSegment("format", "json");
			r.AddUrlSegment("host", hostOrIP);
			var res = rc.Execute<IPGeolocation>(r);
			return res.Data;
		}

		public static IPAddress GetHostAddress(string hostOrIP) => Dns.GetHostAddresses(hostOrIP)[0];

		//public static IPAddress GetHostAddress(Uri u) => GetHostAddress(GetHostComponent(u));

		public static string GetAddress(string u)
		{
			string s = null;

			if (IPAddress.TryParse(u, out var ip)) {
				s = ip.ToString();
			}

			if (IsUri(u, out var ux)) {
				s = GetHostComponent(ux);
			}

			return GetHostAddress(s).ToString();
		}

		public static bool IsAlive(Uri u) => IsAlive(u, TimeoutMS);

		//public static bool IsAlive(string u) => IsAlive(u, Timeout);

		public static bool IsAlive(Uri u, long ms) => Ping(u, ms).Status == IPStatus.Success;


		public static bool IsAlive(string hostOrIP, long ms)
		{
			/*
			 * This approach is about .7 sec faster than using a web request
			 */


			PingReply r = Ping(hostOrIP, ms);

			return r.Status == IPStatus.Success;
		}

		public static PingReply Ping(Uri u, long ms = TimeoutMS) =>
			Ping(GetAddress(u.ToString()), ms);


		public static PingReply Ping(string hostOrIP, long ms = TimeoutMS)
		{
			var ping = new Ping();

			//var   t2 = p2.SendPingAsync(address, (int)TimeSpan.FromSeconds(3).TotalMilliseconds);
			//await t2;

			var task = ping.SendPingAsync(hostOrIP, (int) ms);
			task.Wait();

			PingReply r = task.Result;

			return r;
		}

		/*public static bool IsAlive2(Uri u) => IsAlive2(u, Timeout);

		public static bool IsAlive2(Uri u, TimeSpan span)
		{
			try {
				var request = (HttpWebRequest) WebRequest.Create(u);

				request.Timeout           = (int) span.TotalMilliseconds;
				request.AllowAutoRedirect = false; // find out if this site is up and don't follow a redirector
				request.Method            = "HEAD";

				using var response = (HttpWebResponse) request.GetResponse();
				return true;
			}
			catch (WebException) {
				return false;
			}
		}*/

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

							if (newUrl.Contains("://", StringComparison.Ordinal)) {
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

			if (res.StatusCode == HttpStatusCode.NotFound) {
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

	public sealed class IPGeolocation
	{
		public string IP { get; internal set; }

		public string CountryCode { get; internal set; }

		public string CountryName { get; internal set; }

		public string RegionCode { get; internal set; }

		public string RegionName { get; internal set; }

		public string City { get; internal set; }

		public string ZipCode { get; internal set; }

		public string TimeZone { get; internal set; }

		public double Latitude { get; internal set; }

		public double Longitude { get; internal set; }

		public int MetroCode { get; internal set; }

		public override string ToString()
		{
			return $"{nameof(IP)}: {IP}\n"                   +
			       $"{nameof(CountryCode)}: {CountryCode}\n" +
			       $"{nameof(CountryName)}: {CountryName}\n" +
			       $"{nameof(RegionCode)}: {RegionCode}\n"   +
			       $"{nameof(RegionName)}: {RegionName}\n"   +
			       $"{nameof(City)}: {City}\n"               +
			       $"{nameof(ZipCode)}: {ZipCode}\n"         +
			       $"{nameof(TimeZone)}: {TimeZone}\n"       +
			       $"{nameof(Latitude)}: {Latitude}\n"       +
			       $"{nameof(Longitude)}: {Longitude}\n"     +
			       $"{nameof(MetroCode)}: {MetroCode}";
		}
	}
}