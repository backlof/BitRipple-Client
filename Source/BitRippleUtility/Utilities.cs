namespace BitRippleUtility
{
	public class Utilities
	{
		public IRssReader RssReader { get; set; }
		public ISettingsService Settings { get; set; }
		public ITorrentDownloader TorrentDownloader { get; set; }
		public IFilterFeed Filter { get; set; }

		public Utilities()
		{
		}

		public Utilities(IRssReader reader, ISettingsService settings, ITorrentDownloader td, IFilterFeed filter)
		{
			RssReader = reader;
			Settings = settings;
			TorrentDownloader = td;
			Filter = filter;
		}
	}
}