using BitRippleService.Model;

namespace BitRippleService.Service
{
	 public interface ITorrentDownloader
	 {
		  void Download(string url, string location, string filename);
	 }
}