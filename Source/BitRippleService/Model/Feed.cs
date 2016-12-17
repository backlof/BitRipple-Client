﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BitRippleService.Model
{
	 public class Feed
	 {
		  public int Id { get; set; }

		  public string Name { get; set; }

		  public string Url { get; set; }

		  public virtual ICollection<Filter> Filters { get; set; }

		  public virtual ICollection<Download> Downloads { get; set; }

		  [NotMapped]
		  public virtual ICollection<Torrent> Torrents { get; set; }
	 }
}