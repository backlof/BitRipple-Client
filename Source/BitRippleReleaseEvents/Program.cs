﻿using BitRippleReleaseEvents.Defaults;
using BitRippleService.Model;
using BitRippleService.Repository;
using BitRippleService.Service;
using BitRippleShared;
using Ninject;
using System;
using System.IO;
using System.Linq;
using ILRepacking;
using System.Collections.Generic;

namespace BitRippleReleaseEvents
{
	public class Program
	{
		private static readonly StandardKernel _container = new StandardKernel();

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
			builder.Repack("BitRippleService.dll", "Microsoft.Data.Sqlite.dll", "Microsoft.EntityFrameworkCore.dll");
			builder.MoveAssembliesToSubDirectory();
		}
	}

	public class PostBuildr
	{
		private readonly Type _defaultType;
		private string Location => Constants.Location;
		private string DataDirectory => Constants.DataDirectory;
		private string AssemblyDirectory => Constants.AssemblyDirectory;

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
			Directory.Delete(AssemblyDirectory, true);
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
				File.Delete(Path.Combine(Location, filename));
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

		internal void Repack(params string[] assemblies)
		{
			(new ILRepack(Options(assemblies))).Repack();
			// Have to delete dlls at the end because it is used
		}

		internal RepackOptions Options(params string[] assemblies)
		{
			return new RepackOptions
			{
				Parallel = true,
				Internalize = true,
				InputAssemblies = GetArray("BitRippleClient.exe", assemblies),
				TargetKind = ILRepack.Kind.Exe,
				OutputFile = "BitRippleClient.exe",
				AllowWildCards = true,
				SearchDirectories = new string[] { Location }
			};
		}

		private string[] GetArray(string item, params string[] items)
		{
			var list = new List<string>() { item };
			list.AddRange(items);
			return list.ToArray();
		}
	}
}