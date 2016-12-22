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

		public void AddFeed(Feed feed)
		{
			_context.Feeds.Add(feed);
			_context.SaveChanges();
		}

		public void AddFilter(Filter filter)
		{
			_context.Filters.Add(filter);
			_context.SaveChanges();
		}

		public ICollection<Feed> GetFeeds()
		{
			return _context.Feeds
				.Select(x => new Feed
				{
					Downloads = x.Downloads,
					Filters = x.Filters.Select(y => new Filter
					{
						Downloads = y.Downloads,
						Disabled = y.Disabled,
						Episode = y.Episode,
						Exclude = y.Exclude,
						FeedId = y.FeedId,
						Id = y.Id,
						Include = y.Include,
						Name = y.Name,
						OnlyMatchOnce = y.OnlyMatchOnce,
						Season = y.Season,
						TitleMatch = y.TitleMatch,
						Feed = y.Feed
					}).ToArray(),
					Id = x.Id,
					Name = x.Name,
					Url = x.Url
				}).ToList();
		}
	}
}