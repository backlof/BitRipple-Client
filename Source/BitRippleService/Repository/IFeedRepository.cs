using BitRippleService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRippleService.Repository
{
	public interface IFeedRepository
	{
		void AddDownload(Download download);
		ICollection<Feed> GetFeeds();
	}
}
