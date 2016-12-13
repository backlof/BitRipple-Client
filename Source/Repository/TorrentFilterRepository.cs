using Models;
using System;
using System.Collections.Generic;

namespace Repository
{
	 public class TorrentFilterRepository
	 {
		  public readonly Context _context;

		  public TorrentFilterRepository(Context context)
		  {
				_context = context;
		  }

		  public IEnumerable<Download> FindTorrentsMatchingFilters(Feed feed)
		  {
				foreach (var filter in feed.Filters)
				{
					 foreach (var torrent in feed.Torrents)
					 {
						  if (TorrentFilter.IsMatch(filter, torrent))
						  {
								Download download = new Download
								{
									 FeedId = feed.Id,
									 FilterId = filter.Id,
									 Id = _context.Data.Settings.NextIndex.Download,
									 GUID = torrent.GUID,
									 Name = torrent.Name,
									 FileUrl = torrent.FileUrl,
									 TimeOfUpload = torrent.TimeOfUpload,
									 TimeOfDownload = DateTime.Now
								};

								_context.Data.Settings.NextIndex.Download++;

								filter.Downloads.Add(download); // Necessary for future filtering

								yield return download;
						  }
					 }
				}
		  }
	 }
}