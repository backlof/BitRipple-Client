using BitRippleService.Service;
using System.IO;

namespace BitRippleServiceTests
{
	public class TestSettings : ISettingsService
	{
		public int Interval { get; set; } = 5;

		public string Location { get; set; } = Directory.GetCurrentDirectory();
	}
}