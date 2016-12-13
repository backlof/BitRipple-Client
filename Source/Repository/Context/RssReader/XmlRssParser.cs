using Models;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Repository
{
	 public class XmlRssParser
	 {
		  public static List<Torrent> Parse(XmlDocument xmlDocument)
		  {
				XmlNodeList itemNodes = xmlDocument.SelectNodes("rss/channel/item");

				var torrents = new List<Torrent>();

				foreach (XmlNode itemNode in itemNodes)
				{
					 Torrent torrent = new Torrent();
					 torrent.Name = itemNode.SelectSingleNode("title").InnerText.Trim();
					 torrent.GUID = itemNode.SelectSingleNode("guid").InnerText.Trim();
					 torrent.TimeOfUpload = DateTime.Parse(itemNode.SelectSingleNode("pubDate").InnerText.Trim());
					 torrent.FileUrl = itemNode.SelectSingleNode("enclosure") != null
						  ? itemNode.SelectSingleNode("enclosure").Attributes["url"].InnerText.Trim()
						  : itemNode.SelectSingleNode("link").InnerText.Trim();

					 torrents.Add(torrent);
				}

				return torrents;
		  }
	 }
}