using Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Repository
{
	 public class DownloadRepository
	 {
		  public readonly Context _context;

		  public DownloadRepository(Context context)
		  {
				_context = context;
		  }

		  public RepositoryResult RegisterDownload(Download download)
		  {
				_context.Data.Downloads.Add(download);
				_context.Data.SaveChanges();
				return RepositoryResult.SuccessResult;
		  }

		  public RepositoryResult<List<Torrent>> DownloadFeed(Feed feed)
		  {
				if (String.IsNullOrWhiteSpace(feed.Url))
				{
					 return RepositoryResult.CreateError<List<Torrent>>($"Feed <{feed.Name}> is missing URL");
				}

				try
				{
					 _context.Data.Settings.FeedDownloadCount++;
					 _context.Data.SaveChanges();
					 return RepositoryResult.Create(_context.RssReader.FetchFeed(feed.Url));
				}
				catch (Exception)
				{
					 return RepositoryResult.CreateError<List<Torrent>>($"Couldn't download feed <{feed.Name}>");
				}
		  }

		  public RepositoryResult DownloadTorrent(Download download)
		  {
				try
				{
					 if (_context.Data.Settings.Location != null && !Directory.Exists(_context.Data.Settings.Location))
					 {
						  return RepositoryResult.CreateError($"Couldn't find download location for <{download.Name}>");
					 }

					 _context.TorrentDownloader.Download(download, _context.Data.Settings.Location);
					 _context.Data.Downloads.Add(download);
					 _context.Data.Settings.TorrentDownloadCount++;
					 _context.Data.SaveChanges();
					 return RepositoryResult.SuccessResult;
				}
				catch (Exception)
				{
					 return RepositoryResult.CreateError($"Couldn't download torrent <{download.Name}>");
				}
		  }
	 }
}