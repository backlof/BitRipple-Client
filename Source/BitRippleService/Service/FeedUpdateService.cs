using BitRippleService.Functionality;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BitRippleRepository;
using BitRippleUtility;
using BitRippleModel;
using BitRippleShared;

namespace BitRippleService.Service
{
	public class FeedUpdateService
	{
		// Move everything related to settings into this repository / Not a service

		private readonly Utilities _utility;
		private readonly Repositories _repository;

		public FeedUpdateService(Utilities utility, Repositories repository)
		{
			_utility = utility;
			_repository = repository;
		}

		public async Task Update(ILogger logger, string droplocation)
		{
			Logger.Log("Updating feeds...");

			var updatedFeeds = await TaskEnumerator.WhenAll(FetchRss, logger, GetFeeds());

			var torrentsToDownload = await TaskEnumerator.WhenAll(FindTorrentsToDownload, logger, updatedFeeds);

			await TaskEnumerator.WhenAll(DownloadTorrent, logger, torrentsToDownload.SelectMany(x => x));
		}

		private ICollection<Feed> GetFeeds()
		{
			return _repository.Feeds.Entities.Select(x => new Feed
			{
				Id = x.Id,
				Name = x.Name,
				Filters = x.Filters.Select(y => new Filter
				{
					Disabled = y.Disabled,
					Episode = y.Episode,
					Exclude = y.Exclude,
					Id = y.Id,
					Include = y.Include,
					Name = y.Name,
					Season = y.Season,
					TitleMatch = y.TitleMatch,
					OnlyMatchOnce = y.OnlyMatchOnce,
					Downloads = y.Downloads.Select(z => new Download
					{
						Id = z.Id,
						FileUrl = z.FileUrl,
						GUID = z.GUID,
						Name = z.Name,
						TimeOfDownload = z.TimeOfDownload,
						TimeOfUpload = z.TimeOfUpload,
						Season = z.Season,
						Episode = z.Episode
					}).ToList()
				}).ToList(),
				Url = x.URL,

			}).ToList();
		}

		public Feed FetchRss(ILogger logger, Feed feed)
		{
			if (String.IsNullOrWhiteSpace(feed.Url))
			{
				Logger.WriteError($"Feed <{feed.Url}> is missing URL");
			}
			else
			{
				try
				{
					feed.Torrents = _utility.RssReader.FetchFeed(feed.Url);
				}
				catch (Exception)
				{
					Logger.WriteError($"Couldn't download feed <{feed.Name}>");
				}
			}

			return feed;
		}

		public void DownloadTorrent(ILogger logger, Download torrent)
		{
			Logger.Log($"Downloading <{torrent.Name}>");

			if (String.IsNullOrWhiteSpace(_utility.Settings.Location) && !Directory.Exists(_utility.Settings.Location))
			{
				Logger.WriteError($"Can't download torrent <{torrent.Name}> to location <{_utility.Settings.Location}>");
			}
			else
			{
				try
				{
					_utility.TorrentDownloader.Download(torrent.FileUrl, _utility.Settings.Location, torrent.Name.CleanInvalidFileNameChars());
					_repository.Downloads.Insert(new BitRippleRepository.Table.Download
					{
						FileUrl = torrent.FileUrl,
						GUID = torrent.GUID,
						Name = torrent.Name,
						Season = torrent.Season,
						Episode = torrent.Episode,
						TimeOfDownload = DateTime.Now,
						TimeOfUpload = torrent.TimeOfUpload,
						FeedId = torrent.FeedId,
						FilterId = torrent.FilterId,

					});
				}
				catch (Exception e)
				{
					Logger.WriteError($"Couldn't download <{torrent.Name}>");
				}
			}
		}

		public ICollection<Download> FindTorrentsToDownload(ILogger logger, Feed feed)
		{
			try
			{
				return _utility.Filter.FindMatchingTorrents(feed);
			}
			catch (Exception)
			{
				Logger.WriteError($"Couldn't filter feed <{feed.Name}>");
				return new Download[] { };
			}
		}
	}
}