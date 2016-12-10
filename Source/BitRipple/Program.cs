using Ninject;
using Repository;
using System;
using System.IO;

namespace BitRipple
{
	 internal class Program
	 {
		  public static void Main(string[] args)
		  {
				AppDomain.CurrentDomain.UnhandledException += OnUnhandleException;
				GetApplicationService().Run();

		  }

		  public static IApplicationService GetApplicationService(StandardKernel container = null)
		  {
				container = new StandardKernel();
				container.Bind<IDataStorage>().To<JsonDataStorage>().InSingletonScope();
				container.Bind<IRssReader>().To<XmlRssReader>().InSingletonScope();
				container.Bind<ITorrentDownloader>().To<WebTorrentDownloader>().InSingletonScope();
				container.Bind<BitRippleRepository>().ToSelf().InSingletonScope();
				container.Bind<IApplicationService>().To<ConsoleApplication>().InSingletonScope();
				return container.Get<IApplicationService>();
		  }

		  private static void OnUnhandleException(object sender, UnhandledExceptionEventArgs e)
		  {
				using (StreamWriter sw = File.AppendText(@"Error.log"))
				{
					 sw.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
					 sw.WriteLine(e.ExceptionObject.ToString());
					 sw.Write(Environment.NewLine);
				}

				Environment.Exit(1);
		  }
	 }
}