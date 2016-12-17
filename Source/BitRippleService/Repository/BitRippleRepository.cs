using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRippleService.Repository
{
	public class BitRippleRepository
	{
		public IFeedRepository Feed { get; set; }

		public BitRippleRepository() { }

		public BitRippleRepository(BitRippleContext context)
		{
			Feed = new FeedRepository(context);
		}
	}
}
