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
	 public static class FilterExtensions
	 {
		  private static string MiniSeriesPattern => @" E(\d{1,3}) ";
		  private static string TvShowPattern => @" S(\d{1,3})E(\d{1,3}) ";
		  private static string SeasonPackPattern => @" S(\d{1,3}) ";

		  public static bool CanDownloadMiniSeriesEpisode(this Filter filter, Torrent torrent)
		  {
				return torrent.GetMiniSeriesEpisode() >= filter.MiniSeries.Episode;
		  }

		  public static bool CanDownloadTvShowEpisode(this Filter filter, Torrent torrent)
		  {
				return torrent.GetTvSeason() > filter.TvShow.Season || (torrent.GetTvSeason() == filter.TvShow.Season && torrent.GetTvEpisode() >= filter.TvShow.Episode);
		  }

		  public static bool CanDownloadSeasonPackSeason(this Filter filter, Torrent torrent)
		  {
				return torrent.GetSeasonPackSeason() >= filter.SeasonPack.Season;
		  }

		  public static bool DoesTitleMatch(this Filter filter, Torrent torrent)
		  {
				return InternalDoesTitleMatch(filter, filter.IgnoreCaps ? torrent.Name.ToLower() : torrent.Name);
		  }

		  public static List<string> GetExclude(this Filter filter)
		  {
				return filter.IgnoreCaps ? filter.Exclude.Select(x => x.ToLower()).ToList() : filter.Exclude;
		  }

		  public static List<string> GetInclude(this Filter filter)
		  {
				return filter.IgnoreCaps ? filter.Include.Select(x => x.ToLower()).ToList() : filter.Include;
		  }

		  public static int GetMiniSeriesEpisode(this Torrent torrent)
		  {
				return Int32.Parse(Regex.Match(torrent.Name, MiniSeriesPattern, RegexOptions.IgnoreCase).Groups[1].Captures[0].Value);
		  }

		  public static string GetRegexPattern(this Filter filter)
		  {
				return InternalGetRegexPattern(filter.Criterias);
		  }

		  public static int GetTvEpisode(this Torrent torrent)
		  {
				return Int32.Parse(Regex.Match(torrent.Name, TvShowPattern, RegexOptions.IgnoreCase).Groups[2].Captures[0].Value);
		  }

		  public static int GetTvSeason(this Torrent torrent)
		  {
				return Int32.Parse(Regex.Match(torrent.Name, TvShowPattern, RegexOptions.IgnoreCase).Groups[1].Captures[0].Value);
		  }

		  public static int GetSeasonPackSeason(this Torrent torrent)
		  {
				return Int32.Parse(Regex.Match(torrent.Name, SeasonPackPattern, RegexOptions.IgnoreCase).Groups[1].Captures[0].Value);
		  }

		  public static bool HasDownloadedBefore(this Filter filter, Torrent torrent)
		  {
				return filter.Downloads.Any(x => x.GUID == torrent.GUID && x.Name == torrent.Name);
		  }

		  public static bool IsMiniSeries(this Filter filter)
		  {
				return filter.MiniSeries != null;
		  }

		  public static bool IsSeasonPack(this Filter filter)
		  {
				return filter.SeasonPack != null;
		  }

		  public static bool IsMiniSeries(this Torrent torrent)
		  {
				return Regex.IsMatch(torrent.Name, MiniSeriesPattern, RegexOptions.IgnoreCase);
		  }

		  public static bool IsTvShow(this Filter filter)
		  {
				return filter.TvShow != null;
		  }

		  public static bool IsTvShow(this Torrent torrent)
		  {
				return Regex.IsMatch(torrent.Name, TvShowPattern, RegexOptions.IgnoreCase);
		  }

		  public static bool IsSeasonPack(this Torrent torrent)
		  {
				return Regex.IsMatch(torrent.Name, SeasonPackPattern, RegexOptions.IgnoreCase);
		  }

		  public static bool MiniSeriesEpisodeHasBeenDownloadedBefore(this Filter filter, Torrent torrent)
		  {
				return filter.Downloads.Any(x => x.GetMiniSeriesEpisode() == torrent.GetMiniSeriesEpisode());
		  }

		  public static bool SeasonPackSeasonHasBeenDownloadedBefore(this Filter filter, Torrent torrent)
		  {
				return filter.Downloads.Any(x => x.GetSeasonPackSeason() == torrent.GetSeasonPackSeason());
		  }

		  public static string RemoveDiacritics(this string text)
		  {
				StringBuilder sb = new StringBuilder();

				foreach (var c in text.Normalize(NormalizationForm.FormD))
				{
					 if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
					 {
						  sb.Append(c);
					 }
				}

				return sb.ToString().Normalize(NormalizationForm.FormC);
		  }

		  public static string RemoveIllegalFilterCharacters(this string filter)
		  {
				return String.Join("", filter.Split(@"$^{[(|)]}+\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
		  }

		  public static bool ShouldDownloadMiniSeries(this Filter filter, Torrent torrent)
		  {
				if (!filter.CanDownloadMiniSeriesEpisode(torrent))
					 return false;
				else if (filter.MiniSeries.OnlyDownloadEpisodeOnce && filter.MiniSeriesEpisodeHasBeenDownloadedBefore(torrent))
					 return false;
				else
					 return true;
		  }

		  public static bool ShouldDownloadTvShow(this Filter filter, Torrent torrent)
		  {
				if (!filter.CanDownloadTvShowEpisode(torrent))
					 return false;
				else if (filter.TvShow.OnlyDownloadEpisodeOnce && filter.TvEpisodeHasBeenDownloadedBefore(torrent))
					 return false;
				else
					 return true;
		  }

		  public static bool ShouldDownloadSeasonPack(this Filter filter, Torrent torrent)
		  {
				if (!filter.CanDownloadSeasonPackSeason(torrent))
					 return false;
				else if (filter.SeasonPack.OnlyDownloadSeasonOnce && filter.SeasonPackSeasonHasBeenDownloadedBefore(torrent))
					 return false;
				else
					 return true;
		  }

		  public static bool TvEpisodeHasBeenDownloadedBefore(this Filter filter, Torrent torrent)
		  {
				return filter.Downloads.Any(x => x.GetTvEpisode() == torrent.GetTvEpisode() && x.GetTvSeason() == torrent.GetTvSeason());
		  }
		  private static bool InternalDoesTitleMatch(Filter filter, string title)
		  {
				if (!Regex.IsMatch(title, filter.GetRegexPattern(), filter.IgnoreCaps ? RegexOptions.IgnoreCase : RegexOptions.None))
					 return false;
				else if (!filter.GetInclude().All(title.Contains))
					 return false;
				else if (filter.GetExclude().Any(title.Contains))
					 return false;
				else
					 return true;
		  }

		  private static string InternalGetRegexPattern(string filter)
		  {
				if (filter.Length == 0) return @".^";

				StringBuilder sb = new StringBuilder();

				if (filter[0] != '*')
				{
					 sb.Append(@"^");
				}
				foreach (char letter in filter)
				{
					 if (letter == '*')
					 {
						  sb.Append(@".*");
					 }
					 else if (letter == '?')
					 {
						  sb.Append(".?");
					 }
					 else if (letter == '.')
					 {
						  sb.Append(@"[\s\.\-_]");
					 }
					 else
					 {
						  sb.Append(letter);
					 }
				}

				return sb.ToString();
		  }
	 }
}
