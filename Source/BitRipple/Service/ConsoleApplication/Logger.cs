using Models;
using System;

namespace BitRipple
{
	 public class Logger
	 {
		  public static void Log(string description)
		  {
				InternalLog($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] {description}");
		  }

		  private static void InternalLog(string text)
		  {
				Console.WriteLine(text);
		  }

		  public static void WriteErrors(IRepositoryResult result)
		  {
				foreach (var error in result.Errors)
				{
					 InternalWriteError(error);
				}
		  }

		  private static void InternalWriteError(string error)
		  {
				Log($"Error: {error}");
		  }
	 }
}