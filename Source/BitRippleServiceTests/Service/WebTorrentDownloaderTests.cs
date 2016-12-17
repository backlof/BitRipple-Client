using BitRippleService.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace BitRippleServiceTests.Service
{
	[TestClass]
	public class WebTorrentDownloaderTests
	{
		public WebTorrentDownloader WebTorrentDownloader => new WebTorrentDownloader();

		[TestMethod]
		public void ShouldBeAbleToDownloadTorrents()
		{
			WebTorrentDownloader.Download(@"http://www.mininova.org/tor/13363385", Directory.GetCurrentDirectory(), "test");
		}
	}
}