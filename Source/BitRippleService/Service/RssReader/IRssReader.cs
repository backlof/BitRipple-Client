using System.Collections.Generic;
using BitRippleService.Model;

namespace BitRippleService.Service
{
	 public interface IRssReader
	 {
		  ICollection<Torrent> FetchFeed(string url);
	 }
}