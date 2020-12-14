using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
// ReSharper disable UnusedMember.Global
namespace SimpleCore.Selenium.Utilities
{
	public static class Scrolling
	{
		public static void ScrollToBottom(this IWebDriver driver)
		{
			((IJavaScriptExecutor) driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
		}

		public static void ScrollToTop(this IWebDriver driver)
		{
			((IJavaScriptExecutor) driver).ExecuteScript("window.scrollTo(document.body.scrollHeight, 0)");
		}

		public static void ScrollBy(this IWebDriver driver, int x, int y)
		{
			((IJavaScriptExecutor) driver).ExecuteScript(String.Format("scrollBy({0},{1})", x, y));
		}

		public static void ScrollIntoView(this IWebDriver driver, IWebElement element)
		{
			// document.getElementById('Chk_3_4000001871527773').scrollIntoView()
			// .scrollIntoView()

			var js = (IJavaScriptExecutor) driver;
			js.ExecuteScript("arguments[0].scrollIntoView();", element);
		}

		public static void ScrollToElement(this IWebDriver driver, IWebElement element)
		{
			var actions = new Actions(driver);
			actions.MoveToElement(element);

			actions.Perform();
		}
	}
}