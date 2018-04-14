using System;

namespace BitRippleRepository.Context
{
	public class DisposableSqLiteDbContext : SQLiteDbContext, IDisposable
	{
		public override void Dispose()
		{
			DeleteDatabase();
			base.Dispose();
		}
	}
}