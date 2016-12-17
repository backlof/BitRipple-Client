using BitRippleService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRippleService.Service
{
	public interface IFilterFeed
	 {
		  ICollection<Download> FindMatchingTorrents(Feed feed);
	 }
}
