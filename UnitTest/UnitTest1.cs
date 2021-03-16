using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using SimpleCore.Net;
using SimpleCore.Utilities;

namespace UnitTest
{
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
			
		}


		[Test]
		public void Test1()
		{
			const string jpg = "https://i.ytimg.com/vi/r45a-l9Gqdk/hqdefault.jpg";


			Assert.True(Network.IsUri(jpg, out var u));
			Assert.True(MediaTypes.IsType(MediaTypes.Identify(jpg), MimeType.Image));

			Assert.True(MediaTypes.IsDirect(jpg, MimeType.Image));
			Assert.True(MediaTypes.GetTypeComponent("image/jpg")              == "image");
			Assert.True(MediaTypes.GetSubTypeComponent("image/jpg")           == "jpg");
			Assert.True(MediaTypes.GetTypeComponent("text/html;charset=utf8") == "text");
		}

		[Test]
		public void Test2()
		{
			var rg      = new List<int>() {1, 2, 3, 4, 5, 6, 3, 4, 5};
			var search  = new List<int>() {3, 4, 5};
			var replace = new List<int>() {5, 4, 3};


			rg.ReplaceAllSequences(search, replace);

			//TestContext.WriteLine($"{rg.QuickJoin()}");

			var rgNew = new List<int>() {1, 2, 5, 4, 3, 6, 5, 4, 3};

			Assert.True(rg.SequenceEqual(rgNew));
			
			


			var rg2      = new[] {"a", "foo", "bar", "hi"};
			var search2  = new[] {"foo", "bar"};
			var replace2 = new[] {"goo"};


			rg2 = rg2.ReplaceAllSequences(search2, replace2);

			//TestContext.WriteLine($"{rg2.QuickJoin()}");
			var rg2New = new[] {"a", "goo", "hi"};
			Assert.True(rg2.SequenceEqual(rg2New));
		}
	}
}