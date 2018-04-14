using BitRippleRepository.Context;
using BitRippleRepository.Table;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BitRippleRepository
{
	public class Entity<TEntity> where TEntity : class, IDalEntity
	{
		protected readonly BitRippleContext _context;
		protected readonly DbSet<TEntity> _dbSet;

		public Entity(BitRippleContext context, DbSet<TEntity> dbSet)
		{
			_context = context;
			_dbSet = dbSet;
		}

		public IQueryable<TEntity> Entities => _dbSet.AsNoTracking();

		public void Insert(TEntity entity)
		{
			Insert(new[] { entity });
		}

		public void Insert(IEnumerable<TEntity> entities)
		{
			if (entities == null)
			{
				throw new ArgumentNullException();
			}

			_dbSet.AddRange(entities);
			_context.SaveChanges();
		}

		public void Update(TEntity entity)
		{
			Update(new[] { entity });
		}

		public void Update(IEnumerable<TEntity> entities)
		{
			if (entities == null)
			{
				throw new ArgumentNullException();
			}

			_dbSet.UpdateRange(entities);
			_context.SaveChanges();
		}

		public void Remove(IEnumerable<TEntity> entities)
		{
			if (entities == null)
			{
				throw new ArgumentNullException();
			}

			_dbSet.RemoveRange(entities);
			_dbSet.AttachRange(entities);
			_context.SaveChanges();
		}

		public void Remove(TEntity entity)
		{
			Remove(new[] { entity });
		}

		public void Remove(int id)
		{
			Remove(x => x.Id == id);
		}

		public void Remove(IEnumerable<int> ids)
		{
			if (ids == null)
			{
				throw new ArgumentNullException();
			}

			Remove(x => ids.Contains(x.Id));
		}

		public void Remove(Expression<Func<TEntity, bool>> condition)
		{
			if (condition == null)
			{
				throw new ArgumentNullException();
			}

			var entities = _dbSet.Where(condition).ToList();
			_dbSet.RemoveRange(entities);
			_context.SaveChanges();
		}
	}
}