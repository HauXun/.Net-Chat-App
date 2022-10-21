using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dotNet_Chat_App.Services
{
	public class AsynchronousServices
	{
		public static void setTimeout(Action TheAction, int Timeout)
		{
			Thread t = new Thread(
				() =>
				{
					Thread.Sleep(Timeout);
					TheAction.Invoke();
				}
			);
			t.Start();
			t.Join();

			t.Interrupt();
			t.Abort();

			t = null;
		}

		public static async Task setInterval(Action action, TimeSpan timeout)
		{
			await Task.Delay(timeout).ConfigureAwait(false);

			action();

			setInterval(action, timeout);
		}
	}
}
