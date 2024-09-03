using System.Linq.Expressions;

namespace DentalSys.Api.Common.Interfaces
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : class
    {
        Task<TKey> CreateAsync(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);
        Task<int> DeleteAsync(TEntity entity);
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>?> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null);
    }
}
