using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
	 public class DataRepository
	 {
		  private readonly Context _context;

		  public DataRepository(Context context)
		  {
				_context = context;
		  }

		  public RepositoryResult Load()
		  {
				return Load(new List<string>());
		  }

		  private RepositoryResult Load(List<string> errorList)
		  {
				Load(_context.Data.LoadFilters, ref errorList, "Couldn't load filter");
				Load(_context.Data.LoadFeeds, ref errorList, "Couldn't load feeds");
				Load(_context.Data.LoadDownloads, ref errorList, "Couldn't load downloads");
				Load(_context.Data.LoadSettings, ref errorList, "Couldn't load settings");
				return errorList.Count == 0 ? RepositoryResult.SuccessResult : RepositoryResult.CreateError(errorList.ToArray());
		  }

		  public IEnumerable<Filter> Filters => _context.Data.Filters
				.Select(filter =>
				{
					 filter.Feed = _context.Data.Feeds.FirstOrDefault(feed => feed.Id == filter.FeedId);
					 filter.Downloads = _context.Data.Downloads.Where(download => download.FilterId == filter.Id).ToList();
					 return filter;
				})
				;

		  public IEnumerable<Feed> Feeds => _context.Data.Feeds
				.Select(feed =>
				{
					 feed.Filters = _context.Data.Filters
						  .Where(filter => filter.FeedId == feed.Id)
						  .Select(filter =>
						  {
								filter.Downloads = _context.Data.Downloads.Where(download => download.FilterId == filter.Id).ToList();
								return filter;
						  })
						  .ToList();
					 feed.Downloads = _context.Data.Downloads.Where(download => download.FeedId == feed.Id).ToList();
					 return feed;
				})
				;

		  public IEnumerable<Download> Downloads => _context.Data.Downloads
				.Select(download =>
				{
					 download.Feed = _context.Data.Feeds.FirstOrDefault(feed => feed.Id == download.FeedId);
					 download.Filter = _context.Data.Filters.FirstOrDefault(filter => filter.Id == download.Id);
					 return download;
				});

		  public Settings Settings => _context.Data.Settings;

		  public RepositoryResult Save()
		  {
				try
				{
					 _context.Data.SaveChanges();
					 return RepositoryResult.SuccessResult;
				}
				catch (Exception)
				{
					 return RepositoryResult.CreateError($"Couldn't save data");
				}
		  }

		  private void Load(Action action, ref List<string> errorList, string errorMessage)
		  {
				try
				{
					 action();
				}
				catch (Exception)
				{
					 errorList.Add(errorMessage);
				}
		  }
	 }
}