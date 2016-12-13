using Models;
using System.Collections.Generic;
using System.Net;
using System.Xml;

namespace Repository
{
	 public class XmlRssReader : IRssReader
	 {
		  public List<Torrent> FetchFeed(string url)
		  {
				return XmlRssParser.Parse(Load(url));
		  }

		  private XmlDocument Load(string url, XmlDocument xmlDocument = null)
		  {
				using (var response = GetRequest(url).GetResponse())
				{
					 xmlDocument = new XmlDocument();
					 xmlDocument.Load(response.GetResponseStream());
					 return xmlDocument;
				}
		  }

		  private WebRequest GetRequest(string url, WebRequest wr = null)
		  {
				wr = WebRequest.Create(url);
				wr.Timeout = 10000;
				return wr;
		  }
	 }
}