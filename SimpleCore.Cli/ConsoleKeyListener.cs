using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace SimpleCore.Cli
{
	public static class ConsoleKeyListener
	{
		public delegate void KeyEventHandler(object sender, EventArgs e);

		public static event KeyEventHandler KeyInputEvent;


		private static Thread ListenThread { get; set; }

		public class KeyEventArgs : EventArgs
		{
			public ConsoleKeyInfo key;
		}

		private static void Listen()
		{
			Trace.WriteLine($"listening");

			ConsoleKeyInfo cki;


			// Block until input is entered.
			while (!Console.KeyAvailable) { }

			cki = Console.ReadKey(true);


			// Key was read


			Trace.WriteLine($"{cki.Key} {cki.KeyChar}");
			KeyInputEvent?.Invoke(null, new KeyEventArgs() {key = cki});
		}

		public static void Start()
		{
			ListenThread = new(Listen) {Priority = ThreadPriority.Highest};
			ListenThread.Start();

			SpinWait.SpinUntil(() => ListenThread.IsAlive);
		}

		public static void End()
		{

			ListenThread.Join();
		}
	}
}