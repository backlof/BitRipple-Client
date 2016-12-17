using BitRippleService.Model;
using System.Collections.Generic;

namespace BitRippleService.Service
{
	public interface IFilterFeed
	{
		ICollection<Download> FindMatchingTorrents(Feed feed);
	}
}