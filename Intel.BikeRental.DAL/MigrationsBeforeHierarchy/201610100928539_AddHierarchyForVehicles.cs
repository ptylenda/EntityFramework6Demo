namespace Intel.BikeRental.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHierarchyForVehicles : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "rentals.Bikes", newName: "Vehicles");
            DropIndex("rentals.Rentals", new[] { "USer_UserKey" });
            AddColumn("rentals.Vehicles", "EngineCapacity", c => c.Int());
            AddColumn("rentals.Vehicles", "MaxSpeed", c => c.Byte());
            AddColumn("rentals.Vehicles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("rentals.Vehicles", "BikeType", c => c.Int());
            CreateIndex("rentals.Rentals", "User_UserKey");
        }
        
        public override void Down()
        {
            DropIndex("rentals.Rentals", new[] { "User_UserKey" });
            AlterColumn("rentals.Vehicles", "BikeType", c => c.Int(nullable: false));
            DropColumn("rentals.Vehicles", "Discriminator");
            DropColumn("rentals.Vehicles", "MaxSpeed");
            DropColumn("rentals.Vehicles", "EngineCapacity");
            CreateIndex("rentals.Rentals", "USer_UserKey");
            RenameTable(name: "rentals.Vehicles", newName: "Bikes");
        }
    }
}
