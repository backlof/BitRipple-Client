using BitRippleService;
using BitRippleService.Repository;
using BitRippleService.Utility;
using Ninject;
using Rhino.Mocks;

namespace BitRippleServiceTests
{
	public class Container
	{
		#region Runtime

		private static StandardKernel GetTestContainer()
		{
			StandardKernel container = new StandardKernel();
			container.Bind<IRssReader>().To<XmlRssReader>().InSingletonScope();
			container.Bind<ITorrentDownloader>().To<WebTorrentDownloader>().InSingletonScope();
			container.Bind<IFilterFeed>().To<FeedFilterer>().InSingletonScope();
			container.Bind<ISettingsService>().To<TestSettings>().InSingletonScope();
			container.Bind<BitRippleContext>().To<DisposableSqLiteDbContext>().InSingletonScope();
			return container;
		}

		public static DisposableApplicationService GetApplicationService()
		{
			return GetTestContainer().Get<DisposableApplicationService>();
		}

		public static DisposableBitRippleRepository GetRepository()
		{
			return GetTestContainer().Get<DisposableBitRippleRepository>();
		}

		public static DisposableSqLiteDbContext GetContext()
		{
			return GetTestContainer().Get<DisposableSqLiteDbContext>();
		}

		public static BitRippleServices GetService()
		{
			return GetTestContainer().Get<BitRippleService.Utility.BitRippleUtilities>();
		}

		#endregion Runtime

		#region Mocks

		public static BitRippleServices GetStubService()
		{
			return new BitRippleService.Utility.BitRippleUtilities()
			{
				Filter = MockRepository.GenerateStub<IFilterFeed>(),
				RssReader = MockRepository.GenerateStub<IRssReader>(),
				Settings = GetTestContainer().Get<ISettingsService>(),
				TorrentDownloader = MockRepository.GenerateStub<ITorrentDownloader>()
			};
		}

		public static ApplicationService GetStubApplicationService()
		{
			return new ApplicationService(GetStubService(), GetStubRepository());
		}

		public static BitRippleRepositories GetStubRepository()
		{
			return new BitRippleRepositories()
			{
				Feed = MockRepository.GenerateStub<IFeedRepository>()
			};
		}

		#endregion Mocks
	}
}