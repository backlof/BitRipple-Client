using BitRippleReleaseEvents.Defaults;
using BitRippleService.Model;
using BitRippleService.Repository;
using BitRippleService.Service;
using Ninject;
using System;
using System.IO;

namespace BitRippleReleaseEvents
{
	public class Program
	{
		private static readonly StandardKernel _container = new StandardKernel();

		public static void Main(string[] args)
		{
			Execute(PostBuild, typeof(MiniNovaBuilder), args);
		}

		private static void Execute(Action<PostBuildr> method, Type defaultType, string[] args, StandardKernel container = null)
		{
			method(new PostBuildr(defaultType));
		}

		public static void PostBuild(PostBuildr builder)
		{
			builder.RemoveUnwantedFiles();
			builder.BuildDefaults();
		}
	}

	public class PostBuildr
	{
		private readonly Type _defaultType;
		private readonly string _location;
		private string DataFolder => Path.Combine(Directory.GetCurrentDirectory(), "Data");

		public PostBuildr(Type dataWriter)
		{
			_defaultType = dataWriter;
			_location = Directory.GetCurrentDirectory();
		}

		public void RemoveUnwantedFiles()
		{
			RemoveUnwantedFiles(new[]
			{
					 "AutoGenerate.exe",
					 "AutoGenerate.pdb",
					 "BitRipple.exe.config",
					 "BitRipple.pdb",
					 "BitRipple.vshost.exe",
					 "BitRipple.vshost.exe.config",
					 "BitRipple.vshost.exe.manifest",
					 "BuildEventHandler.exe.config",
					 "BuildEventHandler.pdb",
					 "InsertFeed.exe.config",
					 "InsertFeed.pdb",
					 "InsertFilter.exe.config",
					 "InsertFilter.pdb",
					 "Models.pdb",
					 "Newtonsoft.Json.xml",
					 "Ninject.xml",
					 "Repository.pdb",
					 "ReleaseBuildEventHandler.exe.config",
					 "ReleaseBuildEventHandler.pdb",
					 "ChooseLocation.exe.config",
					 "ChooseLocation.pdb",
					 @"Data\\Data.db",
					 "Microsoft.EntityFrameworkCore.Relational.xml",
					 "Microsoft.EntityFrameworkCore.Sqlite.xml",
					 "Microsoft.Data.Sqlite.xml",
					 "Microsoft.EntityFrameworkCore.xml",
					 "Microsoft.Extensions.Caching.Abstractions.xml",
					 "Microsoft.Extensions.Caching.Memory.xml",
					 "Microsoft.Extensions.DependencyInjection.Abstractions.xml",
					 "Microsoft.Extensions.DependencyInjection.xml",
					 "Microsoft.Extensions.Logging.Abstractions.xml",
					 "Microsoft.Extensions.Primitives.xml",
					 "Service.pdb",
					 "Remotion.Linq.xml",
					 "Microsoft.Extensions.Options.xml",
					 "Microsoft.Extensions.Logging.xml",
					 "System.Collections.Immutable.xml",
					 "System.Diagnostics.DiagnosticSource.xml",
					 "System.Interactive.Async.xml",
					 "System.Runtime.CompilerServices.Unsafe.xml"
				});
		}

		private void RemoveUnwantedFiles(params string[] filenames)
		{
			foreach (var filename in filenames)
			{
				File.Delete(Path.Combine(_location, filename));
			}
		}

		public void BuildDefaults()
		{
			StandardKernel container = new StandardKernel();
			container.Bind<BitRippleContext>().To<SQLiteDbContext>().InSingletonScope();
			container.Bind<IDataWriter>().To(_defaultType).InSingletonScope();
			container.Get<IDataWriter>().BuildDefaults();
			JsonSettingsReader.WriteFile(new Settings { Interval = 5, Location = Directory.GetCurrentDirectory() });
		}
	}
}