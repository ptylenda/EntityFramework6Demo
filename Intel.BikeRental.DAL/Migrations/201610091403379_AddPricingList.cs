namespace Intel.BikeRental.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPricingList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "rentals.PricingLists",
                c => new
                    {
                        PricingListKey = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PricingListKey);

            // Loading SQL file from resource and executing it
            SqlResource("Intel.BikeRental.DAL.Scripts.201610091403379_AddPricingList_Up.sql", suppressTransaction: true);            
        }
        
        public override void Down()
        {
            DropTable("rentals.PricingLists");
        }
    }
}
