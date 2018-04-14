using BitRippleModel;
using System.Collections.Generic;

namespace BitRippleUtility
{
	public interface IFilterFeed
	{
		ICollection<Download> FindMatchingTorrents(Feed feed);
	}
}