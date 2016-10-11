namespace Intel.BikeRental.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameUserStoredProcedures : DbMigration
    {
        public override void Up()
        {
            RenameStoredProcedure(name: "rentals.User_Insert", newName: "FancyUserInsert");
            RenameStoredProcedure(name: "rentals.User_Update", newName: "FancyUserUpdate");
            RenameStoredProcedure(name: "rentals.User_Delete", newName: "FancyUserDelete");
        }
        
        public override void Down()
        {
            RenameStoredProcedure(name: "rentals.FancyUserDelete", newName: "User_Delete");
            RenameStoredProcedure(name: "rentals.FancyUserUpdate", newName: "User_Update");
            RenameStoredProcedure(name: "rentals.FancyUserInsert", newName: "User_Insert");
        }
    }
}
