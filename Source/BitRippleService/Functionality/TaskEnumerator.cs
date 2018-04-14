using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitRippleService.Functionality
{
	public class TaskEnumerator
	{
		public static async Task<IEnumerable<TOut>> WhenAll<TIn, TOut>(Func<ILogger, TIn, TOut> func, ILogger logger, IEnumerable<TIn> items)
		{
			return await Task.WhenAll(items.Select(item => Task.Run(() => func(logger, item))));
		}

		public static async Task WhenAll<T>(Action<ILogger,T> action, ILogger logger, IEnumerable<T> items)
		{
			await Task.WhenAll(items.Select(item => Task.Run(() => action(logger,item))));
		}
	}
}