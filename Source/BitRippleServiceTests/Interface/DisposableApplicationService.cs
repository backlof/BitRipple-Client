using BitRippleService;
using BitRippleService.Service;
using System;

namespace BitRippleServiceTests
{
	public class DisposableApplicationService : ApplicationService, IDisposable
	{
		public new DisposableBitRippleRepository Repository { get; set; }

		public DisposableApplicationService(BitRippleServices service, DisposableBitRippleRepository repository) : base(service, repository)
		{
		}

		public void Dispose()
		{
			Repository.Dispose();
		}
	}
}