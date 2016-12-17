using Rhino.Mocks;
using System;
using BitRippleService.Service;
using BitRippleService.Model;
using BitRippleService.Repository;
using BitRippleService;
using Ninject;
using System.IO;
using BitRippleServiceTests.Interface;

namespace BitRippleServiceTests
{
	public class Container
	{
		public static BitRippleServices GetStubService()
		{
			return new BitRippleServices()
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

		public static BitRippleRepository GetStubRepository()
		{
			return new BitRippleRepository()
			{
				Feed = MockRepository.GenerateStub<IFeedRepository>()
			};
		}

		private static StandardKernel GetTestContainer()
		{
			StandardKernel container = new StandardKernel();
			container.Bind<IRssReader>().To<XmlRssReader>().InSingletonScope();
			container.Bind<ITorrentDownloader>().To<WebTorrentDownloader>().InSingletonScope();
			container.Bind<IFilterFeed>().To<FeedFilterer>().InSingletonScope();
			container.Bind<ISettingsService>().To<TestSettings>().InSingletonScope();
			container.Bind<BitRippleContext>().To<SQLiteDbContext>().InSingletonScope();
			return container;
		}

		public static ApplicationService GetApplicationService()
		{
			return new ApplicationService(GetService(), GetStubRepository());
		}

		public static BitRippleRepository GetRepository()
		{
			return new BitRippleRepository(GetContext());
		}

		public static BitRippleServices GetService()
		{
			return GetTestContainer().Get<BitRippleServices>();
		}

		public static BitRippleContext GetContext()
		{
			return null;
		}
	}
}