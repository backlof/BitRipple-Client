using BitRippleService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRippleService
{
	public class BitRippleServices
	{
		public FeedUpdateService Application { get; set; }
		public MVVMService FeedUpdate { get; set; }

		public BitRippleServices(BitRippleRepository.Repositories repository, BitRippleUtility.Utilities service)
		{
			Application = new FeedUpdateService(service, repository);
			FeedUpdate = new MVVMService(service, repository);
		}
	}
}
