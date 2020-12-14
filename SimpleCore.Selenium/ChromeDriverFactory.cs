using System.Linq;
using OpenQA.Selenium.Chrome;
// ReSharper disable UnusedMember.Global

//https://github.com/Decimation/Bromine
//C:\Users\Deci\RiderProjects\Bromine


namespace SimpleCore.Selenium
{
	/// <summary>
	/// Provides utilities for <see cref="ChromeDriver"/>
	/// </summary>
	public static class ChromeDriverFactory
	{
		/// <summary>
		/// 
		/// </summary>
		public const string NAME = "chromedriver";

		public static ChromeDriver CreateQuietHeadless(params string[] extraArgs)
		{
			var list = extraArgs.ToList();
			list.Add("--headless");

			return CreateQuiet(list.ToArray());
		}

		public static ChromeDriver CreateQuiet(params string[] extraArgs)
		{
			var co = new ChromeOptions();

			co.AddArguments("--silent", "--log-level=3", "--disable-infobars");
			co.AddArguments("--mute-audio", "--disable-gpu");
//			co.AddArguments("--disable-extensions","start-maximized");

			if (extraArgs?.Length > 0)
				co.AddArguments(extraArgs);

			// hide logs
			var service = ChromeDriverService.CreateDefaultService();
			service.HideCommandPromptWindow = true;

			return new ChromeDriver(service, co);
		}
	}
}