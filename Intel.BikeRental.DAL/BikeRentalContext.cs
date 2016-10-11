using Intel.BikeRental.DAL.Configurations;
using Intel.BikeRental.DAL.Conventions;
using Intel.BikeRental.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace Intel.BikeRental.DAL
{
    public class BikeRentalContext : DbContext
    {
        public BikeRentalContext()
            : base("BikeRentalConnection")
        {
            this.ObjectContext.ObjectMaterialized += HandleStationParametersDeserialization;
        }

        public ObjectContext ObjectContext
        {
            get
            {
                return ((IObjectContextAdapter)this).ObjectContext;
            }
        }
        
        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<Bike> Bikes { get; set; }

        public DbSet<Station> Stations { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Rental> Rentals { get; set; }

        public DbSet<PricingList> PricingLists { get; set; }
        
        public override int SaveChanges()
        {
            // Custom serialization for Stations
            var stations = this.ChangeTracker.Entries<Station>()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var station in stations)
            {
                station.Entity.SerializedParameters = JsonConvert.SerializeObject(station.Entity.Parameters);
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.HasDefaultSchema("rentals");
            
            // Organising and adding custom configurations
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new BikeConfiguration());
            modelBuilder.Configurations.Add(new RentalConfiguration());
            modelBuilder.Configurations.Add(new StationConfiguration());

            // Complex types
            //modelBuilder.ComplexType<Location>();

            // Ignore type
            //modelBuilder.Ignore<Location>();

            // Configuring TPT hierarchy
            //modelBuilder.Entity<Bike>().ToTable("Bikes");
            //modelBuilder.Entity<Scooter>().ToTable("Scooters");

            // Configuring TPC hierarchy (unfortunately it gives PK without identity... doesn't work properly)
            /*modelBuilder.Entity<Vehicle>()
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
            });*/

            // Adding custom conventions
            modelBuilder.Conventions.Add(new DateTime2Convention());
            modelBuilder.Conventions.Add(new KeyConvention());

            // Remove some existing default convention
            //modelBuilder.Conventions.Remove<IdKeyDiscoveryConvention>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.ObjectContext.ObjectMaterialized -= HandleStationParametersDeserialization;
            }

            base.Dispose(disposing);
        }

        private void HandleStationParametersDeserialization(object sender, ObjectMaterializedEventArgs e)
        {
            // Custom deserialization for Stations
            var station = e.Entity as Station;
            if (station != null)
            {
                station.Parameters = JsonConvert.DeserializeObject<Station.SetupParameters>(station.SerializedParameters);
            }
        }
    }
}
