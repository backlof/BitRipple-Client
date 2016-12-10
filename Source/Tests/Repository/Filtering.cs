using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Mocks;
using Models;

namespace Tests
{
	 [TestClass]
	 public class Filtering
	 {
		  public BitRippleRepository Repository { get; set; }
		  public Torrent Torrent { get; set; }
		  public Filter Filter { get; set; }
		  public Feed Feed { get; set; }

		  [TestInitialize]
		  public void Initialize()
		  {
				Repository = Container.GetRepository();
		  }

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
					 Criterias = "*",
					 Exclude = new List<string>() { },
					 Include = new List<string>() { },
					 IgnoreCaps = true,
					 Enabled = true,
					 Downloads = new List<Download> { }
				};

				Torrent = new Torrent
				{
					 GUID = "123",
					 Name = "The Whole Truth 2016 1080p WEB-DL"
				};

				Assert.IsTrue(TorrentFilter.IsMatch(Filter, Torrent));
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
					 Criterias = "*",
					 Exclude = new List<string>() { "1080p" },
					 Include = new List<string>() { },
					 IgnoreCaps = true,
					 Enabled = true,
					 Downloads = new List<Download> { }
				};

				Torrent = new Torrent
				{
					 GUID = "123",
					 Name = "The Whole Truth 2016 1080p WEB-DL"
				};

				Assert.IsFalse(TorrentFilter.IsMatch(Filter, Torrent));
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
					 Criterias = "*",
					 Exclude = new List<string>() { },
					 Include = new List<string>() { "720p" },
					 IgnoreCaps = true,
					 Enabled = true,
					 Downloads = new List<Download> { }
				};

				Torrent = new Torrent
				{
					 GUID = "123",
					 Name = "The Whole Truth 2016 1080p WEB-DL"
				};

				Assert.IsFalse(TorrentFilter.IsMatch(Filter, Torrent));
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
					 Criterias = "The.Whole.Truth.2016",
					 Exclude = new List<string>() { },
					 Include = new List<string>() { },
					 IgnoreCaps = true,
					 Enabled = true,
					 Downloads = new List<Download> { }
				};

				Torrent = new Torrent
				{
					 GUID = "123",
					 Name = "The Whole Truth 2016 1080p WEB-DL"
				};

				Assert.IsTrue(TorrentFilter.IsMatch(Filter, Torrent));
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
					 Criterias = "*Whole.Truth.2016",
					 Exclude = new List<string>() { },
					 Include = new List<string>() { },
					 IgnoreCaps = true,
					 Enabled = true,
					 Downloads = new List<Download> { }
				};

				Torrent = new Torrent
				{
					 GUID = "123",
					 Name = "The Whole Truth 2016 1080p WEB-DL"
				};

				Assert.IsTrue(TorrentFilter.IsMatch(Filter, Torrent));
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
					 Criterias = "*The*Truth",
					 Exclude = new List<string>() { },
					 Include = new List<string>() { },
					 IgnoreCaps = true,
					 Enabled = true,
					 Downloads = new List<Download> { }
				};

				Torrent = new Torrent
				{
					 GUID = "123",
					 Name = "The Whole Truth 2016 1080p WEB-DL"
				};

				Assert.IsTrue(TorrentFilter.IsMatch(Filter, Torrent));
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
					 Criterias = "*The.Whole.Truth",
					 Exclude = new List<string>() { },
					 Include = new List<string>() { },
					 IgnoreCaps = true,
					 Enabled = false,
					 Downloads = new List<Download> { }
				};

				Torrent = new Torrent
				{
					 GUID = "123",
					 Name = "The Whole Truth 2016 1080p WEB-DL"
				};

				Assert.IsFalse(TorrentFilter.IsMatch(Filter, Torrent));
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
					 Criterias = "*The.Whole.Truth",
					 Exclude = new List<string>() { },
					 Include = new List<string>() { },
					 IgnoreCaps = true,
					 Enabled = true,
					 Downloads = new List<Download> { new Download { GUID = "123", Name = "The Whole Truth 2016 1080p WEB-DL" }
					 }
				};

				Torrent = new Torrent
				{
					 GUID = "123",
					 Name = "The Whole Truth 2016 1080p WEB-DL"
				};

				Assert.IsFalse(TorrentFilter.IsMatch(Filter, Torrent));
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
					 Criterias = "Acantilado",
					 Exclude = new List<string>() { },
					 Include = new List<string>() { },
					 IgnoreCaps = true,
					 Enabled = true,
					 Downloads = new List<Download> { }
				};

				Torrent = new Torrent
				{
					 GUID = "123",
					 Name = "The Whole Truth 2016 1080p WEB-DL"
				};

				Assert.IsFalse(TorrentFilter.IsMatch(Filter, Torrent));
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
					 Criterias = "the.whole.truth",
					 Exclude = new List<string>() { },
					 Include = new List<string>() { "1080P" },
					 IgnoreCaps = true,
					 Enabled = true,
					 Downloads = new List<Download> { }
				};

				Torrent = new Torrent
				{
					 GUID = "123",
					 Name = "The Whole Truth 2016 1080p WEB-DL"
				};

				Assert.IsTrue(TorrentFilter.IsMatch(Filter, Torrent));
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
					 Criterias = "the.whole.truth",
					 Exclude = new List<string>() { "1080P" },
					 Include = new List<string>() { },
					 IgnoreCaps = false,
					 Enabled = true,
					 Downloads = new List<Download> { }
				};

				Torrent = new Torrent
				{
					 GUID = "123",
					 Name = "The Whole Truth 2016 1080p WEB-DL"
				};

				Assert.IsFalse(TorrentFilter.IsMatch(Filter, Torrent));
		  }

		  #endregion

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
					 Criterias = "Planet.Earth.2",
					 Exclude = new List<string>() { "720p" },
					 Include = new List<string>() { "1080p" },
					 IgnoreCaps = true,
					 MiniSeries = new MiniEpisodeCriteria { Episode = 2, OnlyDownloadEpisodeOnce = true },
					 Enabled = true,
					 Downloads = new List<Download>
					 {

					 }
				};

				Torrent = new Torrent
				{
					 GUID = "123",
					 Name = "Planet Earth 2 E02 1080P WEB-DL"
				};

				Assert.IsTrue(TorrentFilter.IsMatch(Filter, Torrent));
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
					 Criterias = "Planet.Earth.2",
					 Exclude = new List<string>() { "720p" },
					 Include = new List<string>() { "1080p" },
					 IgnoreCaps = true,
					 MiniSeries = new MiniEpisodeCriteria { Episode = 2, OnlyDownloadEpisodeOnce = true },
					 Enabled = true,
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

				Assert.IsFalse(TorrentFilter.IsMatch(Filter, Torrent));
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
					 Criterias = "Planet.Earth.2",
					 Exclude = new List<string>() { "720p" },
					 Include = new List<string>() { "1080p" },
					 IgnoreCaps = true,
					 MiniSeries = new MiniEpisodeCriteria { Episode = 2, OnlyDownloadEpisodeOnce = false },
					 Enabled = true,
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

				Assert.IsTrue(TorrentFilter.IsMatch(Filter, Torrent));
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
					 Criterias = "Planet.Earth.2",
					 Exclude = new List<string>() { "720p" },
					 Include = new List<string>() { "1080p" },
					 IgnoreCaps = true,
					 MiniSeries = new MiniEpisodeCriteria { Episode = 3, OnlyDownloadEpisodeOnce = false },
					 Enabled = true,
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

				Assert.IsFalse(TorrentFilter.IsMatch(Filter, Torrent));
		  }
		  #endregion

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
					 Criterias = "Planet.Earth",
					 Exclude = new List<string>() { "720p" },
					 Include = new List<string>() { "1080p" },
					 IgnoreCaps = true,
					 TvShow = new EpisodeCriteria { Season = 2, Episode = 2, OnlyDownloadEpisodeOnce = true },
					 Enabled = true,
					 Downloads = new List<Download>
					 {

					 }
				};

				Torrent = new Torrent
				{
					 GUID = "123",
					 Name = "Planet Earth S02E02 1080P WEB-DL"
				};

				Assert.IsTrue(TorrentFilter.IsMatch(Filter, Torrent));
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
					 Criterias = "Planet.Earth",
					 Exclude = new List<string>() { "720p" },
					 Include = new List<string>() { "1080p" },
					 IgnoreCaps = true,
					 TvShow = new EpisodeCriteria { Season = 2, Episode = 2, OnlyDownloadEpisodeOnce = true },
					 Enabled = true,
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

				Assert.IsFalse(TorrentFilter.IsMatch(Filter, Torrent));
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
					 Criterias = "Planet.Earth",
					 Exclude = new List<string>() { "720p" },
					 Include = new List<string>() { "1080p" },
					 IgnoreCaps = true,
					 TvShow = new EpisodeCriteria { Season = 2, Episode = 2, OnlyDownloadEpisodeOnce = false },
					 Enabled = true,
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

				Assert.IsTrue(TorrentFilter.IsMatch(Filter, Torrent));
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
					 Criterias = "Planet.Earth",
					 Exclude = new List<string>() { "720p" },
					 Include = new List<string>() { "1080p" },
					 IgnoreCaps = true,
					 TvShow = new EpisodeCriteria { Season = 2, Episode = 3, OnlyDownloadEpisodeOnce = true },
					 Enabled = true,
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

				Assert.IsFalse(TorrentFilter.IsMatch(Filter, Torrent));
		  }
		  #endregion

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
					 Criterias = "Planet.Earth",
					 Exclude = new List<string>() { "720p" },
					 Include = new List<string>() { "1080p" },
					 IgnoreCaps = true,
					 Enabled = true,
					 SeasonPack = new SeasonPackCriteria { Season = 2, OnlyDownloadSeasonOnce = true },
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

				Assert.IsTrue(TorrentFilter.IsMatch(Filter, Torrent));
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
					 Criterias = "Planet.Earth",
					 Exclude = new List<string>() { "720p" },
					 Include = new List<string>() { "1080p" },
					 IgnoreCaps = true,
					 Enabled = true,
					 SeasonPack = new SeasonPackCriteria { Season = 2, OnlyDownloadSeasonOnce = true },
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

				Assert.IsFalse(TorrentFilter.IsMatch(Filter, Torrent));
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
					 Criterias = "Planet.Earth",
					 Exclude = new List<string>() { "720p" },
					 Include = new List<string>() { "1080p" },
					 IgnoreCaps = true,
					 Enabled = true,
					 SeasonPack = new SeasonPackCriteria { Season = 2, OnlyDownloadSeasonOnce = false },
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

				Assert.IsTrue(TorrentFilter.IsMatch(Filter, Torrent));
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
					 Criterias = "Planet.Earth",
					 Exclude = new List<string>() { "720p" },
					 Include = new List<string>() { "1080p" },
					 IgnoreCaps = true,
					 Enabled = true,
					 SeasonPack = new SeasonPackCriteria { Season = 3, OnlyDownloadSeasonOnce = false },
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

				Assert.IsFalse(TorrentFilter.IsMatch(Filter, Torrent));
		  }
		  #endregion
	 }
}
