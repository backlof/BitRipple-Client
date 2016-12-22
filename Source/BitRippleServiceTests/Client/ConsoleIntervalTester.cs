using BitRippleClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BitRippleServiceTests.Client
{
	[TestClass]
	public class ConsoleIntervalTester
	{
		public int Counter { get; set; }
		public CancellationTokenSource CancellationTokenSource { get; set; }

		public async Task Timer()
		{
			Counter++;
		}

		[TestMethod]
		public void ShouldBeAbleToRunOnInterval()
		{
			Counter = 0;
			CancellationTokenSource = new CancellationTokenSource();
			ConsoleInterval.Run(Timer, TimeSpan.FromMilliseconds(50), () => { }, CancellationTokenSource);
			Thread.Sleep(50 * 10);
			CancellationTokenSource.Cancel();
			Assert.AreEqual(9, Counter);
		}
	}
}
