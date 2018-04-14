using BitRippleRepository.Context;
using BitRippleRepository.Table;

namespace BitRippleRepository
{
	public class Repositories
	{
		public Entity<Download> Downloads { get; set; }
		public Entity<Feed> Feeds { get; set; }
		public Entity<Filter> Filters { get; set; }

		public Repositories(BitRippleContext context)
		{
			Downloads = new Entity<Download>(context, context.Downloads);
			Feeds = new Entity<Feed>(context, context.Feeds);
			Filters = new Entity<Filter>(context, context.Filters);
		}
	}
}