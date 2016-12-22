using System;
using System.Threading;
using System.Threading.Tasks;

namespace BitRippleClient
{
	public class ConsoleInterval
	{
		public static void Run(Func<Task> action, TimeSpan ts)
		{
			Run(action, ts, new CancellationTokenSource());
		}

		private static void Run(Func<Task> action, TimeSpan ts, CancellationTokenSource tks)
		{
			Run(action, ts, () => { Environment.Exit(0); }, new CancellationTokenSource());
			Console.Read();
			tks.Cancel();
		}

		public static async void Run(Func<Task> action, TimeSpan ts, Action end, CancellationTokenSource tks)
		{
			try
			{
				do
				{
					await Task.Run(action);
					await Task.Delay(ts, tks.Token);

				} while (!tks.IsCancellationRequested);
			}
			catch (TaskCanceledException)
			{
				end();
			}
		}
	}
}