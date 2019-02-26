using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DutchTreat.Data.Repository
{
    public class EFRepository<TEntity, TContext> : EFReadOnlyRepository<TEntity, TContext>, IRepository<TEntity>
        where TEntity : Entity
        where TContext : DbContext
    {
        public EFRepository(TContext context) : base(context)
        {
        }

        public void Create(TEntity entity, string createdBy = null)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = createdBy;
            context.Set<TEntity>().Add(entity);
        }

        public void Create(IEnumerable<TEntity> entities, string createdBy = null)
        {
            foreach(var e in entities)
            {
                e.CreatedAt = DateTime.UtcNow;
                e.CreatedBy = createdBy;
            }
            context.Set<TEntity>().AddRange(entities);
        }

        public void Delete(object id)
        {
            var entity = context.Set<TEntity>().Find(id);
            Delete(entity);
        }

        public void Delete(TEntity entity)
        {
            var dbSet = context.Set<TEntity>();
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public void Update(TEntity entity, string updatedBy = null)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = updatedBy;
            context.Set<TEntity>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
