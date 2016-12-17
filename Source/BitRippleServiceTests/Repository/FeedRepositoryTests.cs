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
				Feeds = Repository.Feed.GetFeeds();
			}
		}
	}
}