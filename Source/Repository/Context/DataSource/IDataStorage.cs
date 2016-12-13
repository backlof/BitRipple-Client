using Models;
using System.Collections.Generic;

namespace Repository
{
	 public interface IDataStorage
	 {
		  List<Download> Downloads { get; set; }

		  List<Feed> Feeds { get; set; }

		  List<Filter> Filters { get; set; }

		  Settings Settings { get; set; }

		  void LoadDownloads();

		  void LoadFeeds();

		  void LoadFilters();

		  void LoadSettings();

		  void SaveChanges();
	 }
}