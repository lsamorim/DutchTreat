using DutchTreat.Data.Entities;
using System.Collections.Generic;

namespace DutchTreat.Data.Repository
{
    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>
        where TEntity : Entity
    {
        void Create(TEntity entity, string createdBy = null);

        void Create(IEnumerable<TEntity> entities, string createdBy = null);

        void Update(TEntity entity, string updatedBy = null);

        void Delete(object id);

        void Delete(TEntity entity);
    }
}
