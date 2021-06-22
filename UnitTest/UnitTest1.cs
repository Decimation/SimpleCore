using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SimpleCore.Diagnostics;
using SimpleCore.Model;
using SimpleCore.Net;
using SimpleCore.Numeric;
using SimpleCore.Utilities;
// ReSharper disable UnusedMember.Local

// ReSharper disable InconsistentNaming
#pragma warning disable 649, IDE0059

namespace UnitTest
{
	public class Tests
	{
		[SetUp]
		public void Setup() { }

		[Test]
		public void MathTest()
		{
			Assert.AreEqual(MathHelper.Add(1,1), 2);
			Assert.AreEqual(MathHelper.Subtract(1,1), 0);

			Assert.AreEqual(MathHelper.Multiply(2,2), 4);
			Assert.AreEqual(MathHelper.Divide(10,5), 2);
		}

		[Test]
		[TestCase(@"https://i.imgur.com/QtCausw.png")]
		public void UriAliveTest(string s)
		{
			Assert.True(Network.IsUriAlive(new Uri(s)));
		}

		[Test]
		public void MediaTypesTest()
		{
			const string jpg = "https://i.ytimg.com/vi/r45a-l9Gqdk/hqdefault.jpg";

			var i = MediaTypes.Identify(jpg);
			Assert.True(Network.IsUri(jpg, out var u));
			Assert.True(MediaTypes.GetExtensions(i).Contains("jpe"));
			Assert.True(MediaTypes.GetTypeComponent(i)    == "image");
			Assert.True(MediaTypes.GetSubTypeComponent(i) == "jpeg");


		}

		[Test]
		public void EnumTest()
		{
			var name1 = "combo1";
			var p     = Enum.Parse<MyEnum>(name1);
			var flags = Enums.GetSetFlags(p, false);
			var str   = flags.QuickJoin();

			Assert.AreEqual(str, "a, b, c, combo1");
		}

		[Flags]
		private enum MyEnum
		{
			a = 0,
			b = 1 << 0,
			c = 1 << 1,

			combo1 = b | c,
		}

		private class EnumerationTestType : Enumeration
		{
			public static string str;
			public        string str2;

			public static readonly EnumerationTestType a1 = new(1, "g");

			public EnumerationTestType(int id, string name) : base(id, name) { }
		}

		[Test]
		public void EnumerationTest()
		{
			var rg = Enumeration.GetAll<EnumerationTestType>().ToArray();

			TestContext.WriteLine(rg.Length);

			foreach (var a in rg) {
				TestContext.WriteLine(a.ToString());
			}

			TestContext.WriteLine(Enumeration.GetNextId<EnumerationTestType>());
		}

		[Test]
		public void GuardTest()
		{
			Assert.Throws<Exception>(() =>
			{
				Guard.Assert(false);
				
			});

			Assert.Throws<NullReferenceException>(() =>
			{
				Guard.AssertNotNull(null);

			});

			Assert.Throws<ArgumentNullException>(() =>
			{
				Guard.AssertArgumentNotNull(null);

			});


			Assert.Throws<Exception>(() =>
			{
				Guard.AssertEqual("a","b");

			});

			Assert.DoesNotThrow(() =>
			{
				Guard.AssertEqual("a","a");

			});


			Assert.Throws<Exception>(() =>
			{
				Guard.Fail();
			});
		}

		[Test]
		public void StringTest()
		{
			Assert.Null(Strings.NullIfNullOrWhiteSpace("    "));
			Assert.Null(Strings.NullIfNullOrWhiteSpace(""));
			Assert.Null(Strings.NullIfNullOrWhiteSpace(null));
		}

		[Test]
		public void CollectionsTest()
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