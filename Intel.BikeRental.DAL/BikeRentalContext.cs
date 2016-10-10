using Intel.BikeRental.DAL.Configurations;
using Intel.BikeRental.DAL.Conventions;
using Intel.BikeRental.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Intel.BikeRental.DAL
{
    public class BikeRentalContext : DbContext
    {
        public BikeRentalContext()
            : base("BikeRentalConnection")
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<Bike> Bikes { get; set; }

        public DbSet<Station> Stations { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Rental> Rentals { get; set; }

        public DbSet<PricingList> PricingLists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.HasDefaultSchema("rentals");
            
            // Organising and adding custom configurations
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new BikeConfiguration());
            modelBuilder.Configurations.Add(new RentalConfiguration());
            modelBuilder.Configurations.Add(new StationConfiguration());
            
            // Configuring TPT hierarchy
            modelBuilder.Entity<Bike>().ToTable("Bikes");
            modelBuilder.Entity<Scooter>().ToTable("Scooters");

            // Adding custom conventions
            modelBuilder.Conventions.Add(new DateTime2Convention());
            modelBuilder.Conventions.Add(new KeyConvention());

            // Remove some existing default convention
            //modelBuilder.Conventions.Remove<IdKeyDiscoveryConvention>();
        }
    }
}
