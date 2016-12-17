using BitRippleService.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace BitRippleServiceTests.Repository
{
	[TestClass]
	public class SqLiteDbContextTests
	{
		public DisposableSqLiteDbContext Context { get; set; }

		public Feed FeedToAdd { get; set; }
		public Feed FeedFromDb { get; set; }

		public Filter FilterToAdd { get; set; }
		public Filter FilterFromDb { get; set; }

		public Download DownloadToAdd { get; set; }
		public Download DownloadFromDb { get; set; }

		[TestMethod]
		public void ShouldBeAbleToAddEntities()
		{
			using (Context = Container.GetContext())
			{
				#region Insert feed

				FeedToAdd = new Feed
				{
					Name = "Movies",
					Url = @"http://www.mininova.org/rss.xml?cat=4"
				};

				Context.Feeds.Add(FeedToAdd);
				Context.SaveChanges();
				FeedFromDb = Context.Feeds.Single();

				#endregion Insert feed

				#region Insert filter

				FilterToAdd = new Filter
				{
					Name = "The Ivory Game",
					TitleMatch = "The.Ivory.Game",
					Disabled = true,
					Include = "1080p",
					Exclude = "",
					Feed = FeedFromDb
				};

				Context.Filters.Add(FilterToAdd);
				Context.SaveChanges();
				FilterFromDb = Context.Filters.Single();

				#endregion Insert filter

				#region Insert download

				DownloadToAdd = new Download
				{
					Name = "Planet Earth 2 E02 HDTV 1080p H264",
					TimeOfDownload = new DateTime(2016, 11, 12, 16, 2, 32),
					TimeOfUpload = new DateTime(2016, 11, 12, 16, 7, 12),
					GUID = "HIOAWR",
					FileUrl = "x",
					FeedId = FeedFromDb.Id,
					FilterId = FilterFromDb.Id
				};

				Context.Downloads.Add(DownloadToAdd);
				Context.SaveChanges();
				DownloadFromDb = Context.Downloads.Single();

				#endregion Insert download

				AssertFiltersAreEqual(FilterToAdd, FilterFromDb);
				AssertFeedsAreEqual(FeedToAdd, FeedFromDb);
				AssertDownloadsAreEqual(DownloadToAdd, DownloadFromDb);

				#region Foreign keys

				Assert.AreEqual(FeedFromDb.Id, FilterFromDb.FeedId);
				Assert.AreEqual(FeedFromDb.Id, DownloadFromDb.FeedId);
				Assert.AreEqual(FilterFromDb.Id, DownloadFromDb.FilterId);

				#endregion Foreign keys
			}
		}

		private void AssertDownloadsAreEqual(Download expected, Download actual)
		{
			Assert.AreEqual(expected.GUID, actual.GUID);
			Assert.AreEqual(expected.Name, actual.Name);
			Assert.AreEqual(expected.TimeOfDownload, actual.TimeOfDownload);
			Assert.AreEqual(expected.TimeOfUpload, actual.TimeOfUpload);
			Assert.AreEqual(expected.FileUrl, actual.FileUrl);
		}

		private void AssertFeedsAreEqual(Feed expected, Feed actual)
		{
			Assert.AreEqual(expected.Name, actual.Name);
			Assert.AreEqual(expected.Url, actual.Url);
		}

		private void AssertFiltersAreEqual(Filter expected, Filter actual)
		{
			Assert.AreEqual(expected.Disabled, actual.Disabled);
			Assert.AreEqual(expected.Exclude, actual.Exclude);
			Assert.AreEqual(expected.Include, actual.Include);
			Assert.AreEqual(expected.Name, actual.Name);
			Assert.AreEqual(expected.OnlyMatchOnce, actual.OnlyMatchOnce);
			Assert.AreEqual(expected.Season, actual.Season);
			Assert.AreEqual(expected.TitleMatch, actual.TitleMatch);
		}
	}
}