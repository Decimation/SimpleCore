using System;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable UnusedMember.Global
#pragma warning disable CA1416, CA2211
namespace SimpleCore.Cli
{
	public static class NConsoleProgress
	{
		/*
		 * https://github.com/sindresorhus/cli-spinners/blob/main/spinners.json
		 *
		 * https://jsbin.com/tofehujixe/1/edit?js,output
		 * https://www.npmjs.com/package/cli-spinners
		 * https://www.fileformat.info/info/unicode/block/braille_patterns/images.htm
		 */

		#region Spinners

		public static string[] Dots =
		{
			"⠋",
			"⠙",
			"⠹",
			"⠸",
			"⠼",
			"⠴",
			"⠦",
			"⠧",
			"⠇",
			"⠏"
		};

		public static string[] Dots2 =
		{
			"⣾",
			"⣽",
			"⣻",
			"⢿",
			"⡿",
			"⣟",
			"⣯",
			"⣷"
		};

		public static string[] Dots3 =
		{
			"⠋",
			"⠙",
			"⠚",
			"⠞",
			"⠖",
			"⠦",
			"⠴",
			"⠲",
			"⠳",
			"⠓"
		};

		public static string[] Dots4 =
		{
			"⠄",
			"⠆",
			"⠇",
			"⠋",
			"⠙",
			"⠸",
			"⠰",
			"⠠",
			"⠰",
			"⠸",
			"⠙",
			"⠋",
			"⠇",
			"⠆"
		};

		public static string[] Dots5 =
		{
			"⠋",
			"⠙",
			"⠚",
			"⠒",
			"⠂",
			"⠂",
			"⠒",
			"⠲",
			"⠴",
			"⠦",
			"⠖",
			"⠒",
			"⠐",
			"⠐",
			"⠒",
			"⠓",
			"⠋"
		};

		public static string[] Dots8Bit =
		{
			"⠀",
			"⠁",
			"⠂",
			"⠃",
			"⠄",
			"⠅",
			"⠆",
			"⠇",
			"⡀",
			"⡁",
			"⡂",
			"⡃",
			"⡄",
			"⡅",
			"⡆",
			"⡇",
			"⠈",
			"⠉",
			"⠊",
			"⠋",
			"⠌",
			"⠍",
			"⠎",
			"⠏",
			"⡈",
			"⡉",
			"⡊",
			"⡋",
			"⡌",
			"⡍",
			"⡎",
			"⡏",
			"⠐",
			"⠑",
			"⠒",
			"⠓",
			"⠔",
			"⠕",
			"⠖",
			"⠗",
			"⡐",
			"⡑",
			"⡒",
			"⡓",
			"⡔",
			"⡕",
			"⡖",
			"⡗",
			"⠘",
			"⠙",
			"⠚",
			"⠛",
			"⠜",
			"⠝",
			"⠞",
			"⠟",
			"⡘",
			"⡙",
			"⡚",
			"⡛",
			"⡜",
			"⡝",
			"⡞",
			"⡟",
			"⠠",
			"⠡",
			"⠢",
			"⠣",
			"⠤",
			"⠥",
			"⠦",
			"⠧",
			"⡠",
			"⡡",
			"⡢",
			"⡣",
			"⡤",
			"⡥",
			"⡦",
			"⡧",
			"⠨",
			"⠩",
			"⠪",
			"⠫",
			"⠬",
			"⠭",
			"⠮",
			"⠯",
			"⡨",
			"⡩",
			"⡪",
			"⡫",
			"⡬",
			"⡭",
			"⡮",
			"⡯",
			"⠰",
			"⠱",
			"⠲",
			"⠳",
			"⠴",
			"⠵",
			"⠶",
			"⠷",
			"⡰",
			"⡱",
			"⡲",
			"⡳",
			"⡴",
			"⡵",
			"⡶",
			"⡷",
			"⠸",
			"⠹",
			"⠺",
			"⠻",
			"⠼",
			"⠽",
			"⠾",
			"⠿",
			"⡸",
			"⡹",
			"⡺",
			"⡻",
			"⡼",
			"⡽",
			"⡾",
			"⡿",
			"⢀",
			"⢁",
			"⢂",
			"⢃",
			"⢄",
			"⢅",
			"⢆",
			"⢇",
			"⣀",
			"⣁",
			"⣂",
			"⣃",
			"⣄",
			"⣅",
			"⣆",
			"⣇",
			"⢈",
			"⢉",
			"⢊",
			"⢋",
			"⢌",
			"⢍",
			"⢎",
			"⢏",
			"⣈",
			"⣉",
			"⣊",
			"⣋",
			"⣌",
			"⣍",
			"⣎",
			"⣏",
			"⢐",
			"⢑",
			"⢒",
			"⢓",
			"⢔",
			"⢕",
			"⢖",
			"⢗",
			"⣐",
			"⣑",
			"⣒",
			"⣓",
			"⣔",
			"⣕",
			"⣖",
			"⣗",
			"⢘",
			"⢙",
			"⢚",
			"⢛",
			"⢜",
			"⢝",
			"⢞",
			"⢟",
			"⣘",
			"⣙",
			"⣚",
			"⣛",
			"⣜",
			"⣝",
			"⣞",
			"⣟",
			"⢠",
			"⢡",
			"⢢",
			"⢣",
			"⢤",
			"⢥",
			"⢦",
			"⢧",
			"⣠",
			"⣡",
			"⣢",
			"⣣",
			"⣤",
			"⣥",
			"⣦",
			"⣧",
			"⢨",
			"⢩",
			"⢪",
			"⢫",
			"⢬",
			"⢭",
			"⢮",
			"⢯",
			"⣨",
			"⣩",
			"⣪",
			"⣫",
			"⣬",
			"⣭",
			"⣮",
			"⣯",
			"⢰",
			"⢱",
			"⢲",
			"⢳",
			"⢴",
			"⢵",
			"⢶",
			"⢷",
			"⣰",
			"⣱",
			"⣲",
			"⣳",
			"⣴",
			"⣵",
			"⣶",
			"⣷",
			"⢸",
			"⢹",
			"⢺",
			"⢻",
			"⢼",
			"⢽",
			"⢾",
			"⢿",
			"⣸",
			"⣹",
			"⣺",
			"⣻",
			"⣼",
			"⣽",
			"⣾",
			"⣿"
		};

		public static string[] Dots9 =
		{
			"⢹",
			"⢺",
			"⢼",
			"⣸",
			"⣇",
			"⡧",
			"⡗",
			"⡏"
		};

		public static string[] Dots10 =
		{
			"⢄",
			"⢂",
			"⢁",
			"⡁",
			"⡈",
			"⡐",
			"⡠"
		};

		public static string[] Dots11 =
		{
			"⠁",
			"⠂",
			"⠄",
			"⡀",
			"⢀",
			"⠠",
			"⠐",
			"⠈"
		};

		public static string[] Dots12 =
		{
			"⢀⠀",
			"⡀⠀",
			"⠄⠀",
			"⢂⠀",
			"⡂⠀",
			"⠅⠀",
			"⢃⠀",
			"⡃⠀",
			"⠍⠀",
			"⢋⠀",
			"⡋⠀",
			"⠍⠁",
			"⢋⠁",
			"⡋⠁",
			"⠍⠉",
			"⠋⠉",
			"⠋⠉",
			"⠉⠙",
			"⠉⠙",
			"⠉⠩",
			"⠈⢙",
			"⠈⡙",
			"⢈⠩",
			"⡀⢙",
			"⠄⡙",
			"⢂⠩",
			"⡂⢘",
			"⠅⡘",
			"⢃⠨",
			"⡃⢐",
			"⠍⡐",
			"⢋⠠",
			"⡋⢀",
			"⠍⡁",
			"⢋⠁",
			"⡋⠁",
			"⠍⠉",
			"⠋⠉",
			"⠋⠉",
			"⠉⠙",
			"⠉⠙",
			"⠉⠩",
			"⠈⢙",
			"⠈⡙",
			"⠈⠩",
			"⠀⢙",
			"⠀⡙",
			"⠀⠩",
			"⠀⢘",
			"⠀⡘",
			"⠀⠨",
			"⠀⢐",
			"⠀⡐",
			"⠀⠠",
			"⠀⢀",
			"⠀⡀"
		};

		public static string[] Progress7 =
		{
			"▰▱▱▱▱▱▱",
			"▰▰▱▱▱▱▱",
			"▰▰▰▱▱▱▱",
			"▰▰▰▰▱▱▱",
			"▰▰▰▰▰▱▱",
			"▰▰▰▰▰▰▱",
			"▰▰▰▰▰▰▰",
			"▰▱▱▱▱▱▱"
		};

		public static string[] Progress10 =
		{
			"▰▱▱▱▱▱▱▱▱▱",
			"▰▰▱▱▱▱▱▱▱▱",
			"▰▰▰▱▱▱▱▱▱▱",
			"▰▰▰▰▱▱▱▱▱▱",
			"▰▰▰▰▰▱▱▱▱▱",
			"▰▰▰▰▰▰▱▱▱▱",
			"▰▰▰▰▰▰▰▱▱▱",
			"▰▰▰▰▰▰▰▰▱▱",
			"▰▰▰▰▰▰▰▰▰▱",
			"▰▰▰▰▰▰▰▰▰▰",
			"▰▱▱▱▱▱▱▱▱▱"
		};

		public static string[] Line =
		{
			"-",
			"\\",
			"|",
			"/"
		};

		#endregion


		public static string[] Current { get; set; } = Dots2;

		public static TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(80);

		public static void ForTask(Task t)
		{
			var cts = new CancellationTokenSource();
			Queue(cts);
			// ReSharper disable once MethodSupportsCancellation
			t.Wait();
			cts.Cancel();
			cts.Dispose();
		}

		public static void Queue(CancellationTokenSource cts)
		{
			// Pass the token to the cancelable operation.
			ThreadPool.QueueUserWorkItem(Show, cts.Token);
		}

		public static void Show(object obj)
		{
			var token = (CancellationToken) obj;

			var oldTitle = Console.Title;

			while (!token.IsCancellationRequested) {
				foreach (string t in Current) {
					Console.Title = $"{oldTitle} \r{t}";

					if (token.IsCancellationRequested) {
						break;
					}

					Thread.Sleep(Duration);
				}

			}

			Console.Title = oldTitle;
		}
	}
}