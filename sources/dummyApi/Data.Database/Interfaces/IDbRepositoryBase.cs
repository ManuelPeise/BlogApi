using System.Linq.Expressions;

namespace Data.Database.Interfaces
{
    public interface IDbRepositoryBase<TEntity> where TEntity : Entities.AEntityBase
    {
        IQueryable<TEntity> GetAll(bool asNoTracking, params Expression<Func<TEntity, object>>[]? includes);
        Task<TEntity?> GetByIdAsync(bool asNoTracking, int id, params Expression<Func<TEntity, object>>[]? includes);
        Task<TEntity?> QueryData(bool asNoTracking, Expression<Func<TEntity, bool>>? whereExpression, params Expression<Func<TEntity, object>>[]? includes);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(bool asNoTracking, TEntity entity);
        Task DeleteAsync(int id);
        Task DeleteRangeAsync(IEnumerable<int> ids);
        Task SaveChanges(string userName = "System");
    }
}
