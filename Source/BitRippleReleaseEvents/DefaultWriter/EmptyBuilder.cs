using BitRippleService.Repository;

namespace BitRippleReleaseEvents.Defaults
{
	public class EmptyBuilder : IDataWriter
	{
		private readonly BitRippleContext _context;

		public EmptyBuilder(BitRippleContext context)
		{
			_context = context;
		}

		public void BuildDefaults()
		{
		}
	}
}