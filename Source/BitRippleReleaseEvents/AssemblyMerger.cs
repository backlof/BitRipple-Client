using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILRepacking;

namespace BitRippleReleaseEvents
{
	public class AssemblyMerger : IDisposable
	{
		public string Location { get; set; }
		public string Executable { get; set; }
		public string[] Assemblies { get; set; }

		private RepackOptions Options => GetOptions();

		public void Merge()
		{
			(new ILRepack(Options)).Repack();
		}

		private RepackOptions GetOptions(params string[] assemblies)
		{
			return new RepackOptions
			{
				Parallel = true,
				Internalize = true,
				InputAssemblies = GetAssemblies(),
				TargetKind = ILRepack.Kind.Exe,
				OutputFile = Executable,
				AllowWildCards = false,
				SearchDirectories = new string[] { Location }
			};
		}

		public string[] GetAssemblies()
		{
			var list = new List<string>();
			list.Add(Executable);
			list.AddRange(Assemblies);
			return list.ToArray();
		}

		public void Dispose()
		{

		}
	}
}
