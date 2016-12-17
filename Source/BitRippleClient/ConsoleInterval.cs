using System;
using System.Threading;
using System.Threading.Tasks;

namespace BitRippleClient
{
	public class ConsoleInterval
	{
		public static void Run(Func<Task> action, TimeSpan ts)
		{
			InternalRun(action, ts, new CancellationTokenSource());
		}

		private static void InternalRun(Func<Task> action, TimeSpan ts, CancellationTokenSource tks)
		{
			IntervalRunner(action, ts, tks);
			Console.Read();
			tks.Cancel();
		}

		private static async void IntervalRunner(Func<Task> action, TimeSpan ts, CancellationTokenSource tks)
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
				Environment.Exit(0);
			}
		}
	}
}