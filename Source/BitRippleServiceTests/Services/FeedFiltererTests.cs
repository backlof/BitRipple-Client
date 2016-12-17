using BitRippleService.Model;
using BitRippleService.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace BitRippleServiceTests.Service
{
	[TestClass]
	public class FeedFiltererTests
	{
		public FeedFilterer FeedFilterer => new FeedFilterer();
		public Torrent Torrent { get; set; }
		public Filter Filter { get; set; }
		public Feed Feed { get; set; }
		public List<Download> Result { get; set; }

		#region FindMatchingTorrents

		[TestMethod]
		public void ShouldDownloadCorrectItems()
		{
			Feed = new Feed
			{
				Id = 1,
				Filters = new List<Filter>
				{
					new Filter
					{
						Id = 1,
						FeedId =1 ,
						Disabled = false,
						TitleMatch = "The.Whole.Truth",
						Include =  "1080p",
						Exclude = "720p;Remux;AVC;H.264",
						OnlyMatchOnce = false,
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

			Result = FeedFilterer.FindMatchingTorrents(Feed).ToList();

			Assert.AreEqual(1, Result.Count);
			Assert.IsTrue(Result.Any(x => x.GUID == "6"));
		}

		[TestMethod]
		public void ShouldNotDownloadTheSameEpisodeTwiceDuringSameFilter()
		{
			Feed = new Feed
			{
				Id = 1,
				Filters = new List<Filter>
				{
					new Filter
				{
						Id = 1,
						FeedId =1 ,
						Disabled = false,
						TitleMatch = "Planet.Earth 2",
						Include = "",
						Exclude = "",
						Episode = 2,
						OnlyMatchOnce = true,
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

			Result = FeedFilterer.FindMatchingTorrents(Feed).ToList();

			Assert.AreEqual(1, Result.Count);
			Assert.IsTrue(Result.Any(x => x.GUID == "1"));
		}

		[TestMethod]
		public void ShouldBeAbleToDownloadEverythingInFeed()
		{
			Feed = new Feed
			{
				Id = 1,
				Filters = new List<Filter>
				{
					new Filter
					{
						Id = 1,
						FeedId =1 ,
						Disabled = false,
						TitleMatch = "*",
						Include = "",
						Exclude = "",
						OnlyMatchOnce = false,
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

			Result = FeedFilterer.FindMatchingTorrents(Feed).ToList();

			Assert.AreEqual(4, Result.Count);
		}

		#endregion

		#region General

		[TestMethod]
		public void ShouldDownloadEverythingInFilter()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "*",
				Exclude = "",
				Include = "",
				Disabled = false,
				Downloads = new List<Download> { }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "The Whole Truth 2016 1080p WEB-DL"
			};

			Assert.IsTrue(FeedFilterer.IsMatch(Filter, Torrent));
		}

		[TestMethod]
		public void ShouldNotDownloadExcluded()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "*",
				Exclude = "1080p",
				Include = "",
				Disabled = false,
				Downloads = new List<Download> { }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "The Whole Truth 2016 1080p WEB-DL"
			};

			Assert.IsFalse(FeedFilterer.IsMatch(Filter, Torrent));
		}

		[TestMethod]
		public void ShouldNotDownloadMissingIncluded()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "*",
				Exclude = "",
				Include = "720p",
				Disabled = false,
				Downloads = new List<Download> { }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "The Whole Truth 2016 1080p WEB-DL"
			};

			Assert.IsFalse(FeedFilterer.IsMatch(Filter, Torrent));
		}

		[TestMethod]
		public void ShouldBeAbleToMatchTitle()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "The.Whole.Truth.2016",
				Exclude = "",
				Include = "",
				Disabled = false,
				Downloads = new List<Download> { }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "The Whole Truth 2016 1080p WEB-DL"
			};

			Assert.IsTrue(FeedFilterer.IsMatch(Filter, Torrent));
		}

		[TestMethod]
		public void ShouldBeAbleToMatchInTheMiddleOfTitle()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "*Whole.Truth.2016",
				Exclude = "",
				Include = "",
				Disabled = false,
				Downloads = new List<Download> { }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "The Whole Truth 2016 1080p WEB-DL"
			};

			Assert.IsTrue(FeedFilterer.IsMatch(Filter, Torrent));
		}

		[TestMethod]
		public void ShouldBeAbleToSkipCharacters()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "*The*Truth",
				Exclude = "",
				Include = "",
				Disabled = false,
				Downloads = new List<Download> { }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "The Whole Truth 2016 1080p WEB-DL"
			};

			Assert.IsTrue(FeedFilterer.IsMatch(Filter, Torrent));
		}

		[TestMethod]
		public void ShouldNotDownloadWhenDisabled()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "*The.Whole.Truth",
				Exclude = "",
				Include = "",
				Disabled = true,
				Downloads = new List<Download> { }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "The Whole Truth 2016 1080p WEB-DL"
			};

			Assert.IsFalse(FeedFilterer.IsMatch(Filter, Torrent));
		}

		[TestMethod]
		public void ShouldNotDownloadTheSameTorrentTwice()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "*The.Whole.Truth",
				Exclude = "",
				Include = "",
				Disabled = false,
				Downloads = new List<Download> { new Download { GUID = "123", Name = "The Whole Truth 2016 1080p WEB-DL" }
					 }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "The Whole Truth 2016 1080p WEB-DL"
			};

			Assert.IsFalse(FeedFilterer.IsMatch(Filter, Torrent));
		}

		[TestMethod]
		public void ShouldNotDownloadTitlesNotMatching()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "Acantilado",
				Exclude = "",
				Include = "",
				Disabled = false,
				Downloads = new List<Download> { }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "The Whole Truth 2016 1080p WEB-DL"
			};

			Assert.IsFalse(FeedFilterer.IsMatch(Filter, Torrent));
		}

		[TestMethod]
		public void ShouldIgnoreCaps()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "the.whole.truth",
				Exclude = "",
				Include = "1080P",
				Disabled = false,
				Downloads = new List<Download> { }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "The Whole Truth 2016 1080p WEB-DL"
			};

			Assert.IsTrue(FeedFilterer.IsMatch(Filter, Torrent));
		}

		[TestMethod]
		public void ShouldFailWhenCapsAreWrong()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "the.whole.truth",
				Exclude = "1080P",
				Include = "",
				Disabled = false,
				Downloads = new List<Download> { }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "The Whole Truth 2016 1080p WEB-DL"
			};

			Assert.IsFalse(FeedFilterer.IsMatch(Filter, Torrent));
		}

		#endregion General

		#region Mini Series

		[TestMethod]
		public void ShouldMatchMiniSeries()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "Planet.Earth.2",
				Exclude = "720p",
				Include = "1080p",
				Episode = 2,
				OnlyMatchOnce = true,
				Disabled = false,
				Downloads = new List<Download>
				{
				}
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "Planet Earth 2 E02 1080P WEB-DL"
			};

			Assert.IsTrue(FeedFilterer.IsMatch(Filter, Torrent));
		}

		[TestMethod]
		public void ShouldNotDownloadMiniSeriesEpisodeTwice()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "Planet.Earth.2",
				Exclude = "720p",
				Include = "1080p",
				Episode = 2,
				OnlyMatchOnce = true,
				Disabled = false,
				Downloads = new List<Download>
					 {
						  new Download
						  {
								GUID = "321",
								Name = "Planet Earth 2 E02 1080P WEB-DL"
						  }
					 }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "Planet Earth 2 E02 1080P WEB-DL"
			};

			Assert.IsFalse(FeedFilterer.IsMatch(Filter, Torrent));
		}

		[TestMethod]
		public void ShouldDownloadMiniSeriesTwice()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "Planet.Earth.2",
				Exclude = "720p",
				Include = "1080p",
				Episode = 2,
				OnlyMatchOnce = false,
				Disabled = false,
				Downloads = new List<Download>
					 {
						  new Download
						  {
								GUID = "321",
								Name = "Planet Earth 2 E02 1080P WEB-DL"
						  }
					 }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "Planet Earth 2 E02 1080P WEB-DL"
			};

			Assert.IsTrue(FeedFilterer.IsMatch(Filter, Torrent));
		}

		[TestMethod]
		public void ShouldNotDownloadPreviousMiniSeries()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "Planet.Earth.2",
				Exclude = "720p",
				Include = "1080p",
				Episode = 3,
				OnlyMatchOnce = false,
				Disabled = false,
				Downloads = new List<Download>
					 {
						  new Download
						  {
								GUID = "321",
								Name = "Planet Earth 2 E02 1080P WEB-DL"
						  }
					 }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "Planet Earth 2 E02 1080P WEB-DL"
			};

			Assert.IsFalse(FeedFilterer.IsMatch(Filter, Torrent));
		}

		#endregion Mini Series

		#region Tv Show

		[TestMethod]
		public void ShouldDownloadTvShow()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "Planet.Earth",
				Exclude = "720p",
				Include = "1080p",
				Season = 2,
				Episode = 2,
				OnlyMatchOnce = true,
				Disabled = false,
				Downloads = new List<Download>
				{
				}
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "Planet Earth S02E02 1080P WEB-DL"
			};

			Assert.IsTrue(FeedFilterer.IsMatch(Filter, Torrent));
		}

		[TestMethod]
		public void ShouldNotDownloadTheSameTvShowEpisodeTwice()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "Planet.Earth",
				Exclude = "720p",
				Include = "1080p",
				Season = 2,
				Episode = 2,
				OnlyMatchOnce = true,
				Disabled = false,
				Downloads = new List<Download>
					 {
						  new Download
						  {
								GUID = "321",
								Name = "Planet Earth S02E02 1080P WEB-DL"
						  }
					 }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "Planet Earth S02E02 1080P WEB-DL"
			};

			Assert.IsFalse(FeedFilterer.IsMatch(Filter, Torrent));
		}

		[TestMethod]
		public void ShouldDownloadTvShowTwice()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "Planet.Earth",
				Exclude = "720p",
				Include = "1080p",
				Season = 2,
				Episode = 2,
				OnlyMatchOnce = false,
				Disabled = false,
				Downloads = new List<Download>
					 {
						  new Download
						  {
								GUID = "321",
								Name = "Planet Earth S02E02 1080P WEB-DL"
						  }
					 }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "Planet Earth S02E02 1080P WEB-DL"
			};

			Assert.IsTrue(FeedFilterer.IsMatch(Filter, Torrent));
		}

		[TestMethod]
		public void ShouldNotDownloadPreviousTvShow()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "Planet.Earth",
				Exclude = "720p",
				Include = "1080p",
				Season = 2,
				Episode = 3,
				OnlyMatchOnce = true,
				Disabled = false,
				Downloads = new List<Download>
					 {
						  new Download
						  {
								GUID = "321",
								Name = "Planet Earth S02E02 1080P WEB-DL"
						  }
					 }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "Planet Earth S02E02 1080P WEB-DL"
			};

			Assert.IsFalse(FeedFilterer.IsMatch(Filter, Torrent));
		}

		#endregion Tv Show

		#region Season pack

		[TestMethod]
		public void ShouldDownloadSeasonPack()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "Planet.Earth",
				Exclude = "720p",
				Include = "1080p",
				Disabled = false,
				Season = 2,
				OnlyMatchOnce = true,
				Downloads = new List<Download>
					 {
						  new Download
						  {
								GUID = "321",
								Name = "Planet Earth S01 1080P WEB-DL"
						  }
					 }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "Planet Earth S02 1080P WEB-DL"
			};

			Assert.IsTrue(FeedFilterer.IsMatch(Filter, Torrent));
		}

		[TestMethod]
		public void ShouldNotDownloadTheSameSeasonPackTwice()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "Planet.Earth",
				Exclude = "720p",
				Include = "1080p",
				Disabled = false,
				Season = 2,
				OnlyMatchOnce = true,
				Downloads = new List<Download>
					 {
						  new Download
						  {
								GUID = "321",
								Name = "Planet Earth S02 1080P WEB-DL"
						  }
					 }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "Planet Earth S02 1080P WEB-DL"
			};

			Assert.IsFalse(FeedFilterer.IsMatch(Filter, Torrent));
		}

		[TestMethod]
		public void ShouldDownloadSeasonPackTwice()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "Planet.Earth",
				Exclude = "720p",
				Include = "1080p",
				Disabled = false,
				Season = 2,
				OnlyMatchOnce = false,
				Downloads = new List<Download>
					 {
						  new Download
						  {
								GUID = "321",
								Name = "Planet Earth S02 1080P WEB-DL"
						  }
					 }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "Planet Earth S02 1080P WEB-DL"
			};

			Assert.IsTrue(FeedFilterer.IsMatch(Filter, Torrent));
		}

		[TestMethod]
		public void ShouldNotDownloadPreviousSeasonPack()
		{
			Feed = new Feed
			{
				Id = 1
			};

			Filter = new Filter
			{
				TitleMatch = "Planet.Earth",
				Exclude = "720p",
				Include = "1080p",
				Disabled = false,
				Season = 3,
				OnlyMatchOnce = false,
				Downloads = new List<Download>
					 {
						  new Download
						  {
								GUID = "321",
								Name = "Planet Earth S01 1080P WEB-DL"
						  }
					 }
			};

			Torrent = new Torrent
			{
				GUID = "123",
				Name = "Planet Earth S02 1080P WEB-DL"
			};

			Assert.IsFalse(FeedFilterer.IsMatch(Filter, Torrent));
		}

		#endregion Season pack
	}
}