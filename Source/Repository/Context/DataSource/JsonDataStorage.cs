using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Repository
{
    public class JsonDataStorage : IDataStorage
    {
        public List<Download> Downloads { get; set; }
        public List<Feed> Feeds { get; set; }
        public List<Filter> Filters { get; set; }
        public Settings Settings { get; set; }

        public static string DownloadsPath => GetPathToFile("Downloads");
        public static string FeedsPath => GetPathToFile("Feeds");
        public static string FiltersPath => GetPathToFile("Filters");
        public static string Folder => Path.Combine(Directory.GetCurrentDirectory(), "Data");
        public static string SettingsPath => GetPathToFile("Settings");

        public static T ReadFromFile<T>(string location)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(location));
        }

        public static void WriteFile<T>(string location, T obj)
        {
            using (var stream = new StreamWriter(location, false))
            {
                (new JsonSerializer() { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore }).Serialize(stream, obj);
            }
        }

        public int GetNextId(DataContextId type)
        {
            switch (type)
            {
                case DataContextId.Filter:
                    Settings.NextIndex.Filter++;
                    return Settings.NextIndex.Filter;

                case DataContextId.Download:
                    Settings.NextIndex.Download++;
                    return Settings.NextIndex.Download;

                case DataContextId.Feed:
                    Settings.NextIndex.Feed++;
                    return Settings.NextIndex.Feed;

                default:
                    throw new NotImplementedException();
            }
        }

        public void LoadDownloads()
        {
            Downloads = ReadFromFile<List<Download>>(DownloadsPath);
        }

        public void LoadFeeds()
        {
            Feeds = ReadFromFile<List<Feed>>(FeedsPath);
        }

        public void LoadFilters()
        {
            Filters = ReadFromFile<List<Filter>>(FiltersPath);
        }

        public void LoadSettings()
        {
            Settings = ReadFromFile<Settings>(SettingsPath);
        }

        public void SaveChanges()
        {
            WriteFile(FiltersPath, Filters);
            WriteFile(FeedsPath, Feeds);
            WriteFile(DownloadsPath, Downloads);
            WriteFile(SettingsPath, Settings);
        }

        private static string GetPathToFile(string filename)
        {
            return Path.Combine(Folder, filename + ".json");
        }
    }
}