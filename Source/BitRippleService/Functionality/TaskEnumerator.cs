using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitRippleService.Functionality
{
	public class TaskEnumerator
	{
		public static async Task<IEnumerable<TOut>> WhenAll<TIn, TOut>(Func<TIn, TOut> func, IEnumerable<TIn> items)
		{
			return await Task.WhenAll(items.Select(item => Task.Run(() => func(item))));
		}

		public static async Task WhenAll<T>(Action<T> action, IEnumerable<T> items)
		{
			await Task.WhenAll(items.Select(item => Task.Run(() => action(item))));
		}
	}
}