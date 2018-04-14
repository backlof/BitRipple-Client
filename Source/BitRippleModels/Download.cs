using System;

namespace BitRippleModel
{
	public class Download
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string FileUrl { get; set; }
		public string GUID { get; set; }

		public DateTime TimeOfUpload { get; set; }
		public DateTime TimeOfDownload { get; set; }

		public int? FeedId { get; set; }
		public virtual Feed Feed { get; set; }
		public int? FilterId { get; set; }
		public virtual Filter Filter { get; set; }
		public int? Season { get; set; }
		public int? Episode { get; set; }
	}
}