using BitRippleService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRippleService.Service
{
	 public class BitRippleServices
	 {
		  public IRssReader RssReader { get; set; }
		  public ISettingsService Settings { get; set; }
		  public ITorrentDownloader TorrentDownloader { get; set; }
		  public IFilterFeed Filter { get; set; }

		  public BitRippleServices() { }

		  public BitRippleServices(IRssReader reader, ISettingsService settings, ITorrentDownloader td, IFilterFeed filter)
		  {
				RssReader = reader;
				Settings = settings;
				TorrentDownloader = td;
				Filter = filter;
		  }
	 }
}
