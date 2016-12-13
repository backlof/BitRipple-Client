using System;
using System.Threading;
using System.Threading.Tasks;

namespace BitRipple
{
	 public class ConsoleInterval
	 {
		  public static void Run(Task action, TimeSpan ts)
		  {
				InternalRun(action, ts, new CancellationTokenSource());
		  }

		  public static void Run(Action action, TimeSpan ts)
		  {
				InternalRun(Task.Run(action), ts, new CancellationTokenSource());
		  }

		  private static void InternalRun(Task action, TimeSpan ts, CancellationTokenSource tks)
		  {
				IntervalRunner(action, ts, tks);
				Console.Read();
				tks.Cancel();
		  }

		  private static async void IntervalRunner(Task action, TimeSpan ts, CancellationTokenSource tks)
		  {
				try
				{
					 do
					 {
						  action.Wait();
						  await Task.Delay(ts, tks.Token);
					 } while (tks.IsCancellationRequested);
				}
				catch (TaskCanceledException)
				{
					 Environment.Exit(0);
				}
		  }
	 }
}