using Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitRipple
{
	 public class ConsoleApplication : IApplicationService
	 {
		  public BitRippleRepository Repository { get; set; }

		  public ConsoleApplication(BitRippleRepository repository)
		  {
				Repository = repository;
		  }

		  public void Run()
		  {
				var loadResult = Repository.Data.Load();
				if (loadResult.Success)
				{
					 ConsoleInterval.Run(Task.Run(GoThroughFeeds), TimeSpan.FromMinutes(Repository.Data.Settings.Interval));
				}
				else
				{
					 Logger.WriteErrors(loadResult);
					 Logger.Log("Application did not start");
					 Console.Write("Press enter to exit: ");
					 Console.Read();
				}
		  }

		  private async Task GoThroughFeeds()
		  {
				Logger.Log("Updating...");

				var updatedFeeds = await TaskEnumerator.WhenAll(FetchRss, Repository.Data.Feeds);

				var torrentsToDownload = await TaskEnumerator.WhenAll(FindTorrentsToDownload, updatedFeeds);

				await TaskEnumerator.WhenAll(DownloadTorrent, torrentsToDownload.SelectMany(x => x));

				Repository.Data.Save();
		  }

		  public Feed FetchRss(Feed feed)
		  {
				System.Diagnostics.Debug.WriteLine("Feeder");

				var feedUpdateResult = Repository.Downloader.DownloadFeed(feed);
				if (feedUpdateResult.Success)
				{
					 feed.Torrents = feedUpdateResult.Value;
					 return feed;
				}
				else
				{
					 Logger.WriteErrors(feedUpdateResult);
					 return feed;
				}
		  }

		  public void DownloadTorrent(Download torrent)
		  {
				Logger.Log($"Downloading: <{torrent.Name}>");

				var downloadResult = Repository.Downloader.DownloadTorrent(torrent);
				if (!downloadResult.Success)
				{
					 Logger.WriteErrors(downloadResult);
				}
		  }

		  public IEnumerable<Download> FindTorrentsToDownload(Feed feed)
		  {
				return Repository.Filter.FindTorrentsMatchingFilters(feed);
		  }
	 }
}