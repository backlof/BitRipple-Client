using System;
using System.Collections.Generic;
using System.Linq;

namespace BitRippleUtility
{
	public class FeedFilterer : IFilterFeed
	{
		public ICollection<Download> FindMatchingTorrents(Feed feed)
		{
			return InternalFindMatchingTorrents(feed).ToArray();
		}

		public IEnumerable<Download> InternalFindMatchingTorrents(Feed feed)
		{
			foreach (var filter in feed.Filters)
			{
				foreach (var torrent in feed.Torrents)
				{
					if (IsMatch(filter, torrent))
					{
						Download download = new Download
						{
							FeedId = feed.Id,
							FilterId = filter.Id,
							GUID = torrent.GUID,
							Name = torrent.Name,
							FileUrl = torrent.FileUrl,
							TimeOfUpload = torrent.TimeOfUpload,
							TimeOfDownload = DateTime.Now
						};

						filter.Downloads.Add(download); // Necessary for future filtering

						yield return download;
					}
				}
			}
		}

		public bool IsMatch(Filter filter, Torrent torrent)
		{
			if (filter.Disabled)
				return false;
			else if (filter.HasDownloadedBefore(torrent))
				return false;
			else if (!filter.DoesTitleMatch(torrent))
				return false;
			else if (!filter.IsTvOfAnyKind && filter.OnlyMatchOnce && filter.HasDownloads())
				return false;
			else if (filter.IsTvShow && (!torrent.IsTvShow || !filter.ShouldDownloadTvShow(torrent)))
				return false;
			else if (filter.IsMiniShow && (!torrent.IsMiniSeries || !filter.ShouldDownloadMiniSeries(torrent)))
				return false;
			else if (filter.IsSeasonPack && (!torrent.IsSeasonPack || !filter.ShouldDownloadSeasonPack(torrent)))
				return false;
			else
				return true;
		}
	}
}