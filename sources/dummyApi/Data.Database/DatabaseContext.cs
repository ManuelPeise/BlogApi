using Data.Database.Entities;
using Data.Database.Seeds;
using Microsoft.EntityFrameworkCore;

namespace Data.Database
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BlogEntity>()
                .HasMany(b => b.Posts)
                .WithOne(p => p.Blog)
                .HasForeignKey(p => p.BlogId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BlogEntity>()
                .HasMany(b => b.Users)
                .WithOne(u => u.Blog)
                .HasForeignKey(u => u.BlogId);

            modelBuilder.ApplyConfiguration(new CredentialsSeed());
            modelBuilder.ApplyConfiguration(new BlogSeed());
            modelBuilder.ApplyConfiguration(new UserSeed());
           
        }

        public DbSet<UserEntity> UserTable { get; set; }
        public DbSet<BlogEntity> BlogTable { get; set; }
        public DbSet<PostEntity> PostTable { get; set; }
        public DbSet<AddressEntity> AddressTable { get; set; }
        public DbSet<CityEntity> CityTable { get; set; }
        public DbSet<CountryEntity> CountryTable { get; set; }
    }
}
