using BitRippleModel;
using System.Collections.Generic;

namespace BitRippleUtility
{
	public interface IRssReader
	{
		ICollection<Torrent> FetchFeed(string url);
	}
}