using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;

namespace SimpleInsert
{
    class Program
    {
        static int _seconds = 30;
        static void Main(string[] args)
        {
            var context = new HetsContext(GetConnectionString());

            UpdateSurname(context);            
        }

        static void UpdateSurname(HetsContext context)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                context.Database.ExecuteSqlCommand(@"LOCK TABLE ""HET_USER"" IN EXCLUSIVE MODE;");

                var surname = "CHUNG" + DateTime.Now.ToString("hh:MM:ss");

                var count = context.Database.ExecuteSqlCommand($@"
                    UPDATE ""HET_USER"" 
                    SET ""SURNAME"" = {surname}, 
                        ""CONCURRENCY_CONTROL_NUMBER"" = ""CONCURRENCY_CONTROL_NUMBER"" + 1 
                    WHERE ""SM_USER_ID"" = 'YCHUNG' 
                ");

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

            return $"Host={host};Username={username};Password={password};Database={database};";
        }
    }
}
