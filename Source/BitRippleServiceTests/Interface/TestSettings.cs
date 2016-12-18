using BitRippleService.Service;
using BitRippleShared;

namespace BitRippleServiceTests
{
	public class TestSettings : ISettingsService
	{
		public int Interval { get; set; } = 5;
		public string Location { get; set; } = Constants.Location;
	}
}