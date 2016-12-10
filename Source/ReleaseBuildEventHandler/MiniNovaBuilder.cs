using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseBuildEventHandler
{
	 public class MiniNovaBuilder : IDataWriter
	 {
		  private readonly DataBuilder _dataBuilder;

		  public MiniNovaBuilder()
		  {
				_dataBuilder = new DataBuilder();
		  }

		  public void BuildDefaults()
		  {
				_dataBuilder.WithLocation(Directory.GetCurrentDirectory());

				_dataBuilder.WithFeed(new Feed() { Name = "Movies", Url = @"http://www.mininova.org/rss.xml?cat=4", }, new Filter() { Name = "The Ivory Game", Criterias = "The.Ivory.Game", Enabled = true, Include = new[] { "1080p" }.ToList(), Exclude = new List<string>(), FeedId = 10001, IgnoreCaps = true });
				_dataBuilder.WithFeed(new Feed() { Name = "Movies - Action", Url = @"http://www.mininova.org/rss.xml?sub=1" });
				_dataBuilder.WithFeed(new Feed() { Name = "Games", Url = @"http://www.mininova.org/rss.xml?cat=3" });
				_dataBuilder.WithFeed(new Feed() { Name = "Books", Url = @"http://www.mininova.org/rss.xml?cat=2" });
				_dataBuilder.WithFeed(new Feed() { Name = "Music", Url = @"http://www.mininova.org/rss.xml?cat=5" });
				_dataBuilder.WithFeed(new Feed() { Name = "Tv", Url = @"http://www.mininova.org/rss.xml?cat=8" }, new Filter { Name = "Family Guy", Criterias = "Family.Guy", Enabled = true, Include = new[] { "1080p" }.ToList(), Exclude = new[] { "SD", "480p", "720p" }.ToList(), FeedId = 10006, TvShow = new EpisodeCriteria { Season = 15, Episode = 7, OnlyDownloadEpisodeOnce = true }, IgnoreCaps = true }, new Filter() { Name = "Planet Earth 2", Criterias = "Planet.Earth.2", Enabled = true, Include = new[] { "1080p" }.ToList(), Exclude = new List<string>(), MiniSeries = new MiniEpisodeCriteria { Episode = 1, OnlyDownloadEpisodeOnce = true }, IgnoreCaps = true });

				_dataBuilder.Build();
		  }
	 }
}
