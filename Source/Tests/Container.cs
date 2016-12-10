using Ninject;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using Rhino.Mocks;
using BitRipple;

namespace Tests
{
	 public class Container
	 {
		  public static BitRippleRepository GetRepository()
		  {
				return InternalGetRepository(GetContext);
		  }

		  private static Context GetContext()
		  {
				return new Context()
				{
					 Data = MockRepository.GenerateStub<IDataStorage>(),
					 RssReader = MockRepository.GenerateStub<IRssReader>(),
					 TorrentDownloader = MockRepository.GenerateStub<ITorrentDownloader>()
				};
		  }

		  private static BitRippleRepository InternalGetRepository(Func<Context> func)
		  {
				return new BitRippleRepository(func());
		  }

		  public static ConsoleApplication GetConsoleService()
		  {
				return InternalGetConsoleService(GetRepository);
		  }

		  private static ConsoleApplication InternalGetConsoleService(Func<BitRippleRepository> func)
		  {
				return new ConsoleApplication(func());
		  }
	 }
}