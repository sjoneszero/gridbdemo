using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using GridBeyondDatabase.Models;

namespace GridBeyondDatabase
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext() { }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        public virtual DbSet<MarketPrice> MarketPrices { get; set; }
        public virtual DbSet<MarketPriceDataSet> MarketPriceDataSets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string appSettingsPath = Directory.GetCurrentDirectory() + @"\..\GridBeyondDemo\";

                var configuration = new ConfigurationBuilder()
                   .SetBasePath(appSettingsPath)
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                   .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                   .Build();
                var connectionString = configuration.GetConnectionString("MarketDataConnection");

                optionsBuilder.UseSqlServer(
                    connectionString,
                    optionsBuilder => optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                 );
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MarketPrice>(entity =>
            {
                entity.Property(e => e.TimeStamp)
                    .IsRequired();

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasColumnType("DECIMAL(10, 2)"); 
            });
        }
    }
}
