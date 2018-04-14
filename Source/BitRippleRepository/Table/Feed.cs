using System.Collections.Generic;

namespace BitRippleRepository.Table
{
	public class Feed : IDalEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string URL { get; set; }
		public virtual ICollection<Download> Downloads { get; set; }
		public virtual ICollection<Filter> Filters { get; set; }
	}
}