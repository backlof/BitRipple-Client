using BitRippleService.Model;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BitRippleService.Service
{
	public static class FilterExtensions
	{
		public static bool CanDownloadMiniSeriesEpisode(this Filter filter, Torrent torrent)
		{
			return torrent.MiniSeriesEpisode >= filter.Episode;
		}

		public static bool CanDownloadTvShowEpisode(this Filter filter, Torrent torrent)
		{
			return torrent.TvSeason > filter.Season || (torrent.TvSeason == filter.Season && torrent.TvEpisode >= filter.Episode);
		}

		public static bool CanDownloadSeasonPackSeason(this Filter filter, Torrent torrent)
		{
			return torrent.SeasonPackSeason >= filter.Season;
		}

		public static bool DoesTitleMatch(this Filter filter, Torrent torrent)
		{
			if (!Regex.IsMatch(torrent.Name, filter.Regex, RegexOptions.IgnoreCase))
				return false;
			else if (!filter.Includes.All(torrent.Name.LowerAndStripOfDiacritics().Contains))
				return false;
			else if (filter.Excludes.Any(torrent.Name.LowerAndStripOfDiacritics().Contains))
				return false;
			else
				return true;
		}

		public static bool HasDownloadedBefore(this Filter filter, Torrent torrent)
		{
			return filter.Downloads.Any(x => x.GUID == torrent.GUID && x.Name == torrent.Name);
		}

		public static bool MiniSeriesEpisodeHasBeenDownloadedBefore(this Filter filter, Torrent torrent)
		{
			return filter.Downloads.Any(x => x.MiniSeriesEpisode == torrent.MiniSeriesEpisode);
		}

		public static bool SeasonPackSeasonHasBeenDownloadedBefore(this Filter filter, Torrent torrent)
		{
			return filter.Downloads.Any(x => x.SeasonPackSeason == torrent.SeasonPackSeason);
		}

		public static string RemoveIllegalFilterCharacters(this string filter)
		{
			return String.Join("", filter.Split(@"$^{[(|)]}+\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
		}

		public static bool ShouldDownloadMiniSeries(this Filter filter, Torrent torrent)
		{
			if (!filter.CanDownloadMiniSeriesEpisode(torrent))
				return false;
			else if (filter.OnlyMatchOnce && filter.MiniSeriesEpisodeHasBeenDownloadedBefore(torrent))
				return false;
			else
				return true;
		}

		public static bool ShouldDownloadTvShow(this Filter filter, Torrent torrent)
		{
			if (!filter.CanDownloadTvShowEpisode(torrent))
				return false;
			else if (filter.OnlyMatchOnce && filter.TvEpisodeHasBeenDownloadedBefore(torrent))
				return false;
			else
				return true;
		}

		public static bool ShouldDownloadSeasonPack(this Filter filter, Torrent torrent)
		{
			if (!filter.CanDownloadSeasonPackSeason(torrent))
				return false;
			else if (filter.OnlyMatchOnce && filter.SeasonPackSeasonHasBeenDownloadedBefore(torrent))
				return false;
			else
				return true;
		}

		public static bool TvEpisodeHasBeenDownloadedBefore(this Filter filter, Torrent torrent)
		{
			return filter.Downloads.Any(x => x.TvEpisode == torrent.TvEpisode && x.TvSeason == torrent.TvSeason);
		}

		public static string LowerAndStripOfDiacritics(this string text)
		{
			StringBuilder sb = new StringBuilder();

			foreach (var c in text.Normalize(NormalizationForm.FormD))
			{
				if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
				{
					sb.Append(c);
				}
			}

			return sb.ToString().Normalize(NormalizationForm.FormC).ToLower();
		}
	}
}