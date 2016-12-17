using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRippleService.Functionality
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

		  public static void WriteError(string error)
		  {
				Log($"Error: {error}");
		  }
	 }
}
