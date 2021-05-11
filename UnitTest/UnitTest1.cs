using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using SimpleCore.Model;
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

			var i = MediaTypes.Identify(jpg);
			Assert.True(Network.IsUri(jpg, out var u));
			Assert.True(MediaTypes.GetExtensions(i).Contains("jpe"));
			Assert.True(MediaTypes.GetTypeComponent(i) =="image");
			Assert.True(MediaTypes.GetSubTypeComponent(i) == "jpeg");


		}

		[Test]
		public void enumtest()
		{
			var name1 = "combo1";
			var p     = Enum.Parse<MyEnum>(name1);
			var flags = Enums.GetSetFlags(p);
			var str   = flags.QuickJoin();

			Assert.AreEqual(str,"a, b, c");
		}

		[Flags]
		enum MyEnum
		{
			a = 0,
			b = 1 << 0,
			c = 1 << 1,

			combo1 = b | c,
		}
		private class a : Enumeration
		{
			public static          string str;
			public                 string str2;
			public static readonly a      a1 = new a(1, "g");
			public a(int id, string name) : base(id, name) { }
		}

		[Test]
		public void test3()
		{
			var rg = Enumeration.GetAll<a>().ToArray();

			TestContext.WriteLine(rg.Length);

			foreach (var a in rg) {
				TestContext.WriteLine(a.ToString());
			}

			TestContext.WriteLine(a.GetNextId<a>());
		}

		[Test]
		public void Test2()
		{
			var rg      = new List<int>() {1, 2, 3, 4, 5, 6, 3, 4, 5};
			var search  = new List<int>() {3, 4, 5};
			var replace = new List<int>() {5, 4, 3};


			rg.ReplaceAllSequences(search, replace);

			TestContext.WriteLine($"{rg.QuickJoin()}");

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