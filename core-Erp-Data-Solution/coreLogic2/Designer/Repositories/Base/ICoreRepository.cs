using System;
namespace coreLogic
{
    public interface ICoreRepository<TEntity>
     where TEntity : class
    {
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        global::System.Collections.Generic.IEnumerable<TEntity> Get(global::System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, Func<global::System.Linq.IQueryable<TEntity>, global::System.Linq.IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        TEntity GetByID(object id);
        void Insert(TEntity entity);
        void Save();
        void Update(TEntity entityToUpdate);
    }
}
