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
                .HasOne(b => b.User)
                .WithMany(u => u.Blogs);

            
            modelBuilder.ApplyConfiguration(new AddressSeed());
            modelBuilder.ApplyConfiguration(new CitySeed());
            modelBuilder.ApplyConfiguration(new CountrySeed());
            modelBuilder.ApplyConfiguration(new CredentialsSeed());
            modelBuilder.ApplyConfiguration(new UserSeed());
           
        }

        public DbSet<UserEntity> UserTable { get; set; }
        public DbSet<BlogEntity> BlogTable { get; set; }
        public DbSet<PostEntity> PostTable { get; set; }
        public DbSet<AddressEntity> AddressTable { get; set; }
        public DbSet<CityEntity> CityTable { get; set; }
        public DbSet<CityEntity> CountryTable { get; set; }
    }
}
