using BitRippleService.Model;
using BitRippleService.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Xml;

namespace BitRippleServiceTests.Service
{
	[TestClass]
	public class XmlRssReaderTest
	{
		public XmlRssReader XmlRssReader => new XmlRssReader();
		public XmlRssParser XmlRssParser => new XmlRssParser();
		public ICollection<Torrent> Torrents { get; set; }

		private XmlDocument GetXmlDocumentFromFile()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load("Files/Feed.xml");
			return xmlDocument;
		}

		[TestMethod]
		public void ShouldBeAbleToParseXml()
		{
			Torrents = XmlRssParser.Parse(GetXmlDocumentFromFile());

			Assert.AreEqual(25, Torrents.Count);
		}

		[TestMethod]
		public void ShouldBeAbleToFetchFeed()
		{
			Torrents = XmlRssReader.FetchFeed(@"http://www.mininova.org/rss.xml?cat=4");

			Assert.IsNotNull(Torrents);
			Assert.IsTrue(Torrents.Count > 0);
		}
	}
}