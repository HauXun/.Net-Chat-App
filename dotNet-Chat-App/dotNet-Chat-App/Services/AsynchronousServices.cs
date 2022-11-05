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
		public static void setTimeout(Action theAction, TimeSpan timeout)
		{
			SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
			Task.Delay(timeout).ContinueWith((task) =>
			{
				theAction();
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		public static async Task setInterval(Action action, TimeSpan timeout, CancellationToken token)
		{
			await Task.Delay(timeout).ConfigureAwait(false);

			if (token.IsCancellationRequested)
			{
				// another thread decided to cancel
				Console.WriteLine("action canceled");
				return;
			}
			action();

			setInterval(action, timeout, token);
		}
	}
}
