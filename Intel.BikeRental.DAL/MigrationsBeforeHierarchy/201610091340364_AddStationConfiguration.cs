namespace Intel.BikeRental.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStationConfiguration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("rentals.Stations", "Name", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("rentals.Stations", "Name", c => c.String());
        }
    }
}
