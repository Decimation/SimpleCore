using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable RedundantAnonymousTypePropertyName
#pragma warning disable IDE0037

namespace SimpleCore.Net
{
	public class SimpleGraphQLClient
	{
		/*
		 * Adapted from https://github.com/latheesan-k/simple-graphql-client
		 */


		private readonly RestClient _client;

		public SimpleGraphQLClient(string GraphQLApiUrl)
		{
			_client = new RestClient(GraphQLApiUrl);

			ServicePointManager.SecurityProtocol =
				SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
		}

		public dynamic Execute(string query, object variables = null,
		                       Dictionary<string, string> additionalHeaders = null, int timeout = 0)
		{
			var request = new RestRequest("/", Method.POST)
			{
				Timeout = timeout
			};

			if (additionalHeaders is {Count: > 0})
			{
				foreach ((string key, string value) in additionalHeaders)
				{
					request.AddHeader(key, value);
				}
			}

			request.AddJsonBody(new
			{
				query     = query,
				variables = variables
			});

			return JObject.Parse(_client.Execute(request).Content);
		}
	}
}