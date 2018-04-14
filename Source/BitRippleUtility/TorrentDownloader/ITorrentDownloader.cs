namespace BitRippleUtility
{
	public interface ITorrentDownloader
	{
		void Download(string url, string location, string filename);
	}
}