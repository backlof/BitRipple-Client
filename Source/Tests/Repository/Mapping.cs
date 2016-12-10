using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using Rhino.Mocks;

namespace Tests
{
    [TestClass]
    public class Mapping
    {
        public BitRippleRepository Repository { get; set; }

        public List<Download> Downloads { get; set; }
        public List<Feed> Feeds { get; set; }
        public List<Filter> Filters { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Repository = Container.GetRepository();

            Filters = Repository.Context.Data.Filters = new List<Filter>();
            Feeds = Repository.Context.Data.Feeds = new List<Feed>();
            Downloads = Repository.Context.Data.Downloads = new List<Download>();
        }

        [TestMethod]
        public void ShouldMapCorrectly()
        {
            CreateFeed(0);
            CreateFeed(1);
            CreateFilter(0, 0);
            CreateFilter(0, 1);
            CreateFilter(1, 2);
            CreateFilter(1, 3);
            CreateDownload(0, 0, 0);
            CreateDownload(0, 0, 1);
            CreateDownload(0, 1, 2);
            CreateDownload(0, 1, 3);
            CreateDownload(1, 2, 4);
            CreateDownload(1, 2, 5);
            CreateDownload(1, 3, 6);
            CreateDownload(1, 3, 7);

            Assert.AreEqual(2, Repository.Data.Feeds.Count());

            foreach (var feed in Repository.Data.Feeds)
            {
                Assert.AreEqual(2, feed.Filters.Count);
                Assert.AreEqual(4, feed.Downloads.Count);

                foreach (var filter in feed.Filters)
                {
                    Assert.AreEqual(2, filter.Downloads.Count());
                }
            }

            Assert.AreEqual(4, Repository.Data.Filters.Count());

            foreach (var filter in Repository.Data.Filters)
            {
                Assert.AreEqual(2, filter.Downloads.Count);
            }

            Assert.AreEqual(8, Repository.Data.Downloads.Count());
        }

        [TestMethod]
        public void ShouldReturnEmptyListsWhenThereAreNoItems()
        {
            Assert.AreEqual(0, Repository.Data.Downloads.Count());
            Assert.AreEqual(0, Repository.Data.Feeds.Count());
            Assert.AreEqual(0, Repository.Data.Filters.Count());
        }

        private void CreateDownload(int feed, int filter, int download)
        {
            Downloads.Add(new Download
            {
                Id = download,
                FilterId = filter,
                FeedId = feed,
            });
        }

        private void CreateFeed(int feed)
        {
            Feeds.Add(new Feed
            {
                Id = feed
            });
        }

        private void CreateFilter(int feed, int filter)
        {
            Filters.Add(new Filter
            {
                Id = filter,
                FeedId = feed
            });
        }
    }
}