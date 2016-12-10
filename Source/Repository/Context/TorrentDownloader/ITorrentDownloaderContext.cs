using Models;

namespace Repository
{
    public interface ITorrentDownloader
    {
        void Download(Download download, string location);
    }
}