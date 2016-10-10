﻿using Intel.BikeRental.DAL.Configurations;
using Intel.BikeRental.DAL.Conventions;
using Intel.BikeRental.Models;
using System.ComponentModel.DataAnnotations.Schema;
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
            //modelBuilder.Entity<Bike>().ToTable("Bikes");
            //modelBuilder.Entity<Scooter>().ToTable("Scooters");

            // Configuring TPC hierarchy (unfortunately it gives PK without identity... doesn't work properly)

            modelBuilder.Entity<Vehicle>()
                .Property(c => c.VehicleId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Bike>().Map(x =>
            {
                x.MapInheritedProperties();
                x.ToTable("Bikes");
            });
            modelBuilder.Entity<Scooter>().Map(x =>
            {
                x.MapInheritedProperties();
                x.ToTable("Scooters");
            });

            // Adding custom conventions
            modelBuilder.Conventions.Add(new DateTime2Convention());
            modelBuilder.Conventions.Add(new KeyConvention());

            // Remove some existing default convention
            //modelBuilder.Conventions.Remove<IdKeyDiscoveryConvention>();
        }
    }
}
