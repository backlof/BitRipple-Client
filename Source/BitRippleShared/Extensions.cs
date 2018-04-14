using System.IO;
using System.Linq;

namespace BitRippleShared
{
	public static class Extensions
	{
		public static string CleanInvalidFileNameChars(this string input)
		{
			return new string(input.Where(x => !Path.GetInvalidFileNameChars().Contains(x)).ToArray());
		}
	}
}
