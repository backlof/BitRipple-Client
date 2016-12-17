using BitRippleService.Model;
using System.Collections.Generic;

namespace BitRippleService.Repository
{
	public interface IFeedRepository
	{
		void AddDownload(Download download);

		ICollection<Feed> GetFeeds();
	}
}