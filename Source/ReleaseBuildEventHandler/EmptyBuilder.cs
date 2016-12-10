using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseBuildEventHandler
{
	 public class EmptyBuilder : IDataWriter
	 {
		  private readonly DataBuilder _dataBuilder;

		  public EmptyBuilder()
		  {
				_dataBuilder = new DataBuilder();
		  }

		  public void BuildDefaults()
		  {
				_dataBuilder.Build();
		  }
	 }
}
