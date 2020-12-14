using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
// ReSharper disable UnusedMember.Global
namespace SimpleCore.Selenium.Utilities
{
	public static class Tabs
	{
		public static void LeftTab(this IWebDriver driver)
		{
			new Actions(driver).SendKeys(Keys.Control).SendKeys(Keys.Shift).SendKeys(Keys.Tab).Build().Perform();
		}

		public static void RightTab(this IWebDriver driver)
		{
			new Actions(driver).SendKeys(Keys.Control).SendKeys(Keys.Tab).Build().Perform();
		}

		public static void SelectTab(this IWebDriver driver, Predicate<IWebDriver> fn)
		{
			var current = driver.CurrentWindowHandle;
			foreach (string hnd in driver.WindowHandles) {
				if (fn(driver.SwitchTo().Window(hnd))) {
					return;
				}
			}

			driver.SwitchTo().Window(current);
		}

		public static void SwitchToTabByUrl(this IWebDriver driver, string url)
		{
			driver.SelectTab(tab => tab.Url.Contains(url));
		}

		public static void SwitchToTabByTitle(this IWebDriver driver, string title)
		{
			driver.SelectTab(tab => tab.Title.Contains(title));
		}

		public static void CreateNewTab(this IWebDriver webDriver, string url)
		{
			var windowHandles  = webDriver.WindowHandles;
			var scriptExecutor = (IJavaScriptExecutor) webDriver;
			scriptExecutor.ExecuteScript(String.Format("window.open('{0}', '_blank');", url));
			var newWindowHandles   = webDriver.WindowHandles;
			var openedWindowHandle = newWindowHandles.Except(windowHandles).Single();
			webDriver.SwitchTo().Window(openedWindowHandle);
		}

		public static bool TabExists(this IWebDriver driver, string title)
		{
			var current = driver.CurrentWindowHandle;
			foreach (string hnd in driver.WindowHandles) {
				if (driver.SwitchTo().Window(hnd).Title.Contains(title)) {
					driver.SwitchTo().Window(current);
					return true;
				}
			}

			driver.SwitchTo().Window(current);
			return false;
		}


		public static void CloseTab(this IWebDriver driver, string title)
		{
			driver.SwitchToTabByTitle(title);
			driver.Close();
		}

		/// <summary>
		/// Creates a new tab.
		/// </summary>
		/// <param name="driver"><see cref="ChromeDriver"/> instance</param>
		/// <param name="url">Destination</param>
		/// <returns><see cref="string"/> window handle of previous tab</returns>
		public static string NewTab(this ChromeDriver driver, string url)
		{
			((IJavaScriptExecutor) driver).ExecuteScript("window.open()");

			var tabs = driver.WindowHandles;
			driver.SwitchTo().Window(tabs[1]); //switches to new tab
			driver.Url = (url);

			// Return previous tab handle
			return tabs[0];
		}
	}
}