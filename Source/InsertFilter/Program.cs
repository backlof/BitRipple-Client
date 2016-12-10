using Models;
using Repository;
using System;
using System.Collections.Generic;

namespace DataInsert
{
	 internal class Program
	 {
		  public JsonDataStorage JsonDataStorage { get; set; }

		  private static void Main(string[] args)
		  {
				Execute(InsertFilter);

		  }

		  private static void Execute(Action<JsonDataStorage> method)
		  {
				method(new JsonDataStorage());
		  }

		  private static void InsertFilter(JsonDataStorage storage)
		  {
				try
				{
					 storage.LoadSettings();
					 storage.LoadFilters();

					 storage.Filters.Add(new Filter
					 {
						  Id = storage.Settings.NextIndex.Filter,
						  Name = "",
						  Enabled = false,
						  Criterias = "",
						  Exclude = new List<string>(),
						  Include = new List<string>(),
						  IgnoreCaps = true,
						  FeedId = storage.Settings.NextIndex.Feed - 1,
					 });

					 storage.Settings.NextIndex.Filter++;

					 JsonDataStorage.WriteFile(JsonDataStorage.SettingsPath, storage.Settings);
					 JsonDataStorage.WriteFile(JsonDataStorage.FiltersPath, storage.Filters);
				}
				catch (Exception)
				{
				}
		  }
	 }
}