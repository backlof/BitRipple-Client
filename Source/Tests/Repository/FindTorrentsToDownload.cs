using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitRipple;
using Rhino.Mocks;

namespace Tests.Repository
{
	 [TestClass]
	 public class FindTorrentsToDownload
	 {
		  public ConsoleApplication Service { get; set; }

		  public Feed Feed { get; set; }
		  public List<Download> Result { get; set; }

		  [TestInitialize]
		  public void Initialize()
		  {
				Service = Container.GetConsoleService();
		  }

		  [TestMethod]
		  public void ShouldDownloadCorrectItems()
		  {
				Service.Repository.Context.Data.Settings = new Settings
				{
					 NextIndex = new IndexStorage
					 {
						  Download = 2
					 }
				};

				Feed = new Feed
				{
					 Id = 1,
					 Filters = new List<Filter>
					 {
						  new Filter
						  {
								Id = 1,
								FeedId =1 ,
								IgnoreCaps = true,
								Enabled = true,
								Criterias = "The.Whole.Truth",
								Include = new List<string> { "1080p" },
								Exclude = new List<string> { "720p", "Remux", "AVC", "H.264" },
								Downloads = new List<Download>
								{
									 new Download
									 {
										  Id = 1,
										  Name = "The Whole Truth 1080p BluRay x264",
										  GUID = "1"
									 }
								},
						  }
					 },
					 Torrents = new List<Torrent>
					 {
						  new Torrent
						  {
								Name = "The Whole Truth 1080p BluRay x264",
								GUID = "1"
						  },
						  new Torrent
						  {
								Name = "The Whole Truth 720 BluRay DTS x264",
								GUID = "2"
						  },
						  new Torrent
						  {
								Name = "The Whole Truth 1080p Remux AVC FLAC",
								GUID = "3"
						  },
						  new Torrent
						  {
								Name = "The Whole Truth 1080p WEB-DL H.264",
								GUID = "4"
						  },
						  new Torrent
						  {
								Name = "The Whole Truth 1080p NOR Blu-ray AVC DTS-HD",
								GUID = "5"
						  },
							new Torrent
						  {
								Name = "The Whole Truth 1080p BluRay x264 INTERNAL",
								GUID = "6"
						  },
					 }
				};

				Result = Service.FindTorrentsToDownload(Feed).ToList();

				Assert.AreEqual(1, Result.Count);
				Assert.IsTrue(Result.Any(x => x.GUID == "6"));
		  }

		  [TestMethod]
		  public void ShouldNotDownloadTheSameEpisodeTwiceDuringSameFilter()
		  {
				Service.Repository.Context.Data.Settings = new Settings
				{
					 NextIndex = new IndexStorage
					 {
						  Download = 2
					 }
				};

				Feed = new Feed
				{
					 Id = 1,
					 Filters = new List<Filter>
					 {
						  new Filter
						  {
								Id = 1,
								FeedId =1 ,
								IgnoreCaps = true,
								Enabled = true,
								Criterias = "Planet.Earth 2",
								Include = new List<string> {  },
								Exclude = new List<string> {  },
								MiniSeries = new MiniEpisodeCriteria { Episode = 2, OnlyDownloadEpisodeOnce = true },
								Downloads = new List<Download>{},
						  }
					 },
					 Torrents = new List<Torrent>
					 {
						  new Torrent
						  {
								Name = "Planet Earth 2 E02 1080p WEB-DEL H.264",
								GUID = "1"
						  },
						  new Torrent
						  {
								Name = "Planet Earth 2 E02 1080p WEB-DEL H.264",
								GUID = "2"
						  },
						  new Torrent
						  {
								Name = "Planet Earth 2 E02 1080p HDTV x64",
								GUID = "3"
						  },
						  new Torrent
						  {
								Name = "Planet Earth 2 E02 720p HDTV x64",
								GUID = "4"
						  }
					 }
				};

				Result = Service.FindTorrentsToDownload(Feed).ToList();

				Assert.AreEqual(1, Result.Count);
				Assert.IsTrue(Result.Any(x => x.GUID == "1"));
		  }

		  [TestMethod]
		  public void ShouldBeAbleToDownloadEverythingInFeed()
		  {
				Service.Repository.Context.Data.Settings = new Settings
				{
					 NextIndex = new IndexStorage
					 {
						  Download = 1
					 }
				};

				Feed = new Feed
				{
					 Id = 1,
					 Filters = new List<Filter>
					 {
						  new Filter
						  {
								Id = 1,
								FeedId =1 ,
								IgnoreCaps = true,
								Enabled = true,
								Criterias = "*",
								Include = new List<string> {  },
								Exclude = new List<string> {  },
								Downloads = new List<Download>{},
						  }
					 },
					 Torrents = new List<Torrent>
					 {
						  new Torrent
						  {
								Name = "Planet Earth 2 E02 1080p WEB-DEL H.264",
								GUID = "1"
						  },
						  new Torrent
						  {
								Name = "Planet Earth 2 E02 1080p WEB-DEL H.264",
								GUID = "2"
						  },
						  new Torrent
						  {
								Name = "Planet Earth 2 E02 1080p HDTV x64",
								GUID = "3"
						  },
						  new Torrent
						  {
								Name = "Planet Earth 2 E02 720p HDTV x64",
								GUID = "4"
						  }
					 }
				};

				Result = Service.FindTorrentsToDownload(Feed).ToList();

				Assert.AreEqual(4, Result.Count);
		  }
	 }
}
