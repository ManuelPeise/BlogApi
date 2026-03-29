using Data.Database.Entities;
using Data.Database.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Database
{
    public class DbRepositoryBase<TEntity> : IDbRepositoryBase<TEntity> where TEntity : AEntityBase
    {
        private readonly DatabaseContext _dbContext;
        public readonly HttpContext _httpContext;

        public DbRepositoryBase(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = context;
            _httpContext = httpContextAccessor.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public IQueryable<TEntity> GetAll(bool asNoTracking, params Expression<Func<TEntity, object>>[]? includes)
        {
            var table = asNoTracking ? _dbContext.Set<TEntity>().AsNoTracking() : _dbContext.Set<TEntity>();
            
            if (includes is { Length: > 0 })
            {
                foreach (var include in includes)
                {
                    table = table.Include(include);
                }
            }

            return table;
        }

        public async Task<TEntity?> GetByIdAsync(
             bool asNoTracking,
             int id,
             params Expression<Func<TEntity, object>>[]? includes)
        {
            IQueryable<TEntity> query = asNoTracking
                ? _dbContext.Set<TEntity>().AsNoTracking()
                : _dbContext.Set<TEntity>();

            if (includes is { Length: > 0 })
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<TEntity?> QueryData(bool asNoTracking, Expression<Func<TEntity, bool>>? whereExpression, params Expression<Func<TEntity, object>>[]? includes)
        {
            var table = asNoTracking ? _dbContext.Set<TEntity>().AsNoTracking() : _dbContext.Set<TEntity>();

            if(whereExpression != null)
            {
                table = table.Where(whereExpression);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    table = table.Include(include);
                }
            }

            return await table.FirstOrDefaultAsync() ?? null;
        }

        public async Task AddAsync(TEntity entity)
        {
            var table = _dbContext.Set<TEntity>();

            await table.AddAsync(entity);
            
        }

        public async Task UpdateAsync(bool asNoTracking, TEntity entity)
        {
            var table = asNoTracking ? _dbContext.Set<TEntity>().AsNoTracking().AsQueryable() : _dbContext.Set<TEntity>().AsQueryable();

           await table.ExecuteUpdateAsync(e => e.SetProperty(p => p, entity));
        }

        public Task DeleteAsync(int id)
        {
            var table = _dbContext.Set<TEntity>();

            return table.Where(e => e.Id == id).ExecuteDeleteAsync();
        }

        public async Task SaveChanges(string userName = "System")
        {
            var user = _httpContext.User.Identity?.Name ?? userName;

            var now = DateTime.UtcNow;

            var entries = _dbContext.ChangeTracker.Entries<AEntityBase>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.CreatedBy = user;
                    entry.Entity.UpdatedBy = user;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(AEntityBase.CreatedAt)).IsModified = false;
                    entry.Property(nameof(AEntityBase.CreatedBy)).IsModified = false;

                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = user;
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
