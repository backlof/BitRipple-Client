using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace ReleaseBuildEventHandler
{
	 public class Program
	 {
		  private static readonly StandardKernel _container = new StandardKernel();

		  public static void Main(string[] args)
		  {
				Execute(PostBuild, args);
		  }

		  private static void Execute(Action<PostBuildr> method, string[] args, StandardKernel container = null)
		  {
				container = new StandardKernel();
				container.Bind<IDataWriter>().To<MiniNovaBuilder>().InSingletonScope();
				container.Bind<PostBuildr>().ToSelf().WithConstructorArgument("location", args[0]);
				method(container.Get<PostBuildr>());
		  }

		  public static void PostBuild(PostBuildr builder)
		  {
				builder.BuildDefaults();
				//builder.MoveDLLs();
				builder.RemoveUnwantedFiles();
		  }
	 }

	 public enum BuildConfiguration
	 {
		  Release, Debug
	 }

	 public class PostBuildr
	 {
		  private readonly IDataWriter _dataWriter;
		  private readonly string _location;

		  private string assembly_dir => Path.Combine(_location, "Library" + @"\\");

		  public PostBuildr(IDataWriter writer, string location)
		  {
				_dataWriter = writer;
				_location = location;
		  }

		  public void RemoveFilesAndFolders()
		  {
				Directory.Delete(assembly_dir);
		  }

		  public void MoveDLLs()
		  {
				MoveDLLs(assembly_dir);
		  }

		  private void MoveDLLs(string dir)
		  {
				if (!Directory.Exists(dir))
				{
					 Directory.CreateDirectory(dir);
				}

				foreach (var file in Directory.GetFiles(assembly_dir))
				{
					 File.Delete(file);
				}

				foreach (var file in Directory.GetFiles(_location, "*.dll"))
				{
					 File.Move(file, Path.Combine(dir, Path.GetFileName(file)));
				}
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
					 "ChooseLocation.pdb"
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
				_dataWriter.BuildDefaults();
		  }
	 }
}
