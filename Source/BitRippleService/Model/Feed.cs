using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitRippleService.Model
{
	public class Feed
	{
		public Feed()
		{
			Name = "";
			Filters = new List<Filter>();
			Downloads = new List<Download>();
			Torrents = new List<Torrent>();
		}

		public int Id { get; set; }

		public string Name { get; set; }

		public string Url { get; set; }

		public virtual ICollection<Filter> Filters { get; set; }

		public virtual ICollection<Download> Downloads { get; set; }

		[NotMapped]
		public virtual ICollection<Torrent> Torrents { get; set; }
	}
}