using BitRippleService.Service;
using BitRippleShared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BitRippleServiceTests.Service
{
	[TestClass]
	public class WebTorrentDownloaderTests
	{
		public WebTorrentDownloader WebTorrentDownloader => new WebTorrentDownloader();

		[TestMethod]
		public void ShouldBeAbleToDownloadTorrents()
		{
			WebTorrentDownloader.Download(@"http://www.mininova.org/tor/13363385", Constants.Location, "test");
		}
	}
}