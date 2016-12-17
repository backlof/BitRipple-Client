namespace BitRippleService.Service
{
	public interface ISettingsService
	{
		string Location { get; set; }
		int Interval { get; set; }
	}
}