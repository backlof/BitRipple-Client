using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Repository;

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
					 RunWithCancellation(new CancellationTokenSource());
				}
				else
				{
					 WriteErrors(loadResult);
					 WriteError("Application did not start");
					 Console.Write("Press enter to exit: ");
					 Console.Read();
				}
		  }

		  private void RunWithCancellation(CancellationTokenSource tks)
		  {
				RunOnInterval(TimeSpan.FromMinutes(Repository.Data.Settings.Interval), tks.Token);
				Console.Read();
				tks.Cancel();
		  }

		  private async void RunOnInterval(TimeSpan delay, CancellationToken token)
		  {
				try
				{
					 do
					 {
						  await GoThroughFeeds();
						  await Task.Delay(delay, token);

					 } while (true);
				}
				catch (TaskCanceledException)
				{
					 Environment.Exit(0);
				}

		  }

		  private async Task GoThroughFeeds()
		  {
				Log("Updating...");

				var updatedFeeds = await WhenAll(FetchRss, Repository.Data.Feeds);

				var torrentsToDownload = await WhenAll(FindTorrentsToDownload, updatedFeeds);

				await WhenAll(DownloadTorrent, torrentsToDownload.SelectMany(x => x));
		  }

		  private async Task<IEnumerable<TOut>> WhenAll<TIn, TOut>(Func<TIn, TOut> func, IEnumerable<TIn> items)
		  {
				return await Task.WhenAll(items.Select(item => Task.Run(() => func(item))));
		  }

		  private async Task WhenAll<T>(Action<T> action, IEnumerable<T> items)
		  {
				await Task.WhenAll(items.Select(item => Task.Run(() => action(item))));
		  }

		  public Feed FetchRss(Feed feed)
		  {
				var feedUpdateResult = Repository.Downloader.DownloadFeed(feed);
				if (feedUpdateResult.Success)
				{
					 feed.Torrents = feedUpdateResult.Value;
					 return feed;
				}
				else
				{
					 WriteErrors(feedUpdateResult);
					 return feed;
				}
		  }

		  public void DownloadTorrent(Download torrent)
		  {
				Log($"Downloading: <{torrent.Name}>");

				var downloadResult = Repository.Downloader.DownloadTorrent(torrent);
				if (!downloadResult.Success)
				{
					 WriteErrors(downloadResult);
				}
		  }

		  public IEnumerable<Download> FindTorrentsToDownload(Feed feed)
		  {
				return Repository.Filter.FindTorrentsMatchingFilters(feed);
		  }

		  private void Log(string description)
		  {
				InternalLog($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] {description}");
		  }

		  private void InternalLog(string text)
		  {
				Console.WriteLine(text);
		  }

		  private void WriteErrors(IRepositoryResult result)
		  {
				foreach (var error in result.Errors)
				{
					 WriteError(error);
				}
		  }

		  private void WriteError(string error)
		  {
				Log($"Error <{error}>");
		  }
	 }
}