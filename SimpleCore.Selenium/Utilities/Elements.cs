using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
// ReSharper disable UnusedMember.Global

namespace SimpleCore.Selenium.Utilities
{
	public static class Elements
	{
		public static bool IsClickable(this IWebDriver driver, TimeSpan timeSpan, IWebElement element)
		{
			try {
				var wait = new WebDriverWait(driver, timeSpan);
				wait.Until(ExpectedConditions.ElementToBeClickable(element));
				return true;
			}
			catch (Exception) {
				return false;
			}
		}

		/// <summary>
		/// An expectation for checking whether an element is visible.
		/// </summary>
		/// <param name="locator">The locator used to find the element.</param>
		/// <returns>The <see cref="IWebElement"/> once it is located, visible and clickable.</returns>
		public static Func<IWebDriver, IWebElement> ElementIsClickable(By locator)
		{
			return driver =>
			{
				var element = driver.FindElement(locator);
				return (element != null && element.Displayed && element.Enabled) ? element : null;
			};
		}

		public static bool ElementExists(this IWebDriver driver, By by)
		{
			return driver.ElementExists(by, out _);
		}

		public static bool ElementExists(this IWebDriver driver, By by, out IWebElement element)
		{
			var elements = driver.FindElements(by);

			bool exists = elements.Count > 0;
			element = exists ? elements[0] : null;

			return exists;
		}

		public static bool IsElementPresent(this IWebDriver driver, By by)
		{
			try {
				driver.FindElement(by);
				return true;
			}
			catch (NoSuchElementException) {
				return false;
			}
		}
	}
}