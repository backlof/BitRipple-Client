using System.IO;

namespace BitRippleShared
{
	public static class Constants
	{
		public static string Location { get; } = Directory.GetCurrentDirectory();
		public static string AssemblyDirectory => Path.Combine(Location, "Library");
		public static string DataDirectory => Path.Combine(Location, "Data");
		public static string SettingsJsonFile => Path.Combine(DataDirectory, "Settings.json");
		public static string DatabaseFile => Path.Combine(DataDirectory, "Database.db");
	}
}