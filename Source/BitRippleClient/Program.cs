using BitRippleService;
using BitRippleService.Repository;
using BitRippleService.Utility;
using Ninject;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BitRippleClient
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += OnUnhandleException;
			RunOnInterval(GetApplicationService, x => x.Update, x => TimeSpan.FromMinutes(x.Utility.Settings.Interval));
		}

		private static void RunOnInterval(Func<ApplicationService> service, Func<ApplicationService, Func<Task>> method, Func<ApplicationService, TimeSpan> interval)
		{
			RunOnInterval(service(), method, interval);
		}

		private static void RunOnInterval(ApplicationService service, Func<ApplicationService, Func<Task>> method, Func<ApplicationService, TimeSpan> interval)
		{
			ConsoleInterval.Run(method(service), interval(service));
		}

		private static ApplicationService GetApplicationService()
		{
			StandardKernel container = new StandardKernel();
			container.Bind<IRssReader>().To<XmlRssReader>().InSingletonScope();
			container.Bind<ITorrentDownloader>().To<WebTorrentDownloader>().InSingletonScope();
			container.Bind<IFilterFeed>().To<FeedFilterer>().InSingletonScope();
			container.Bind<ISettingsService>().To<JsonSettingsReader>().InSingletonScope();
			container.Bind<BitRippleContext>().To<SQLiteDbContext>().InSingletonScope();
			return container.Get<ApplicationService>();
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