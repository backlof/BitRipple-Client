using BitRippleService.Repository;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BitRippleService.Model;
using System;

namespace BitRippleReleaseEvents.Defaults
{
    public class MiniNovaBuilder : IDataWriter
    {
        private readonly BitRippleContext _context;

        public MiniNovaBuilder(BitRippleContext context)
        {
            _context = context;
        }

        public void BuildDefaults()
        {
            _context.Feeds.Add(new Feed
            {
                Name = "Movies",
                Url = @"http://www.mininova.org/rss.xml?cat=4"
            });

            _context.Feeds.Add(new Feed
            {
                Name = "Movies - Action",
                Url = @"http://www.mininova.org/rss.xml?sub=1"
            });

            _context.Feeds.Add(new Feed
            {
                Name = "Games",
                Url = @"http://www.mininova.org/rss.xml?cat=3"
            });

            _context.Feeds.Add(new Feed
            {
                Name = "Books",
                Url = @"http://www.mininova.org/rss.xml?cat=2"
            });

            _context.Feeds.Add(new Feed
            {
                Name = "Music",
                Url = @"http://www.mininova.org/rss.xml?cat=5"
            });

            _context.Feeds.Add(new Feed
            {
                Name = "Tv",
                Url = @"http://www.mininova.org/rss.xml?cat=8"
            });

            _context.SaveChanges();

            var ids = _context.Feeds.Select(x => x.Id).ToArray();

            _context.Filters.Add(new Filter
            {
                Name = "The Ivory Game",
                TitleMatch = "The.Ivory.Game",
                Disabled = true,
                Include = "1080p",
                Exclude = "",
                FeedId = ids[0],
            });
            _context.Filters.Add(new Filter
            {
                Name = "Family Guy",
                TitleMatch = "Family.Guy",
                Disabled = true,
                Include = "1080p",
                Exclude = "SD;480p;720p",
                FeedId = ids[5],
                Season = 15,
                Episode = 7,
                OnlyMatchOnce = true,
            });

            _context.Filters.Add(new Filter
            {
                Name = "Planet Earth 2",
                TitleMatch = "Planet.Earth.2",
                Disabled = true,
                Include = "1080p",
                Exclude = "",
                Episode = 2,
                OnlyMatchOnce = true,
                FeedId = ids[5]
            });

            _context.SaveChanges();
        }
    }
}