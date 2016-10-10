namespace Intel.BikeRental.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHierarchyForVehicles2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("rentals.Rentals", "Bike_BikeId", "rentals.Bikes");
            RenameColumn(table: "rentals.Rentals", name: "Bike_BikeId", newName: "Bike_VehicleId");
            RenameIndex(table: "rentals.Rentals", name: "IX_Bike_BikeId", newName: "IX_Bike_VehicleId");
            DropPrimaryKey("rentals.Vehicles", "PK_rentals.Bikes");
            AddColumn("rentals.Vehicles", "VehicleId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("rentals.Vehicles", "VehicleId");
            AddForeignKey("rentals.Rentals", "Bike_VehicleId", "rentals.Vehicles", "VehicleId");
            DropColumn("rentals.Vehicles", "BikeId");
        }
        
        public override void Down()
        {
            AddColumn("rentals.Vehicles", "BikeId", c => c.Int(nullable: false, identity: true));
            DropForeignKey("rentals.Rentals", "Bike_VehicleId", "rentals.Vehicles");
            DropPrimaryKey("rentals.Vehicles");
            DropColumn("rentals.Vehicles", "VehicleId");
            AddPrimaryKey("rentals.Vehicles", "BikeId");
            RenameIndex(table: "rentals.Rentals", name: "IX_Bike_VehicleId", newName: "IX_Bike_BikeId");
            RenameColumn(table: "rentals.Rentals", name: "Bike_VehicleId", newName: "Bike_BikeId");
            AddForeignKey("rentals.Rentals", "Bike_BikeId", "rentals.Vehicles", "BikeId");
        }
    }
}
