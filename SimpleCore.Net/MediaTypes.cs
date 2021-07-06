using Newtonsoft.Json;
using RestSharp;
using SimpleCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

// ReSharper disable IdentifierTypo

#pragma warning disable 8602
#pragma warning disable 8604
#pragma warning disable 8625
#pragma warning disable 8618
#pragma warning disable IDE0059

// ReSharper disable UnusedMember.Global
#nullable enable

namespace SimpleCore.Net
{
	/// <summary>
	/// Media types, MIME types, etc.
	/// </summary>
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
			var res = Network.GetQueryResponse(url);

			return res?.ContentType;
		}

		/// <summary>
		/// Whether the MIME <paramref name="mime"/> is of type <paramref name="type"/>
		/// </summary>
		public static bool IsType(string mime, MimeType type) =>
			GetTypeComponent(mime) == Enum.GetName(type)!.ToLower();

		public static bool IsDirect(string url, MimeType m)
		{
			//var isUri = Network.IsUri(value, out _);

			string? mediaType = Identify(url);

			if (mediaType == null) {
				return false;
			}

			bool b = IsType(mediaType, m);

			return b;
		}

		/*
		 * https://github.com/khellang/MimeTypes/blob/master/src/MimeTypes/MimeTypeFunctions.ttinclude
		 */

		[DllImport("urlmon.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false)]
		private static extern int FindMimeFromData(IntPtr pBC,
		                                           [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl,
		                                           [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1,
		                                                      SizeParamIndex                      = 3)]
		                                           byte[] pBuffer,
		                                           int cbSize,
		                                           [MarshalAs(UnmanagedType.LPWStr)] string pwzMimeProposed,
		                                           int dwMimeFlags,
		                                           out IntPtr ppwzMimeOut,
		                                           int dwReserved);

		public static string ResolveFromData(string url) => ResolveFromData(WebUtilities.GetStream(url));

		public static string ResolveFromData(Stream s)
		{
			var ms = s as MemoryStream;

			const int BLOCK_SIZE = 256;

			byte[] rg = new byte[BLOCK_SIZE];

			int c = ms.Read(rg, 0, BLOCK_SIZE);

			string m = ResolveFromData(rg);

			return m;
		}

		public static string ResolveFromData(byte[] dataBytes, string? mimeProposed = null)
		{
			//https://stackoverflow.com/questions/2826808/how-to-identify-the-extension-type-of-the-file-using-c/2826884#2826884
			//https://stackoverflow.com/questions/18358548/urlmon-dll-findmimefromdata-works-perfectly-on-64bit-desktop-console-but-gener
			//https://stackoverflow.com/questions/11547654/determine-the-file-type-using-c-sharp
			//https://github.com/GetoXs/MimeDetect/blob/master/src/Winista.MimeDetect/URLMONMimeDetect/urlmonMimeDetect.cs

			Guard.AssertArgumentNotNull(dataBytes, nameof(dataBytes));

			string mimeRet = String.Empty;

			if (!String.IsNullOrEmpty(mimeProposed)) {
				//suggestPtr = Marshal.StringToCoTaskMemUni(mimeProposed); // for your experiments ;-)
				mimeRet = mimeProposed;
			}

			int ret = FindMimeFromData(IntPtr.Zero, null, dataBytes, dataBytes.Length,
			                           mimeProposed, 0, out var outPtr, 0);

			if (ret == 0 && outPtr != IntPtr.Zero) {
				string str = Marshal.PtrToStringUni(outPtr)!;

				Marshal.FreeHGlobal(outPtr);

				return str;
			}

			return mimeRet;
		}

		/*private static IEnumerable<(string Extension, string Type)> GetMediaTypes(
			IEnumerable<KeyValuePair<string, MimeType>> mimeTypes)
			=> mimeTypes.Where(x => x.Value.Extensions.Any())
				.SelectMany(x => x.Value.Extensions.Select(e => (e, x.Key)))
				.Where(x => x.Item1.Length <= 8 && x.Item1.All(char.IsLetterOrDigit))
				.GroupBy(x => x.Item1)
				.Select(x => x.First())
				.OrderBy(x => x.Item1, StringComparer.InvariantCulture);

		public static IList<(string Extension, string Type)> GetMediaTypeList()
		{
			return GetMediaTypes(GetMediaTypes()).ToList();
		}*/

		private const char DELIM = '/';

		private const int TYPE_I = 0;

		private const int SUBTYPE_I = 1;

		public static string GetTypeComponent(string mime) => mime.Split(DELIM)[TYPE_I];

		public static string GetSubTypeComponent(string mime)
		{
			// NOTE: doesn't handle parameters
			return mime.Split(DELIM)[SUBTYPE_I];
		}

		private const string DB_JSON_URL = "https://cdn.jsdelivr.net/gh/jshttp/mime-db@master/db.json";

		private static Dictionary<string, MimeTypeInfo> GetDatabase()
		{
			using var client = new WebClient();

			string json = client.DownloadString(new Uri(DB_JSON_URL));

			var mimeTypes = JsonConvert.DeserializeObject<Dictionary<string, MimeTypeInfo>>(json)!;

			return mimeTypes;
		}

		public static IEnumerable<string> GetExtensions(string mime)
		{
			mime = mime.ToLower();

			return Database.Where(kp => kp.Key == mime).SelectMany(kp => kp.Value.Extensions);
		}

		private static readonly Dictionary<string, MimeTypeInfo> Database;

		static MediaTypes()
		{
			Database = GetDatabase();
		}
	}

	public enum MimeType
	{
		Image,
		Video,
		Audio
	}

	public class MimeTypeInfo
	{
		public MimeTypeInfo()
		{
			Extensions = new List<string>();
		}

		public string Source { get; set; }

		public List<string> Extensions { get; }

		public bool Compressible { get; set; }

		public string Charset { get; set; }
	}
}