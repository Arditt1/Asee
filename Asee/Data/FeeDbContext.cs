using Microsoft.EntityFrameworkCore;
using Asee.Models.Entities;

namespace Asee.Data
{
    public class FeeDbContext : DbContext
    {
        public FeeDbContext(DbContextOptions<FeeDbContext> options) : base(options)
        {
        }

        public DbSet<FeeCalculationHistory> FeeHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FeeCalculationHistory>().ToTable("FeeCalculationHistory");
        }
    }
}
