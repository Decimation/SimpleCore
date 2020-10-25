using System;
using System.Diagnostics;
using System.Threading;

namespace SimpleCore.CommandLine
{
	public sealed class KeyReadEventArgs : EventArgs
	{
		public ConsoleKeyInfo KeyInfo { get; }

		public KeyReadEventArgs(ConsoleKeyInfo keyInfo)
		{
			KeyInfo = keyInfo;
		}
	}

	public static class NConsoleHook
	{
		//todo

		public delegate void KeyReadEventHandler(object sender, KeyReadEventArgs k);

		public delegate void DisplayUI();

		private static Thread Thread { get; set; }

		public static bool Active { get; private set; }

		public static DisplayUI Display { get; set; }

		
		public static void Start()
		{
			Thread = new Thread(watch)
			{
				IsBackground = true,
				Priority = ThreadPriority.Highest
			};
			Thread.Start();
			KeyRead += InternalRead;
			Active = true;
		}

		public static void close()
		{
			Active = false;
			
			Debug.WriteLine("closing");
			__wait();
		}

		public static void __wait()
		{
			SpinWait.SpinUntil(() => !Thread.IsAlive);
		}

		public static event KeyReadEventHandler KeyRead;

		private static void OnKeyRead(KeyReadEventArgs k)
		{
			var handler = KeyRead;

			handler?.Invoke(null, k);
		}

		private static void InternalRead(object sender, KeyReadEventArgs args)
		{
			// if (args.KeyInfo.Key == ConsoleKey.Escape) {
			// 	close();
			// }
		}

		

		private static void watch()
		{
			ConsoleKeyInfo cki;

			do {
				if (!Active) {
					break;
				}
				Console.Clear();

				Display?.Invoke();

				Trace.WriteLine("watch...");

				while (!Console.KeyAvailable) {
					// Block until input is entered.
				}

				// Key was read
				cki = Console.ReadKey(true);
				var args = new KeyReadEventArgs(cki);
				OnKeyRead(args);

			} while (Active);
		}
	}
}