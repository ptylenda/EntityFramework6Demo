namespace Intel.BikeRental.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeHierarchyTypeForVehiclesToTPC : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("rentals.Rentals", "Vehicle_VehicleId", "rentals.Vehicles");
            DropForeignKey("rentals.Bikes", "VehicleId", "rentals.Vehicles");
            DropForeignKey("rentals.Scooters", "VehicleId", "rentals.Vehicles");
            DropIndex("rentals.Rentals", new[] { "Vehicle_VehicleId" });
            DropIndex("rentals.Bikes", new[] { "VehicleId" });
            DropIndex("rentals.Scooters", new[] { "VehicleId" });
            AddColumn("rentals.Bikes", "Color", c => c.String(maxLength: 20, unicode: false));
            AddColumn("rentals.Bikes", "Number", c => c.String(nullable: false, maxLength: 10, unicode: false));
            AddColumn("rentals.Bikes", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("rentals.Scooters", "Color", c => c.String(maxLength: 20, unicode: false));
            AddColumn("rentals.Scooters", "Number", c => c.String(nullable: false, maxLength: 10, unicode: false));
            AddColumn("rentals.Scooters", "IsActive", c => c.Boolean(nullable: false));
            DropTable("rentals.Vehicles");
        }
        
        public override void Down()
        {
            CreateTable(
                "rentals.Vehicles",
                c => new
                    {
                        VehicleId = c.Int(nullable: false, identity: true),
                        Color = c.String(maxLength: 20, unicode: false),
                        Number = c.String(nullable: false, maxLength: 10, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.VehicleId);
            
            DropColumn("rentals.Scooters", "IsActive");
            DropColumn("rentals.Scooters", "Number");
            DropColumn("rentals.Scooters", "Color");
            DropColumn("rentals.Bikes", "IsActive");
            DropColumn("rentals.Bikes", "Number");
            DropColumn("rentals.Bikes", "Color");
            CreateIndex("rentals.Scooters", "VehicleId");
            CreateIndex("rentals.Bikes", "VehicleId");
            CreateIndex("rentals.Rentals", "Vehicle_VehicleId");
            AddForeignKey("rentals.Scooters", "VehicleId", "rentals.Vehicles", "VehicleId");
            AddForeignKey("rentals.Bikes", "VehicleId", "rentals.Vehicles", "VehicleId");
            AddForeignKey("rentals.Rentals", "Vehicle_VehicleId", "rentals.Vehicles", "VehicleId");
        }
    }
}
