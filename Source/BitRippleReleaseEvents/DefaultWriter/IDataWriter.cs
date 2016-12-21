using System;

namespace BitRippleReleaseEvents.Defaults
{
	public interface IDataWriter : IDisposable
	{
		void BuildDefaults();
	}
}