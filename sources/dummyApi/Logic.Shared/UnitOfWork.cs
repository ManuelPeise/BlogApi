using Data.Database;
using Data.Database.Entities;
using Data.Database.Interfaces;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Logic.Shared
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private IHttpContextAccessor _httpContextAccessor;

        public IDbRepositoryBase<UserEntity> UserTable { get; private set; }
        public IDbRepositoryBase<UserCredentialsEntity> UserCredentialsTable { get; private set; }
        public IDbRepositoryBase<BlogEntity>? BlogTable { get; private set; }
        public IDbRepositoryBase<PostEntity> BlogPostTable { get; private set; }
        public IDbRepositoryBase<AddressEntity> AddressTable { get; private set; }
        public IDbRepositoryBase<CityEntity> CityTable { get; private set; }
        public IDbRepositoryBase<CountryEntity> CountryTable { get; private set; }

        public UnitOfWork(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            InitializeRepositories();
        }

        public async Task SaveChanges(string userName = "System")
        {
            var user = _httpContextAccessor?.HttpContext.User.Identity?.Name ?? userName;

            var now = DateTime.UtcNow;

            var entries = _context.ChangeTracker.Entries<AEntityBase>();

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

            await _context.SaveChangesAsync();
        }

        private void InitializeRepositories()
        {
            UserTable = new DbRepositoryBase<UserEntity>(_context, _httpContextAccessor);
            UserCredentialsTable = new DbRepositoryBase<UserCredentialsEntity>(_context, _httpContextAccessor);
            BlogTable = new DbRepositoryBase<BlogEntity>(_context, _httpContextAccessor);
            BlogPostTable = new DbRepositoryBase<PostEntity>(_context, _httpContextAccessor);
            AddressTable = new DbRepositoryBase<AddressEntity>(_context, _httpContextAccessor);
            CityTable = new DbRepositoryBase<CityEntity>(_context, _httpContextAccessor);
            CountryTable = new DbRepositoryBase<CountryEntity>(_context, _httpContextAccessor);
        }
    }
}
