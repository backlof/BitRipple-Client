using Models;
using System.Collections.Generic;

namespace Repository
{
    public interface IRssReader
    {
        List<Torrent> FetchFeed(string url);
    }
}