namespace BitRippleService.Repository
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