using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BitRippleService.Model
{
	 public class Torrent
	 {
		  public int Id { get; set; }

		  private static string MiniSeriesPattern => @" E(\d{1,3}) ";
		  private static string TvShowPattern => @" S(\d{1,3})E(\d{1,3}) ";
		  private static string SeasonPackPattern => @" S(\d{1,3}) ";

		  public string Name { get; set; }

		  public string FileUrl { get; set; }

		  public string GUID { get; set; }

		  public DateTime TimeOfUpload { get; set; }

		  [NotMapped]
		  public int MiniSeriesEpisode => Int32.Parse(Regex.Match(Name, MiniSeriesPattern, RegexOptions.IgnoreCase).Groups[1].Captures[0].Value);

		  [NotMapped]
		  public int TvEpisode => Int32.Parse(Regex.Match(Name, TvShowPattern, RegexOptions.IgnoreCase).Groups[2].Captures[0].Value);

		  [NotMapped]
		  public int TvSeason => Int32.Parse(Regex.Match(Name, TvShowPattern, RegexOptions.IgnoreCase).Groups[1].Captures[0].Value);

		  [NotMapped]
		  public int SeasonPackSeason => Int32.Parse(Regex.Match(Name, SeasonPackPattern, RegexOptions.IgnoreCase).Groups[1].Captures[0].Value);

		  [NotMapped]
		  public bool IsMiniSeries => Regex.IsMatch(Name, MiniSeriesPattern, RegexOptions.IgnoreCase);

		  [NotMapped]
		  public bool IsTvShow => Regex.IsMatch(Name, TvShowPattern, RegexOptions.IgnoreCase);

		  [NotMapped]
		  public bool IsSeasonPack => Regex.IsMatch(Name, SeasonPackPattern, RegexOptions.IgnoreCase);

		  [NotMapped]
		  public string CleanName => new string(Name.Where(x => !Path.GetInvalidFileNameChars().Contains(x)).ToArray());
	 }
}