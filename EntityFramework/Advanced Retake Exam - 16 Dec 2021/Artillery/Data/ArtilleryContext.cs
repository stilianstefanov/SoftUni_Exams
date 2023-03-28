namespace Artillery.Data
{
    using Artillery.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    public class ArtilleryContext : DbContext
    {
        public ArtilleryContext() 
        { 
        }

        public ArtilleryContext(DbContextOptions options)
            : base(options) 
        { 
        }

        public virtual DbSet<Country> Countries { get; set; } = null!;

        public virtual DbSet<CountryGun> CountriesGuns { get; set; } = null!;

        public virtual DbSet<Gun> Guns { get; set; } = null!;

        public virtual DbSet<Manufacturer> Manufacturers { get; set; } = null!;

        public virtual DbSet<Shell> Shells { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CountryGun>(entity =>
            {
                entity.HasKey(e => new { e.CountryId, e.GunId });
            });
        }
    }
}
