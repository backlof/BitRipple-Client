using BitRippleService.Service;
using System.Runtime.Serialization;

namespace BitRippleService.Model
{
	[DataContract]
	public class Settings : ISettingsService
	{
		[DataMember]
		public string Location { get; set; }

		[DataMember]
		public int Interval { get; set; }
	}
}