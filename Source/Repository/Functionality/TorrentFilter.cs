using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class TorrentFilter
    {
        public static bool IsMatch(Filter filter, Torrent torrent)
        {
            if (!filter.Enabled)
                return false;
            else if (filter.HasDownloadedBefore(torrent))
                return false;
            else if (!filter.DoesTitleMatch(torrent))
                return false;
            else if (filter.IsTvShow() && (!torrent.IsTvShow() || !filter.ShouldDownloadTvShow(torrent)))
                return false;
            else if (filter.IsMiniSeries() && (!torrent.IsMiniSeries() || !filter.ShouldDownloadMiniSeries(torrent)))
                return false;
            else if (filter.IsSeasonPack() && (!torrent.IsSeasonPack() || !filter.ShouldDownloadSeasonPack(torrent)))
                return false;
            else
                return true;
        }
    }
}
