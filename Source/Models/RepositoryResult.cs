using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public interface IRepositoryResult
    {
        bool Success { get; set; }
        List<string> Errors { get; set; }
    }

    public class RepositoryResult<T> : IRepositoryResult
    {
        public bool Success { get; set; }
        public T Value { get; set; }
        public List<string> Errors { get; set; }

        public RepositoryResult()
        {
            Errors = new List<string>();
        }
    }

    public class RepositoryResult : IRepositoryResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }

        private RepositoryResult()
        {
            Errors = new List<string>();
        }

        public static RepositoryResult SuccessResult => new RepositoryResult() { Success = true };

        public static RepositoryResult CreateError(params string[] errors)
        {
            return new RepositoryResult { Success = false, Errors = errors.ToList() };
        }

        public static RepositoryResult<T> Create<T>(T item)
        {
            return new RepositoryResult<T>() { Success = true, Value = item };
        }

        public static RepositoryResult<T> CreateError<T>(params string[] errors)
        {
            return new RepositoryResult<T> { Success = false, Errors = errors.ToList() };
        }
    }
}