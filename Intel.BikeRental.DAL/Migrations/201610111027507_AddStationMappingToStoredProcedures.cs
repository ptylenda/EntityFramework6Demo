namespace Intel.BikeRental.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStationMappingToStoredProcedures : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "rentals.User_Insert",
                p => new
                    {
                        UserId = p.Int(),
                        FirstName = p.String(maxLength: 50),
                        LastName = p.String(maxLength: 50),
                        PhoneNumber = p.String(),
                        IsActive = p.Boolean(),
                    },
                body:
                    @"INSERT [rentals].[Users]([UserId], [FirstName], [LastName], [PhoneNumber], [IsActive])
                      VALUES (@UserId, @FirstName, @LastName, @PhoneNumber, @IsActive)
                      
                      DECLARE @UserKey int
                      SELECT @UserKey = [UserKey]
                      FROM [rentals].[Users]
                      WHERE @@ROWCOUNT > 0 AND [UserKey] = scope_identity()
                      
                      SELECT t0.[UserKey]
                      FROM [rentals].[Users] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[UserKey] = @UserKey"
            );
            
            CreateStoredProcedure(
                "rentals.User_Update",
                p => new
                    {
                        UserKey = p.Int(),
                        UserId = p.Int(),
                        FirstName = p.String(maxLength: 50),
                        LastName = p.String(maxLength: 50),
                        PhoneNumber = p.String(),
                        PhoneNumber_Original = p.String(),
                        IsActive = p.Boolean(),
                    },
                body:
                    @"UPDATE [rentals].[Users]
                      SET [UserId] = @UserId, [FirstName] = @FirstName, [LastName] = @LastName, [PhoneNumber] = @PhoneNumber, [IsActive] = @IsActive
                      WHERE (([UserKey] = @UserKey) AND (([PhoneNumber] = @PhoneNumber_Original) OR ([PhoneNumber] IS NULL AND @PhoneNumber_Original IS NULL)))"
            );
            
            CreateStoredProcedure(
                "rentals.User_Delete",
                p => new
                    {
                        UserKey = p.Int(),
                        PhoneNumber_Original = p.String(),
                    },
                body:
                    @"DELETE [rentals].[Users]
                      WHERE (([UserKey] = @UserKey) AND (([PhoneNumber] = @PhoneNumber_Original) OR ([PhoneNumber] IS NULL AND @PhoneNumber_Original IS NULL)))"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("rentals.User_Delete");
            DropStoredProcedure("rentals.User_Update");
            DropStoredProcedure("rentals.User_Insert");
        }
    }
}
