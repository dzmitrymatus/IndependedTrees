using System.Linq.Expressions;

namespace IndependedTrees.DAL.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> filterExpression);
        Task<TEntity?> GetAsync(object id);
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
