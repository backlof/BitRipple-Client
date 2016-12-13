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