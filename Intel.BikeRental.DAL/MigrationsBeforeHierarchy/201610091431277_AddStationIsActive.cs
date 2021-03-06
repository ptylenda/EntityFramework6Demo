namespace Intel.BikeRental.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStationIsActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("rentals.Stations", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("rentals.Stations", "IsActive");
        }
    }
}
