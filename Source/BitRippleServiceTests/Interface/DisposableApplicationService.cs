using BitRippleService;
using BitRippleService.Utility;
using System;

namespace BitRippleServiceTests
{
	public class DisposableApplicationService : ApplicationService, IDisposable
	{
		public new DisposableBitRippleRepository Repository { get; set; }

		public DisposableApplicationService(BitRippleService.Utility.BitRippleUtilities service, DisposableBitRippleRepository repository) : base(service, repository)
		{
		}

		public void Dispose()
		{
			Repository.Dispose();
		}
	}
}