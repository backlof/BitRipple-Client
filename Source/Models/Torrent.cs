using System;
using System.Runtime.Serialization;

namespace Models
{
	 [DataContract]
	 public class Torrent
	 {
		  [DataMember(IsRequired = true)]
		  public string Name { get; set; }

		  [DataMember(IsRequired = true)]
		  public string FileUrl { get; set; }

		  [DataMember(IsRequired = true)]
		  public string GUID { get; set; }

		  [DataMember(IsRequired = true)]
		  public DateTime TimeOfUpload { get; set; }
	 }
}