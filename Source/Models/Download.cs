using System;
using System.Runtime.Serialization;

namespace Models
{
	 [DataContract]
	 public class Download : Torrent
	 {
		  [DataMember(IsRequired = true)]
		  public int Id { get; set; }

		  [DataMember(IsRequired = true)]
		  public DateTime TimeOfDownload { get; set; }

		  [DataMember(Name = "Feed", IsRequired = true)]
		  public int? FeedId { get; set; }

		  [IgnoreDataMember]
		  public virtual Feed Feed { get; set; }

		  [DataMember(Name = "Filter", IsRequired = true)]
		  public int? FilterId { get; set; }

		  [IgnoreDataMember]
		  public virtual Filter Filter { get; set; }
	 }
}