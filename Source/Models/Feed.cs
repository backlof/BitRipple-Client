using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Models
{
	 [DataContract]
	 public class Feed
	 {
		  [DataMember(IsRequired = true)]
		  public int Id { get; set; }

		  [DataMember(IsRequired = true)]
		  public string Name { get; set; }

		  [DataMember(IsRequired = true)]
		  public string Url { get; set; }

		  [IgnoreDataMember]
		  public virtual List<Filter> Filters { get; set; }

		  [IgnoreDataMember]
		  public virtual List<Download> Downloads { get; set; }

		  [IgnoreDataMember]
		  public virtual List<Torrent> Torrents { get; set; }
	 }
}