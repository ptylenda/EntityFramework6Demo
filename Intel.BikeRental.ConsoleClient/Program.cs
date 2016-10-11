using ConsoleDump;
using Intel.BikeRental.DAL;
using Intel.BikeRental.DAL.Migrations;
using Intel.BikeRental.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Transactions;

namespace Intel.BikeRental.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BikeRentalContext, Configuration>());
            /*AddStationTest();
            AddUserTest();
            AddBikeTest();
            AddRentalTest();
            UpdateBikeTest();
            AddUserAndCompareStateTest();
            DeleteUserTest();
            AttachBikeTest();
            AddVehiclesTest();
            GetVehiclesTest();
            SelectTest();
            GroupByTest();
            ExceptTest();
            SyntaxTest();
            SqlUpdateTest();
            SqlUpdateWithParamUnsafeTest("Red");
            SqlUpdateWithParamUnsafeTest(@"Red'; USE master;
ALTER DATABASE [BikeRentalDb] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [BikeRentalDb] ;
select 'HE HE");
            SqlUpdateWithParamTest("Red");
            SqlUpdateWithParamTest(@"Red'; USE master;
ALTER DATABASE [BikeRentalDb] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [BikeRentalDb] ;
select 'HE HE");
            GetSqlTest();
            ExecuteSpWithOutParameterTest();
            SerializeParametersTest();
            DeserializeParametersTest();
            TransactionTest();
            DistributedTransactionTest();
            ConcurrentWithRowVersionTest();
            UpdateUserViaSpTest();*/


        }

        private static void UpdateUserViaSpTest()
        {
            using (var context = new BikeRentalContext())
            {
                var user = context.Users.First();
                user.FirstName = "Johnny";
                context.SaveChanges();
            }
        }

        private static void ConcurrentWithRowVersionTest()
        {
            using (var context1 = new BikeRentalContext())
            using (var context2 = new BikeRentalContext())
            {
                var station1 = context1.Stations.Find(1);
                station1.Name = "Cmc";

                var station2 = context2.Stations.Find(1);
                station2.Name = "Lmc";

                // Different order
                context2.SaveChanges();

                // Some longer break, just for demo purpose - coffee break
                Thread.Sleep(5000);

                try
                {
                    context1.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine("Hey man, your coffee caused an exception");
                    var entry = ex.Entries.Single();
                    entry.Reload();
                    entry.Entity.Dump("Somebody has just saved the following station:");
                }
            }
        }

        private static void ConcurrentTest()
        {
            using (var context1 = new BikeRentalContext())
            using (var context2 = new BikeRentalContext())
            {
                var user1 = context1.Users.Find(1);
                user1.PhoneNumber = "555-555-555";

                var user2 = context2.Users.Find(1);
                user2.PhoneNumber = "777-777-777";

                // Different order
                context2.SaveChanges();

                // Some longer break, just for demo purpose - coffee break
                Thread.Sleep(5000);

                try
                {
                    context1.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine("Hey man, your coffee caused an exception");
                    var entry = ex.Entries.Single();
                    entry.Reload();
                    entry.Entity.Dump("Somebody has just saved the following user:");
                }
            }
        }

        private static void DistributedTransactionTest()
        {
            var badGuy = new Random();

            try
            {
                using (var scope = new TransactionScope())
                {
                    // First action (on DB1)
                    using (var context = new BikeRentalContext())
                    {
                        var user = new User
                        {
                            FirstName = "Jack",
                            LastName = "Doe"
                        };

                        context.Users.Add(user);
                        context.SaveChanges();
                    }

                    if (badGuy.Next(2) == 0)
                    {
                        throw new Exception("Imma bad guy!");
                    }

                    // Second action (on DB2)
                    using (var context = new BikeRentalContext())
                    {
                        var user = new User
                        {
                            FirstName = "Mick",
                            LastName = "Noe"
                        };

                        context.Users.Add(user);
                        context.SaveChanges();
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void TransactionTest()
        {
            var badGuy = new Random();

            using (var context = new BikeRentalContext())
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    // First action
                    var user = new User
                    {
                        FirstName = "Jack",
                        LastName = "Doe"
                    };

                    context.Users.Add(user);
                    context.SaveChanges();

                    if (badGuy.Next(2) == 0)
                    {
                        throw new Exception("Imma bad guy!");
                    }

                    // Second action
                    var bikeToRent = context.Bikes.First();
                    var station = context.Stations.First();
                    var promoRental = new Rental
                    {
                        StationFrom = station,
                        Cost = 100,
                        DateFrom = DateTime.Now,
                        User = user,
                        Vehicle = bikeToRent
                    };

                    context.Rentals.Add(promoRental);
                    context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex);
                }
            }
        }

        private static void DeserializeParametersTest()
        {
            using (var context = new BikeRentalContext())
            {
                foreach (var station in context.Stations.Where(x => x.SerializedParameters != null))
                {
                    station.Parameters.Dump();
                }
            }
        }

        private static void SerializeParametersTest()
        {
            using (var context = new BikeRentalContext())
            {
                var station = new Station
                {
                    Name = "Station with parameters",
                    Location = new Location(50, 18),
                    Parameters = new Station.SetupParameters
                    {
                        A = 2,
                        B = 3,
                        C = "test param 4"
                    }
                };

                context.Stations.Add(station);
                context.SaveChanges();

                context.Stations.Dump();
            }
        }

        private static void ExecuteSpWithOutParameterTest()
        {
            var proc = @"
            create procedure uspDeleteBikeWithColorOut(@BikeId int, @Color varchar(30) output)
            as begin
	            select @Color = [color] from [rentals].[Vehicles] where [VehicleId] = @BikeId;
	            delete from [rentals].[Vehicles] where [VehicleId] = @BikeId;
            end
            ";

            using (var context = new BikeRentalContext())
            {
                context.Bikes.Add(new Bike
                {
                    BikeType = BikeType.City,
                    Color = "Blue!!!",
                    Number = "B124"
                });

                context.SaveChanges();
                context.Bikes.ToList().Dump();

                var bikeToDelete = context.Bikes.FirstOrDefault(x => x.Number == "B124");

                if (bikeToDelete != null)
                {
                    var idParameter = new SqlParameter("BikeId", bikeToDelete.VehicleId);
                    var outputParameter = new SqlParameter
                    {
                        ParameterName = "Color",
                        DbType = DbType.String,
                        Size = 30,
                        Direction = ParameterDirection.Output
                    };

                    context.Database.ExecuteSqlCommand("uspDeleteBikeWithColorOut @BikeId, @Color OUTPUT", idParameter, outputParameter);

                    context.SaveChanges();
                    outputParameter.Dump();
                    context.Bikes.ToList().Dump();
                }
            }
        }

        private static void ExecuteSpTest()
        {
            var proc = @"
            create procedure uspDeleteBike(@BikeId int)
            as begin
	            delete from [rentals].[Vehicles] where [VehicleId] = @BikeId;
            end
            ";

            using (var context = new BikeRentalContext())
            {
                context.Bikes.Add(new Bike
                {
                    BikeType = BikeType.City,
                    Color = "Test",
                    Number = "B123"
                });

                context.SaveChanges();
                context.Bikes.ToList().Dump();

                var bikeToDelete = context.Bikes.FirstOrDefault(x => x.Number == "B123");

                if (bikeToDelete != null)
                {
                    var idParameter = new SqlParameter("BikeId", bikeToDelete.VehicleId);
                    context.Database.ExecuteSqlCommand("uspDeleteBike @BikeId", idParameter);

                    context.SaveChanges();
                    context.Bikes.ToList().Dump();
                }
            }
        }

        private static void GetSqlTest()
        {
            string sql = $"SELECT * FROM [rentals].[Vehicles] where [Color] like 'r%' and Discriminator = 'Bike';";

            using (var context = new BikeRentalContext())
            {
                var bikes = context.Database.SqlQuery<Bike>(sql);
                bikes.ToList().Dump();
            }
        }

        private static void SqlUpdateWithParamTest(string color)
        {
            string sql = $"update[rentals].[Vehicles] set [IsActive] = 0 where [Color] = @pColor;";
            var colorParameter = new SqlParameter("pColor", color);

            using (var context = new BikeRentalContext())
            {
                context.Database.ExecuteSqlCommand(sql, colorParameter);
            }
        }

        private static void SqlUpdateWithParamUnsafeTest(string color)
        {
            string sql = $"update[rentals].[Vehicles] set [IsActive] = 0 where [Color] = '{color}';";

            using (var context = new BikeRentalContext())
            {
                context.Database.ExecuteSqlCommand(sql);
            }
        }

        private static void SqlUpdateTest()
        {
            string sql = "update[rentals].[Vehicles] set [IsActive] = 0;";

            using (var context = new BikeRentalContext())
            {
                context.Database.ExecuteSqlCommand(sql);                
            }
        }

        private static void SyntaxTest()
        {
            using (var context = new BikeRentalContext())
            {
                var query = (from vehicle in context.Vehicles
                            where vehicle.Color == "Red"
                            orderby vehicle.Number
                            select vehicle)
                            .Where(x => x.Number.Contains("1"));

                Console.WriteLine(query.ToString());
                var unionDistinct = query.Distinct();
                unionDistinct.ToList().Dump();
            }
        }

        private static void ExceptTest()
        {
            using (var context = new BikeRentalContext())
            {
                var vehicle = context.Vehicles.First();
                vehicle.IsActive = true;
                context.SaveChanges();

                var red = context.Vehicles.Where(x => x.Color == "Red");
                red.ToList().Dump("Red:");

                var active = context.Vehicles.Where(x => x.IsActive);
                active.ToList().Dump("Active:");

                var diff = red.Except(active);
                diff.ToList().Dump("Red without active:");

                var union = red.Union(active);
                union.ToList().Dump("Red with active with duplicates:");

                var unionDistinct = red.Union(active).Distinct();
                unionDistinct.ToList().Dump("Red with active without duplicates:");
            }
        }

        private static void GroupByTest()
        {
            using (var context = new BikeRentalContext())
            {
                var groups = context.Vehicles
                    .GroupBy(v => v.Color);

                foreach (var group in groups)
                {
                    Console.WriteLine(group);
                }

                Console.WriteLine(groups.ToString());
                Console.WriteLine();

                var groups2 = context.Vehicles
                    .GroupBy(v => v.Color)
                    .Select(g => new { Color = g.Key, Qty = g.Count() });

                foreach (var group in groups2)
                {
                    Console.WriteLine(group);
                }

                // A really different query will be executed!
                Console.WriteLine(groups2.ToString());
                Console.WriteLine();

                var groups3 = context.Vehicles
                    .GroupBy(v => new { v.Color, v.IsActive })
                    .Select(g => new { Color = g.Key.Color, IsActive = g.Key.IsActive, Qty = g.Count() });

                foreach (var group in groups3)
                {
                    Console.WriteLine(group);
                }
                
                Console.WriteLine(groups3.ToString());
                Console.WriteLine();
            }
        }

        private static void SelectTest()
        {
            using (var context = new BikeRentalContext())
            {
                var vehicles = context.Vehicles.Select(x => new { x.Color, x.Number });
                foreach (var v in vehicles)
                {
                    Console.WriteLine(v);
                }

                Console.WriteLine(vehicles.ToString());
            }
        }

        private static void GetVehiclesTest()
        {
            using (var context = new BikeRentalContext())
            {
                foreach (var v in context.Vehicles)
                {
                    Console.WriteLine($"{v.Number}, {v.GetType().FullName}");
                }
            }
        }

        private static void AddVehiclesTest()
        {
            using (var context = new BikeRentalContext())
            {
                var bike = new Bike
                {
                    BikeType = BikeType.City,
                    Color = "red",
                    Number = "B005"
                };

                var bike2 = new Bike
                {
                    BikeType = BikeType.Moutain,
                    Color = "yellow",
                    Number = "B006"
                };

                var scooter = new Scooter
                {
                    Color = "yellow",
                    Number = "S001",
                    EngineCapacity = 100
                };

                context.Vehicles.Add(bike);
                context.Vehicles.Add(bike2);
                context.Vehicles.Add(scooter);

                context.SaveChanges();
            }
        }

        private static void AttachBikeTest()
        {
            using (var context = new BikeRentalContext())
            {
                // Either use AsNoTracking or Detach later. Otherwise these bike and bike2 will collide when attaching
                var bike = context.Bikes.AsNoTracking().First(x => x.VehicleId == 1);

                // deserialization simulation, similar to App -> WS -> App -> WS
                var bike2 = new Bike
                {
                    VehicleId = bike.VehicleId,
                    BikeType = bike.BikeType,
                    Color = bike.Color,
                    IsActive = bike.IsActive,
                    Number = bike.Number
                };

                // Changing deserialized object
                bike2.Number = "X123";

                Console.WriteLine(context.Entry(bike2).State);
                context.Bikes.Attach(bike2);
                Console.WriteLine(context.Entry(bike2).State);

                // This would be not enough here. It is still only in state Unchanged. Setting state to Modified.
                context.Entry(bike2).State = EntityState.Modified;
                Console.WriteLine(context.Entry(bike2).State);

                // Forcing only "Number" property to be treated as modified. This results in better SQL code generation.
                // Another way is to use EntityFramework.Extended
                context.Entry(bike2).State = EntityState.Unchanged;
                context.Entry(bike2).Property(x => x.Number).IsModified = true;
                Console.WriteLine(context.Entry(bike2).State);
                
                context.SaveChanges();
            }
        }

        private static void UpdateBikeTest()
        {
            using (var context = new BikeRentalContext())
            {
                var bike = context.Bikes.AsNoTracking().First();
                bike.Color = "yellow!";
                //context.Entry(bike).State = EntityState.Added;
                foreach (var e in context.ChangeTracker.Entries<Bike>())
                {
                    Console.WriteLine(e.Entity);
                }

                context.SaveChanges();
            }
        }

        private static void TryLoadingNonExistentBikeTest()
        {
            using (var context = new BikeRentalContext())
            {
                var bike = context.Bikes.Find(15); // checking how non existent row will be retrieved
                // no exception
            }
        }

        private static void AddUserAndCompareStateTest()
        {
            using (var context = new BikeRentalContext())
            {
                var user = new User
                {
                    FirstName = "!!!",
                    LastName = "???"
                };

                Console.WriteLine(context.Entry(user).State);
                context.Users.Add(user);
                Console.WriteLine(context.Entry(user).State);
                context.SaveChanges();
            }
        }

        private static void DeleteUserTest()
        {
            using (var context = new BikeRentalContext())
            {
                var user = context.Users.Find(1);

                Console.WriteLine(context.Entry(user).State);
                context.Users.Remove(user);
                Console.WriteLine(context.Entry(user).State);

                context.SaveChanges();
            }
        }

        private static void AddBikeTest()
        {
            using (var context = new BikeRentalContext())
            {
                var bike = new Bike
                {
                    BikeType = BikeType.City,
                    Color = "Blue",
                    Number = "R001",
                    IsActive = true
                };

                context.Bikes.Add(bike);
                context.SaveChanges();
            }
        }

        private static void AddUserTest()
        {
            var users = new List<User>
            {
                new User { FirstName = "a", LastName = "b" },
                new User { FirstName = "c", LastName = "d" },
            };

            using (var context = new BikeRentalContext())
            {
                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }

        private static void AddStationTest()
        {
            var stations = new List<Station>
            {
                new Station { Name = "ST001", Capacity = 10, Location = new Location(10, 20) },
                new Station { Name = "ST002", Capacity = 10, Location = new Location(11, 20) },
                new Station { Name = "ST003", Capacity = 10, Location = new Location(12, 20) }
            };

            using (var context = new BikeRentalContext())
            {
                context.Stations.AddRange(stations);
                context.SaveChanges();
            }
        }

        private static void AddRentalTest()
        {
            using (var context = new BikeRentalContext())
            {
                var bike = context.Bikes.Find(1);
                var station = context.Stations.First(x => x.Name == "ST001");
                var user = context.Users.First();

                var rental = new Rental
                {
                    Vehicle = bike,
                    StationFrom = station,
                    User = user,
                    DateFrom = DateTime.Now
                };

                context.Rentals.Add(rental);
                context.SaveChanges();
            }
        }
    }
}
