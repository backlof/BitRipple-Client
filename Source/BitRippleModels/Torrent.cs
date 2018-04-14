using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BitRippleModel
{
	public class Torrent
	{
		public string Name { get; set; }
		public string GUID { get; set; }
		public string FileUrl { get; set; }
		public DateTime TimeOfUpload { get; set; }

		private static string MiniSeriesPattern => @" E(\d{1,3}) ";
		private static string TvShowPattern => @" S(\d{1,3})E(\d{1,3}) ";
		private static string SeasonPackPattern => @" S(\d{1,3}) ";

		// TODO Put function in here to calculate episodes and all that stuff once and for all

		public int MiniSeriesEpisode => Int32.Parse(Regex.Match(Name, MiniSeriesPattern, RegexOptions.IgnoreCase).Groups[1].Captures[0].Value);

		public int TvEpisode => Int32.Parse(Regex.Match(Name, TvShowPattern, RegexOptions.IgnoreCase).Groups[2].Captures[0].Value);

		public int TvSeason => Int32.Parse(Regex.Match(Name, TvShowPattern, RegexOptions.IgnoreCase).Groups[1].Captures[0].Value);

		public int SeasonPackSeason => Int32.Parse(Regex.Match(Name, SeasonPackPattern, RegexOptions.IgnoreCase).Groups[1].Captures[0].Value);

		public bool IsMiniSeries => Regex.IsMatch(Name, MiniSeriesPattern, RegexOptions.IgnoreCase);

		public bool IsTvShow => Regex.IsMatch(Name, TvShowPattern, RegexOptions.IgnoreCase);

		public bool IsSeasonPack => Regex.IsMatch(Name, SeasonPackPattern, RegexOptions.IgnoreCase);
	}
}