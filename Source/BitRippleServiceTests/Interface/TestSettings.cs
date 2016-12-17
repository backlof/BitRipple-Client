using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitRippleService.Service;
using System.IO;

namespace BitRippleServiceTests.Interface
{
	public class TestSettings : ISettingsService
	{
		public int Interval { get; set; } = 5;

		public string Location { get; set; } = Directory.GetCurrentDirectory();
	}
}
