using BitRippleReleaseEvents.Defaults;
using BitRippleService.Model;
using BitRippleService.Repository;
using BitRippleService.Service;
using Ninject;
using System;
using System.IO;
using System.Linq;

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
			builder.RemoveFilesWithExtension(".config", ".pdb", ".xml");
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

		public void RemoveFilesWithExtension(params string[] extensions)
		{
			foreach (var extension in extensions)
			{
				RemoveFilesWithExtension(extension);
			}
		}

		private void RemoveFilesWithExtension(string extension)
		{
			foreach (string file in Directory.GetFiles(_location, $"*{extension}").Where(item => item.EndsWith(extension)))
			{
				File.Delete(file);
			}
		}

		public void RemoveFiles(params string[] filenames)
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