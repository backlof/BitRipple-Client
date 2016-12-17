using BitRippleService.Functionality;
using BitRippleService.Model;
using BitRippleService.Repository;
using BitRippleService.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BitRippleService
{
	public class ApplicationService
	{
		public BitRippleServices Service { get; set; }
		public BitRippleRepository Repository { get; set; }

		public ApplicationService(BitRippleServices service, BitRippleRepository repository)
		{
			Service = service;
			Repository = repository;
		}

		public async Task Update()
		{
			Logger.Log("Updating feeds...");

			var updatedFeeds = await TaskEnumerator.WhenAll(FetchRss, Repository.Feed.GetFeeds());

			var torrentsToDownload = await TaskEnumerator.WhenAll(FindTorrentsToDownload, updatedFeeds);

			await TaskEnumerator.WhenAll(DownloadTorrent, torrentsToDownload.SelectMany(x => x));
		}

		public Feed FetchRss(Feed feed)
		{
			if (String.IsNullOrWhiteSpace(feed.Url))
			{
				Logger.WriteError($"Feed <{feed.Url}> is missing URL");
			}
			else
			{
				try
				{
					feed.Torrents = Service.RssReader.FetchFeed(feed.Url);
				}
				catch (Exception)
				{
					Logger.WriteError($"Couldn't download feed <{feed.Name}>");
				}
			}

			return feed;
		}

		public void DownloadTorrent(Download torrent)
		{
			if (String.IsNullOrWhiteSpace(Service.Settings.Location) && !Directory.Exists(Service.Settings.Location))
			{
				Logger.WriteError($"Couldn't find the download location for <{torrent.Name}>");
			}
			else
			{
				try
				{
					Service.TorrentDownloader.Download(torrent.FileUrl, Service.Settings.Location, torrent.CleanName);
					Repository.Feed.AddDownload(torrent);
				}
				catch (Exception)
				{
					Logger.WriteError($"Couldn't download <{torrent.Name}>");
				}
			}
		}

		public ICollection<Download> FindTorrentsToDownload(Feed feed)
		{
			try
			{
				return Service.Filter.FindMatchingTorrents(feed);
			}
			catch (Exception)
			{
				Logger.WriteError($"Couldn't filter feed <{feed.Name}>");
				return new Download[] { };
			}
		}
	}
}