using BitRippleService.Model;
using System.Collections.Generic;

namespace BitRippleService.Service
{
	public interface IRssReader
	{
		ICollection<Torrent> FetchFeed(string url);
	}
}