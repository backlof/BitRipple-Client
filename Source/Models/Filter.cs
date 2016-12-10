using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Models
{
	 [DataContract]
	 public class Filter
	 {
		  [OnDeserializing]
		  private void OnDeserializing(StreamingContext context)
		  {
				Include = new List<string>();
				Exclude = new List<string>();
				Criterias = "";
				Enabled = true;
				IgnoreCaps = true;
		  }

		  [DataMember(IsRequired = true)]
		  public int Id { get; set; }

		  [DataMember(IsRequired = true)]
		  public string Name { get; set; }

		  [DataMember]
		  public bool Enabled { get; set; }

		  [DataMember(Name = "Filter")]
		  public string Criterias { get; set; }

		  [DataMember]
		  public List<string> Exclude { get; set; }

		  [DataMember]
		  public List<string> Include { get; set; }

		  [DataMember]
		  public bool IgnoreCaps { get; set; }

		  [DataMember]
		  public EpisodeCriteria TvShow { get; set; }

		  [DataMember]
		  public MiniEpisodeCriteria MiniSeries { get; set; }

		  [DataMember]
		  public SeasonPackCriteria SeasonPack { get; set; }

		  [DataMember(Name = "Feed", IsRequired = true)]
		  public int? FeedId { get; set; }

		  [IgnoreDataMember]
		  public virtual Feed Feed { get; set; }

		  [IgnoreDataMember]
		  public virtual List<Download> Downloads { get; set; }
	 }

	 [DataContract]
	 public class EpisodeCriteria
	 {
		  [DataMember]
		  public int Season { get; set; }

		  [DataMember]
		  public int Episode { get; set; }

		  [DataMember]
		  public bool OnlyDownloadEpisodeOnce { get; set; }
	 }

	 [DataContract]
	 public class MiniEpisodeCriteria
	 {
		  [DataMember]
		  public int Episode { get; set; }

		  [DataMember]
		  public bool OnlyDownloadEpisodeOnce { get; set; }
	 }

	 [DataContract]
	 public class SeasonPackCriteria
	 {
		  [DataMember]
		  public int Season { get; set; }

		  [DataMember]
		  public bool OnlyDownloadSeasonOnce { get; set; }
	 }
}