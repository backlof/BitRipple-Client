using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BitRippleService.Model
{
	 public class Download : Torrent
	 {
		  public DateTime TimeOfDownload { get; set; }

		  public int? FeedId { get; set; }

		  public virtual Feed Feed { get; set; }

		  public int? FilterId { get; set; }

		  public virtual Filter Filter { get; set; }
	 }
}