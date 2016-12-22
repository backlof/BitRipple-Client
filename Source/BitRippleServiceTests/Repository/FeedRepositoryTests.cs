using BitRippleService.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BitRippleServiceTests.Repository
{
	[TestClass]
	public class FeedRepositoryTests
	{
		public DisposableBitRippleRepository Repository { get; set; }
		public ICollection<Feed> Feeds { get; set; }

		[TestMethod]
		public void ShouldBeAbleToAddDownload()
		{
			using (Repository = Container.GetRepository())
			{
				Repository.Feed.AddDownload(new Download
				{
					Name = "Test",
					TimeOfDownload = DateTime.Now,
					TimeOfUpload = DateTime.Now,
					GUID = "123",
					FileUrl = "x"
				});
			}
		}

		[TestMethod]
		public void ShouldBeAbleToGetFeeds()
		{
			using (Repository = Container.GetRepository())
			{
				Repository.Feed.AddFeed(new Feed
				{
					Name = "Movies",
					Url = @"http://www.mininova.org/rss.xml?cat=4"
				});

				Repository.Feed.AddFilter(new Filter
				{
					Name = "The Ivory Game",
					TitleMatch = "The.Ivory.Game",
					Disabled = true,
					Include = "1080p",
					Exclude = "",
					FeedId = 1
				});

				Repository.Feed.AddDownload(new Download
				{
					Name = "Planet Earth 2 E02 HDTV 1080p H264",
					TimeOfDownload = new DateTime(2016, 11, 12, 16, 2, 32),
					TimeOfUpload = new DateTime(2016, 11, 12, 16, 7, 12),
					GUID = "HIOAWR",
					FileUrl = "x",
					FeedId = 1,
					FilterId = 1
				});

				Feeds = Repository.Feed.GetFeeds();

				Assert.AreEqual(1, Feeds.Count);
				foreach (var feed in Feeds)
				{
					Assert.AreEqual(1, feed.Filters.Count);
					foreach (var filter in feed.Filters)
					{
						Assert.AreEqual(1, filter.Downloads.Count);
					}
				}
			}
		}
	}
}