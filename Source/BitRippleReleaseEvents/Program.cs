using BitRippleReleaseEvents.Defaults;
using BitRippleService.Model;
using BitRippleService.Repository;
using BitRippleService.Service;
using BitRippleShared;
using Ninject;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitRippleReleaseEvents
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Execute(PostBuild, typeof(EmptyBuilder), args);
		}

		private static void Execute(Action<PostBuildr> method, Type defaultType, string[] args, StandardKernel container = null)
		{
			method(new PostBuildr(defaultType));
		}

		public static void PostBuild(PostBuildr builder)
		{
			builder.MoveSqLiteAssembly();
			builder.RemoveFilesWithExtension(".pdb", ".xml");
			builder.RemoveFiles("BitRippleReleaseEvents.exe.config");
			builder.BuildDefaults();
			builder.MoveAssembliesToSubDirectory();
		}
	}

	public class PostBuildr
	{
		private readonly Type _defaultType;
		private string Location { get; set; } = Constants.Location;
		private string DataDirectory { get; set; } = Constants.DataDirectory;
		private string AssemblyDirectory { get; set; } = Constants.AssemblyDirectory;

		public PostBuildr(Type dataWriter)
		{
			_defaultType = dataWriter;
		}

		public void RemoveFilesWithExtension(params string[] extensions)
		{
			foreach (var extension in extensions)
			{
				RemoveFilesWithExtension(extension);
			}
		}

		public void DeleteDataDir()
		{
			if (Directory.Exists(DataDirectory))
			{
				Directory.Delete(DataDirectory, true);
			}
		}

		public void MoveAssembliesToSubDirectory()
		{
			if (Directory.Exists(AssemblyDirectory))
			{
				Directory.Delete(AssemblyDirectory, true);
			}
			Directory.CreateDirectory(AssemblyDirectory);

			foreach (string file in Directory.GetFiles(Location, $"*.dll").Where(item => item.EndsWith(".dll")))
			{
				File.Move(file, Path.Combine(AssemblyDirectory, Path.GetFileName(file)));
			}
		}

		public void MoveSqLiteAssembly()
		{
			MoveFile(Path.Combine(Location, "x86", "sqlite3.dll"), Path.Combine(Location, "SqLite3.dll"));
			Directory.Delete(Path.Combine(Location, "x86"), true);
			Directory.Delete(Path.Combine(Location, "x64"), true);
		}

		private void MoveFile(string input, string output)
		{
			if (File.Exists(output))
			{
				File.Delete(output);
			}
			File.Move(input, output);
		}

		private void RemoveFilesWithExtension(string extension)
		{
			foreach (string file in Directory.GetFiles(Location, $"*{extension}").Where(item => item.EndsWith(extension)))
			{
				File.Delete(file);
			}
		}

		public void RemoveFiles(params string[] filenames)
		{
			foreach (var filename in filenames)
			{
				InternalRemoveFile(Path.Combine(Location, filename));
			}
		}

		private void InternalRemoveFile(string file)
		{
			File.SetAttributes(file, FileAttributes.Normal);
			File.Delete(file);
		}

		public void BuildDefaults()
		{
			using (var datawriter = GetDataWriter())
			{
				datawriter.BuildDefaults();
			}
		}

		private IDataWriter GetDataWriter(StandardKernel container = null)
		{
			container = new StandardKernel();
			container.Bind<BitRippleContext>().To<SQLiteDbContext>().InSingletonScope();
			container.Bind<IDataWriter>().To(_defaultType).InSingletonScope();
			return container.Get<IDataWriter>();
		}
	}
}