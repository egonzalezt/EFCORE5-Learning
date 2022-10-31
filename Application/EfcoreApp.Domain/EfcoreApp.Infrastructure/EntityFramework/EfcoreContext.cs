using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EfcoreApp.Domain;
using System;

namespace EfcoreApp.Infrastructure.EntityFramework
{
    public class EfcoreContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<SamuraiBattleStat> SamuraiBattleStats { get; set; }

        public EfcoreContext()
        { }

        public EfcoreContext(DbContextOptions opt)
            : base(opt)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=SamuraiDb;Trusted_Connection=True;",
                options => options.MaxBatchSize(150))
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Samurai>()
                         .HasMany(s => s.Battles)
                         .WithMany(b => b.Samurais)
                         .UsingEntity<BattleSamurai>
                          (bs => bs.HasOne<Battle>().WithMany(),
                           bs => bs.HasOne<Samurai>().WithMany())
                         .Property(bs => bs.DateJoined)
                         .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Horse>().ToTable("Horses");
            modelBuilder.Entity<SamuraiBattleStat>().HasNoKey().ToView("SamuraiBattleStats");
        }

    }
 }
