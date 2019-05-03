using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;

namespace SimpleTest
{
    class Program
    {
        static int _seconds = 30;
        static void Main(string[] args)
        {
            var context = new HetsContext(GetConnectionString());

            Console.WriteLine("Test started.");
            UpdateSurname(context);
            Console.WriteLine("Test finished and sleeping 1 hour.");
            
            Thread.Sleep(_seconds * 60 * 1000);
        }

        static void UpdateSurname(HetsContext context)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                var surname = "CHUNG-" + DateTime.Now.ToString("hh:mm:ss");

                Console.WriteLine($"Updating surname to {surname}.");
                var count = context.Database.ExecuteSqlCommand($@"
                    UPDATE ""HET_USER"" 
                    SET ""SURNAME"" = {surname}, 
                        ""CONCURRENCY_CONTROL_NUMBER"" = ""CONCURRENCY_CONTROL_NUMBER"" + 1 
                    WHERE ""SM_USER_ID"" = 'YCHUNG' 
                ");

                Console.WriteLine($"Sleeping {_seconds} seconds.");
                Thread.Sleep(_seconds * 1000);

                transaction.Commit();
            }
        }

        static string GetConnectionString()
        {
            string host = Environment.GetEnvironmentVariable("DATABASE_SERVICE_NAME");
            string username = Environment.GetEnvironmentVariable("POSTGRESQL_USER");
            string password = Environment.GetEnvironmentVariable("POSTGRESQL_PASSWORD");
            string database = Environment.GetEnvironmentVariable("POSTGRESQL_DATABASE");
            string port = Environment.GetEnvironmentVariable("POSTGRESQL_PORT");

            return $"Host={host};Username={username};Password={password};Port={port};Database={database};";
        }
    }
}
