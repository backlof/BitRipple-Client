using System;

namespace BitRippleRepository.Table
{
	public class Download : IDalEntity
	{
		public int Id { get; set; }
		public string FileUrl { get; set; }
		public string GUID { get; set; }
		public string Name { get; set; }
		public DateTime TimeOfUpload { get; set; }
		public int? FeedId { get; set; }
		public virtual Feed Feed { get; set; }
		public int? FilterId { get; set; }
		public virtual Filter Filter { get; set; }
		public DateTime TimeOfDownload { get; set; }
		public int? Season { get; set; }
		public int? Episode { get; set; }
	}
}