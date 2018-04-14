using System;
using System.IO;

namespace BitRippleUtility
{
	[DataContract]
	public class JsonSettingsReader : ISettingsService
	{
		public string Location { get; set; }
		public int Interval { get; set; }

		private static string Path => Constants.SettingsJsonFile;

		public JsonSettingsReader(Settings settings)
		{
			Map(settings);
		}

		public JsonSettingsReader()
		{
			Map(ReadFromJson);
		}

		private void Map(Func<Settings> func)
		{
			Map(func());
		}

		private void Map(Settings settings)
		{
			Location = settings.Location;
			Interval = settings.Interval;
		}

		public static void WriteFile(Settings settings)
		{
			using (var stream = new StreamWriter(Path, false))
			{
				(new JsonSerializer() { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore }).Serialize(stream, settings);
			}
		}

		private Settings ReadFromJson()
		{
			return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Path));
		}
	}
}