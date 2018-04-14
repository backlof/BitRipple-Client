using BitRippleRepository;
using BitRippleUtility;

namespace BitRippleService.Service
{
	public class MVVMService
	{
		private readonly Utilities _utility;
		private readonly Repositories _repository;

		public MVVMService(Utilities utility, Repositories repository)
		{
			_utility = utility;
			_repository = repository;
		}
	}
}
