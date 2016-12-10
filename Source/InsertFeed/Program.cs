using Models;
using Repository;
using System;

namespace InsertFeed
{
	 internal class Program
	 {
		  public JsonDataStorage JsonDataStorage { get; set; }

		  private static void Main(string[] args)
		  {
				Execute(InsertFeed);
		  }

		  private static void Execute(Action<JsonDataStorage> method)
		  {
				method(new JsonDataStorage());
		  }

		  private static void InsertFeed(JsonDataStorage storage)
		  {
				try
				{
					 storage.LoadSettings();
					 storage.LoadFeeds();

					 storage.Feeds.Add(new Feed
					 {
						  Id = storage.Settings.NextIndex.Feed,
						  Name = "",
						  Url = ""
					 });

					 storage.Settings.NextIndex.Feed++;

					 JsonDataStorage.WriteFile(JsonDataStorage.SettingsPath, storage.Settings);
					 JsonDataStorage.WriteFile(JsonDataStorage.FeedsPath, storage.Feeds);
				}
				catch (Exception)
				{
				}
		  }
	 }
}