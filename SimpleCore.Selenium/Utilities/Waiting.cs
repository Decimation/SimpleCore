using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
// ReSharper disable UnusedMember.Global
namespace SimpleCore.Selenium.Utilities
{
	public static class Waiting
	{
		public static void WaitForLoad(this IWebDriver driver, int timeoutSec = 15)
		{
			var js   = (IJavaScriptExecutor) driver;
			var wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeoutSec));
			wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
		}


		public static void Wait(this IWebDriver driver, double delay, double interval)
		{
			// Causes the WebDriver to wait for at least a fixed delay
			var now = DateTime.Now;
			var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(delay))
			{
				PollingInterval = TimeSpan.FromMilliseconds(interval)
			};
			wait.Until(wd => (DateTime.Now - now) - TimeSpan.FromMilliseconds(delay) > TimeSpan.Zero);
		}

		public static void Wait(this IWebDriver driver, Func<IWebDriver, bool> condition, double delay)
		{
			var ignoredExceptions = new List<Type> {typeof(StaleElementReferenceException)};
			var wait              = new WebDriverWait(driver, TimeSpan.FromMilliseconds(delay));
			wait.IgnoreExceptionTypes(ignoredExceptions.ToArray());
			wait.Until(condition);
		}

		// todo: did I write this correctly?
		public static TResult WaitFind<TResult>(this IWebDriver driver, Func<IWebDriver, TResult> fn,
		                                        int             timeoutInSeconds = 2)
		{
			if (timeoutInSeconds > 0) {
				var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
				return wait.Until(fn);
			}

			return fn(driver);
		}

		// @formatter:off
		public static ReadOnlyCollection<IWebElement> WaitFindElements(this IWebDriver driver, By by, int timeoutInSeconds = 2)
		{
			if (timeoutInSeconds > 0) {
				var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
				return wait.Until(drv => drv.FindElements(by));
			}

			return driver.FindElements(by);
		}
		// @formatter:on

		public static IWebElement WaitFindElement(this IWebDriver driver, By by, int timeoutInSeconds = 2)
		{
			if (timeoutInSeconds > 0) {
				var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
				return wait.Until(drv => drv.FindElement(by));
			}

			return driver.FindElement(by);
		}
	}
}