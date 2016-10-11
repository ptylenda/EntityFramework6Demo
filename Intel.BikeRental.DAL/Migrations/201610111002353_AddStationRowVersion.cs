namespace Intel.BikeRental.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStationRowVersion : DbMigration
    {
        public override void Up()
        {
            AddColumn("rentals.Stations", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("rentals.Stations", "RowVersion");
        }
    }
}
