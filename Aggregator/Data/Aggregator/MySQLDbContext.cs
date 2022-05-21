using Aggregator.Models.Aggregator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Aggregator.Data.Aggregator
{
    public class MySqlDbContext : DbContext
    {
        //================= список таблиц ===================
        public DbSet<Channel> Channels { get; set; }      // каналы
        public DbSet<News> News { get; set; }             // новости

        private readonly string connectionString;

        public MySqlDbContext(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");

            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Channel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
                entity.HasOne(d => d.Channel)
                  .WithMany(p => p.News)
                  .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}