using Intel.BikeRental.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Intel.BikeRental.ConsoleClientAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            /*NoTaskTest();*/

            AsyncTest().Wait();
        }

        private static async Task AsyncTest()
        {
            var task1 = CalculateAsync();
            var task2 = CalculateAsync();
            await Task.WhenAll(task1, task2);
            Console.WriteLine(task1.Result + task2.Result);

            using (var context = new BikeRentalContext())
            {
                var user = await context.Users.FirstAsync();
                user.FirstName = "Johnny2";
                await context.SaveChangesAsync();
            }

            Console.WriteLine("Saved Johnny2");
        }

        private static void NoTaskTest()
        {
            var result1 = Calculate();
            var result2 = Calculate();

            Console.WriteLine(result1 + result2);
        }

        private static Task<decimal> CalculateTask()
        {
            return Task.Factory.StartNew(Calculate);
        }

        private static Task DoWorkTask()
        {
            return Task.Factory.StartNew(DoWork);
        }

        private static void DoWork()
        {
            Console.WriteLine("Working...");
            Task.Delay(5000);
            Console.WriteLine("Done!");
        }
        
        private static decimal Calculate()
        {
            Console.WriteLine("Calculating...");
            Task.Delay(5000);
            Console.WriteLine("Done!");
            return 100.06m;
        }

        private static async Task DoWorkAsync()
        {
            Console.WriteLine("Working...");
            await Task.Delay(5000);
            Console.WriteLine("Done!");
        }

        private static async Task<decimal> CalculateAsync()
        {
            Console.WriteLine("Calculating...");
            await Task.Delay(5000);
            Console.WriteLine("Done!");
            return 100.06m;
        }
    }
}
