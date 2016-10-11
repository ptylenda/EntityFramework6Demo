namespace Intel.BikeRental.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStationParameters : DbMigration
    {
        public override void Up()
        {
            AddColumn("rentals.Stations", "Parameters", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("rentals.Stations", "Parameters");
        }
    }
}
