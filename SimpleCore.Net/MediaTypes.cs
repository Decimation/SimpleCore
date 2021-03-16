using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using RestSharp;
using SimpleCore.Utilities;

// ReSharper disable UnusedMember.Global
#nullable enable
namespace SimpleCore.Net
{
	public static class MediaTypes
	{
		/*
		 * type/subtype
		 * type/subtype;parameter=value
		 */

		//todo

		/// <summary>
		/// Identifies the MIME type of <paramref name="url"/>
		/// </summary>
		public static string? Identify(string url)
		{
			
			var req    = new RestRequest(url, Method.HEAD);
			
			var client = new RestClient();
			
			//client.FollowRedirects = true;

			var res = client.Execute(req);

			if (res.StatusCode == HttpStatusCode.NotFound) {
				return null;
			}

			return res.ContentType;
		}

		private const char   TYPE_DELIM  = '/';
		private const string PARAM_DELIM = ";";

		private const int TYPE_I    = 0;
		private const int SUBTYPE_I = 1;

		private static string[] GetMediaTypeComponents(string mediaType)
		{
			var rg = mediaType.Split(TYPE_DELIM);

			if (rg[SUBTYPE_I].Contains(PARAM_DELIM)) {

				rg[SUBTYPE_I] = rg[SUBTYPE_I].SubstringBefore(PARAM_DELIM);
			}

			return rg;
		}

		public static string GetTypeComponent(string mediaType) => GetMediaTypeComponents(mediaType)[TYPE_I];

		public static string GetSubTypeComponent(string mediaType) => GetMediaTypeComponents(mediaType)[SUBTYPE_I];


		/// <summary>
		/// Whether the MIME <paramref name="mediaType"/> is of type <paramref name="type"/>
		/// </summary>
		public static bool IsType(string mediaType, MimeType type)
		{
			return GetTypeComponent(mediaType) == Enum.GetName(type).ToLower();
		}


		public static bool IsDirect(string url, MimeType m)
		{
			var mediaType = Identify(url);

			if (mediaType == null) {
				return false;
			}

			var b = IsType(mediaType, m);

			return b;

		}
	}

	public enum MimeType
	{
		Image,
	}
}