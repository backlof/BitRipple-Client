using BitRippleService.Model;
using System.Collections.Generic;
using System.Linq;

namespace BitRippleService.Repository
{
	public class FeedRepository : IFeedRepository
	{
		private readonly BitRippleContext _context;

		public FeedRepository(BitRippleContext context)
		{
			_context = context;
		}

		public void AddDownload(Download download)
		{
			_context.Downloads.Add(download);
			_context.SaveChanges();
		}

		public ICollection<Feed> GetFeeds()
		{
			return _context.Feeds.Select(x => new Feed
			{
				Downloads = x.Downloads,
				Filters = x.Filters,
				Id = x.Id,
				Name = x.Name,
				Url = x.Url
			}).ToList();
		}
	}
}