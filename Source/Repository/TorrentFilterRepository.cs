using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Repository
{
    public class TorrentFilterRepository
    {
        public readonly Context _context;

        public TorrentFilterRepository(Context context)
        {
            _context = context;
        }

        public IEnumerable<Download> FindTorrentsMatchingFilters(Feed feed)
        {
            foreach (var filter in feed.Filters)
            {
                foreach (var torrent in feed.Torrents)
                {
                    if (TorrentFilter.IsMatch(filter, torrent))
                    {
                        Download download = new Download
                        {
                            FeedId = feed.Id,
                            FilterId = filter.Id,
                            Id = _context.Data.Settings.NextIndex.Download,
                            GUID = torrent.GUID,
                            Name = torrent.Name,
                            FileUrl = torrent.FileUrl,
                            TimeOfUpload = torrent.TimeOfUpload,
                            TimeOfDownload = DateTime.Now
                        };

                        _context.Data.Settings.NextIndex.Download++;

                        filter.Downloads.Add(download); // Will add again outside, but to make sure the logic is correct

                        yield return download;
                    }
                }
            }
        }
    }
}
