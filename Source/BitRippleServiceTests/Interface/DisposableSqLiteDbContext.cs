using BitRippleService.Repository;

namespace BitRippleServiceTests
{
	public class DisposableSqLiteDbContext : SQLiteDbContext
	{
		public override void Dispose()
		{
			base.Dispose();
			DeleteDatabase();
		}
	}
}