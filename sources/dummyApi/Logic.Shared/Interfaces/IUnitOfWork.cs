using Data.Database.Entities;
using Data.Database.Interfaces;

namespace Logic.Shared.Interfaces
{
    public interface IUnitOfWork
    {
        IDbRepositoryBase<UserEntity> UserTable { get; }
        IDbRepositoryBase<UserCredentialsEntity> UserCredentialsTable { get; }
        IDbRepositoryBase<BlogEntity> BlogTable { get; }
        IDbRepositoryBase<PostEntity> BlogPostTable { get; }
        IDbRepositoryBase<AddressEntity> AddressTable { get; }
        IDbRepositoryBase<CityEntity> CityTable { get; }
        IDbRepositoryBase<CountryEntity> CountryTable { get; }

        Task SaveChanges(string userName = "System");
    }
}
