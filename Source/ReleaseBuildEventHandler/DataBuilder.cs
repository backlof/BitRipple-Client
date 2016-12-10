using Models;
using Repository;
using System.Collections.Generic;
using System.IO;

namespace ReleaseBuildEventHandler
{
	 public class DataBuilder
	 {
		  private readonly List<Filter> _filters;
		  private readonly List<Feed> _feeds;
		  private readonly List<Download> _downloads;
		  private readonly Settings _settings;

		  public DataBuilder()
		  {
				_filters = new List<Filter>();
				_feeds = new List<Feed>();
				_downloads = new List<Download>();
				_settings = new Settings
				{
					 Interval = 5,
					 Location = "",
					 NextIndex = new IndexStorage
					 {
						  Download = 10001,
						  Feed = 10001,
						  Filter = 10001
					 },
					 FeedDownloadCount = 0,
					 TorrentDownloadCount = 0
				};
		  }

		  public DataBuilder WithFeed(Feed feed)
		  {
				feed.Id = _settings.NextIndex.Feed;
				_feeds.Add(feed);
				_settings.NextIndex.Feed++;

				return this;
		  }

		  public DataBuilder WithFeed(Feed feed, params Filter[] filters)
		  {
				feed.Id = _settings.NextIndex.Feed;
				_feeds.Add(feed);
				_settings.NextIndex.Feed++;

				foreach (var filter in filters)
				{
					 filter.Id = _settings.NextIndex.Filter;
					 filter.FeedId = feed.Id;
					 _filters.Add(filter);
					 _settings.NextIndex.Filter++;
				}

				return this;
		  }

		  public DataBuilder WithLocation(string location)
		  {
				_settings.Location = location;
				return this;
		  }

		  public void Build()
		  {
				if (!Directory.Exists(JsonDataStorage.Folder))
				{
					 Directory.CreateDirectory(JsonDataStorage.Folder);
				}

				JsonDataStorage.WriteFile(JsonDataStorage.FiltersPath, _filters);
				JsonDataStorage.WriteFile(JsonDataStorage.FeedsPath, _feeds);
				JsonDataStorage.WriteFile(JsonDataStorage.DownloadsPath, _downloads);
				JsonDataStorage.WriteFile(JsonDataStorage.SettingsPath, _settings);
		  }
	 }
}