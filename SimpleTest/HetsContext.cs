﻿using Microsoft.EntityFrameworkCore;

namespace SimpleTest
{
    public partial class HetsContext : DbContext
    {
        private readonly string _connectionString;

        public HetsContext()
        {
        }

        public HetsContext(DbContextOptions<HetsContext> options)
            : base(options)
        {
        }

        public HetsContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_connectionString);
            }
        }        
    }
}
