using BitRippleService.Repository;
using System;

namespace BitRippleServiceTests
{
	public class DisposableBitRippleRepository : BitRippleRepositories, IDisposable
	{
		private readonly DisposableSqLiteDbContext _context;

		public DisposableBitRippleRepository(DisposableSqLiteDbContext context) : base(context)
		{
			_context = context;
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}