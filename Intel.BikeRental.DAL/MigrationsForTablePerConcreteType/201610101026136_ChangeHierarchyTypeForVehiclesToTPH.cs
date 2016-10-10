namespace Intel.BikeRental.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeHierarchyTypeForVehiclesToTPH : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "rentals.Bikes", newName: "Vehicles");
            DropPrimaryKey("rentals.Vehicles");
            AddColumn("rentals.Vehicles", "EngineCapacity", c => c.Int());
            AddColumn("rentals.Vehicles", "MaxSpeed", c => c.Byte());
            AddColumn("rentals.Vehicles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("rentals.Vehicles", "VehicleId", c => c.Int(nullable: false, identity: true));
            AlterColumn("rentals.Vehicles", "BikeType", c => c.Int());
            AddPrimaryKey("rentals.Vehicles", "VehicleId");
            CreateIndex("rentals.Rentals", "Vehicle_VehicleId");
            AddForeignKey("rentals.Rentals", "Vehicle_VehicleId", "rentals.Vehicles", "VehicleId");
            DropTable("rentals.Scooters");
        }
        
        public override void Down()
        {
            CreateTable(
                "rentals.Scooters",
                c => new
                    {
                        VehicleId = c.Int(nullable: false),
                        Color = c.String(maxLength: 20, unicode: false),
                        Number = c.String(nullable: false, maxLength: 10, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        EngineCapacity = c.Int(nullable: false),
                        MaxSpeed = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.VehicleId);
            
            DropForeignKey("rentals.Rentals", "Vehicle_VehicleId", "rentals.Vehicles");
            DropIndex("rentals.Rentals", new[] { "Vehicle_VehicleId" });
            DropPrimaryKey("rentals.Vehicles");
            AlterColumn("rentals.Vehicles", "BikeType", c => c.Int(nullable: false));
            AlterColumn("rentals.Vehicles", "VehicleId", c => c.Int(nullable: false));
            DropColumn("rentals.Vehicles", "Discriminator");
            DropColumn("rentals.Vehicles", "MaxSpeed");
            DropColumn("rentals.Vehicles", "EngineCapacity");
            AddPrimaryKey("rentals.Vehicles", "VehicleId");
            RenameTable(name: "rentals.Vehicles", newName: "Bikes");
        }
    }
}
