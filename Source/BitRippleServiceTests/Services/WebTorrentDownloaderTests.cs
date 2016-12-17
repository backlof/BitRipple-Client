using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitRippleService.Service;
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
