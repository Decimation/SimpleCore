using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using RestSharp;
// ReSharper disable UnusedMember.Global

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
		[CanBeNull]
		public static string IdentifyType(string url)
		{
			var req    = new RestRequest(url, Method.HEAD);
			var client = new RestClient();

			var res = client.Execute(req);

			if (res.StatusCode == HttpStatusCode.NotFound) {
				return null;
			}
			
			return res.ContentType;
		}

		public static string GetTypeComponent(string mediaType) => mediaType.Split('/')[0];

		private static readonly string[] ImageMimeTypes =
			{"image", "bmp", "gif", "jpeg", "png", "svg+xml", "tiff", "webp"};

		/// <summary>
		/// Whether the MIME type <paramref name="mediaType"/> is an image type.
		/// </summary>
		public static bool IsImage(string mediaType)
		{
			return ImageMimeTypes.Any(i => i == GetTypeComponent(mediaType));
		}
	}
}
