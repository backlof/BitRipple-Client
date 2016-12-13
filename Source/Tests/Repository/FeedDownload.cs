using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Repository;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Tests
{
	 [TestClass]
	 public class FeedDownload
	 {
		  public BitRippleRepository Repository { get; set; }

		  public List<Torrent> Torrents { get; set; }
		  public RepositoryResult<List<Torrent>> Result { get; set; }

		  [TestInitialize]
		  public void Initialize()
		  {
				Repository = Container.GetRepository();
		  }

		  private XmlDocument GetXmlDocumentDummy()
		  {
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load("Files/Feed.xml");
				return xmlDocument;
		  }

		  public List<Torrent> Parse(Func<XmlDocument> func)
		  {
				return XmlRssParser.Parse(func());
		  }

		  [TestMethod]
		  public void ShouldBeAbleToParseXml()
		  {
				Torrents = Parse(GetXmlDocumentDummy);

				Assert.AreEqual(25, Torrents.Count);
		  }

		  [TestMethod]
		  public void ShouldThrowErrorWhenExceptionParsingFeed()
		  {
				Repository.Context.RssReader.Stub(x => x.FetchFeed(null)).Throw(new Exception());

				Result = Repository.Downloader.DownloadFeed(new Feed { });

				Assert.AreEqual(false, Result.Success);
				Assert.IsNull(Result.Value);
				Assert.AreEqual(1, Result.Errors.Count);
		  }
	 }
}