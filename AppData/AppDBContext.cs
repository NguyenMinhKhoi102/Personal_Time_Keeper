using Microsoft.EntityFrameworkCore;
using MyProject.Models;

namespace MyProject.AppData
{
    public class AppDBContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<LogTimeSummary> LogTimeSummaries { get; set; }
        public DbSet<LogTime> LogTimes { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogTimeSummary>()
                .HasOne(c => c.Account)
                .WithMany(t => t.LogTimeSummaries)
                .HasForeignKey(c => c.AccountId);

            modelBuilder.Entity<LogTime>()
                .HasOne(c => c.LogTimeSummary)
                .WithMany(t => t.LogTimes)
                .HasForeignKey(c => c.LogTimeSummaryId);

            modelBuilder.Entity<LogTime>()
                .HasOne(c => c.ActivityType)
                .WithMany(t => t.LogTimes)
                .HasForeignKey(c => c.ActivityTypeId);
        }
    }
}

