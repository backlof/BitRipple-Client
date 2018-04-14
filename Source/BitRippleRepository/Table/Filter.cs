using System.Collections.Generic;

namespace BitRippleRepository.Table
{
	public class Filter : IDalEntity
	{
		public Filter()
		{
			Name = "";
			Downloads = new List<Download>();
			Disabled = true;
			TitleMatch = "";
			Exclude = "";
			Include = "";
			OnlyMatchOnce = true;
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public int? FeedId { get; set; }
		public virtual Feed Feed { get; set; }
		public bool Disabled { get; set; }
		public string TitleMatch { get; set; }
		public string Exclude { get; set; }
		public string Include { get; set; }
		public bool OnlyMatchOnce { get; set; }
		public int? Season { get; set; }
		public int? Episode { get; set; }
		public virtual ICollection<Download> Downloads { get; set; }
	}
}