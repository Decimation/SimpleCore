using NUnit.Framework;
using SimpleCore.Net;

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


			Assert.True(Network.IsUri(jpg, out _));
			
		}
	}
}