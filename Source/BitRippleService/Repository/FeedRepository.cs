using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitRippleService.Model;

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
