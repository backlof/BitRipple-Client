namespace BitRippleService.Repository
{
	public class BitRippleRepository
	{
		public IFeedRepository Feed { get; set; }

		public BitRippleRepository()
		{
		}

		public BitRippleRepository(BitRippleContext context)
		{
			Feed = new FeedRepository(context);
		}
	}
}