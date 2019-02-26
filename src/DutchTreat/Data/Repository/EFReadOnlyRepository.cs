using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;

namespace DutchTreat.Data.Repository
{
    public class EFReadOnlyRepository<TEntity, TContext> : IReadOnlyRepository<TEntity>
        where TEntity : Entity
        where TContext : DbContext
    {
        protected readonly TContext context;

        public EFReadOnlyRepository(TContext context)
        {
            this.context = context;
        }

        protected virtual IQueryable<TEntity> GetQueryable(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
        {
            includeProperties = includeProperties ?? string.Empty;
            IQueryable<TEntity> query = context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        public IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return GetQueryable(null, orderBy, includeProperties, skip, take).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return await GetQueryable(null, orderBy, includeProperties, skip, take).ToListAsync();
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return GetQueryable(filter, orderBy, includeProperties, skip, take).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return await GetQueryable(filter, orderBy, includeProperties, skip, take).ToListAsync();
        }

        public TEntity GetOne(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null)
        {
            return GetQueryable(filter, null, includeProperties).SingleOrDefault();
        }

        public async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null)
        {
            return await GetQueryable(filter, null, includeProperties).SingleOrDefaultAsync();
        }

        public TEntity GetFirst(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "")
        {
            return GetQueryable(filter, orderBy, includeProperties).FirstOrDefault();
        }

        public async Task<TEntity> GetFirstAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null)
        {
            return await GetQueryable(filter, orderBy, includeProperties).FirstOrDefaultAsync();
        }

        public TEntity GetById(object id)
        {
            return context.Set<TEntity>().Find(id);
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public int GetCount(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQueryable(filter).Count();
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return await GetQueryable(filter).CountAsync();
        }

        public bool GetExists(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQueryable(filter).Any();
        }

        public async Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return await GetQueryable(filter).AnyAsync();
        }
    }
}
