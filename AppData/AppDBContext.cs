using Microsoft.EntityFrameworkCore;
using MyProject.Models;

namespace MyProject.AppData
{
    public class AppDBContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<LogTime> LogTimes { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogTime>()
                .HasOne(c => c.Account)
                .WithMany(t => t.LogTimes)
                .HasForeignKey(c => c.AccountId);

            modelBuilder.Entity<LogTime>()
                .HasOne(c => c.ActivityType)
                .WithMany(t => t.LogTimes)
                .HasForeignKey(c => c.ActivityTypeId);
        }
    }
}

