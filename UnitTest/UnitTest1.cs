using System;
using System.Drawing;
using NUnit.Framework;
using SimpleCore.Net;
using SimpleCore.Utilities;

namespace UnitTest
{
	public class Tests
	{
		[SetUp]
		public void Setup() { }

		

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
	}
}