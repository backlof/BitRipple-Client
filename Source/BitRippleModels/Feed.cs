using System.Collections.Generic;

namespace BitRippleModel
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

		public virtual ICollection<Download> Downloads { get; set; }
		public virtual ICollection<Filter> Filters { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public virtual ICollection<Torrent> Torrents { get; set; }
		public string Url { get; set; }
	}
}