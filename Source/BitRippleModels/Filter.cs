using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitRippleModel
{
	public class Filter
	{
		public Filter()
		{
			Name = "";
			Downloads = new List<Download>();
			Disabled = true;
			TitleMatch = "";
			Exclude = "";
			Include = "";
			OnlyMatchOnce = true;
		}

		public int Id { get; set; }

		public string Name { get; set; }

		public bool Disabled { get; set; }

		public string TitleMatch { get; set; }

		public string Regex
		{
			get
			{
				if (TitleMatch.Length == 0) return @".^";

				StringBuilder sb = new StringBuilder();

				if (TitleMatch[0] != '*')
				{
					sb.Append(@"^");
				}
				foreach (char letter in TitleMatch)
				{
					if (letter == '*')
					{
						sb.Append(@".*");
					}
					else if (letter == '?')
					{
						sb.Append(".?");
					}
					else if (letter == '.')
					{
						sb.Append(@"[\s\.\-_]");
					}
					else
					{
						sb.Append(letter);
					}
				}

				return sb.ToString();
			}
		}

		public string Exclude { get; set; }

		public string[] Excludes => String.IsNullOrWhiteSpace(Exclude) ? new string[] { } : Exclude.ToLower().Split(';').ToArray();

		public string Include { get; set; }

		public string[] Includes => String.IsNullOrWhiteSpace(Include) ? new string[] { } : Include.ToLower().Split(';').ToArray();

		public int? Season { get; set; }

		public int? Episode { get; set; }

		public bool OnlyMatchOnce { get; set; }

		public int? FeedId { get; set; }

		public virtual Feed Feed { get; set; }

		public virtual ICollection<Download> Downloads { get; set; }

		public bool IsTvOfAnyKind => IsTvShow || IsMiniShow || IsSeasonPack;

		public bool IsTvShow => Season != null && Episode != null;

		public bool IsMiniShow => Season == null && Episode != null;

		public bool IsSeasonPack => Season != null && Episode == null;
	}
}