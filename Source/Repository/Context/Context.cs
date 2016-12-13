namespace Repository
{
	 public class Context
	 {
		  public IDataStorage Data { get; set; }
		  public IRssReader RssReader { get; set; }
		  public ITorrentDownloader TorrentDownloader { get; set; }

		  public Context(IDataStorage dt, IRssReader rs, ITorrentDownloader to)
		  {
				Data = dt;
				RssReader = rs;
				TorrentDownloader = to;
		  }

		  public Context()
		  {
		  }
	 }
}