using Intel.BikeRental.DAL;
//using Intel.BikeRental.DAL.Migrations;
using Intel.BikeRental.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Intel.BikeRental.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<BikeRentalContext, Configuration>());
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
            SelectTest();*/

            GroupByTest();
        }

        private static void GroupByTest()
        {
            using (var context = new BikeRentalContext())
            {
                var groups = context.Vehicles.GroupBy(v => v.Color);
                foreach (var group in groups)
                {
                    Console.WriteLine(group);
                }

                Console.WriteLine(groups.ToString());
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
