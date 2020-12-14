using System;
using OpenQA.Selenium;
// ReSharper disable UnusedMember.Global
namespace SimpleCore.Selenium.Utilities
{
	public static class Selectors
	{
		public static By ClassNameContains(string s)
		{
			return By.XPath(String.Format("//*[contains(@class, \"{0}\")]", s));
		}
		
		// todo

		public static By ClassNameContainsCurrent(string s)
		{
			return By.XPath(String.Format(".//*[contains(@class, \"{0}\")]", s));
		}
		
		/// <summary>
		/// Creates an XPath selector which selects anything whose text contains <paramref name="txt"/> .
		/// </summary>
		public static By TextContains(string txt)
		{
			// //*[text()[contains(.,'ABC')]]
			return By.XPath(String.Format("//*[text()[contains(.,'{0}')]]", txt));
		}

		public static By Parent()
		{
			return By.XPath("./..");
		}
	}
}