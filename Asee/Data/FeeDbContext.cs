using Asee.Models.Domain;
using Asee.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Asee.Data
{
    public class FeeDbContext : DbContext
    {
        public FeeDbContext(DbContextOptions<FeeDbContext> options) : base(options)
        {
        }

        public DbSet<FeeCalculationHistory> FeeHistories { get; set; }
        public DbSet<FeeRules> FeeRules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FeeCalculationHistory>().ToTable("FeeCalculationHistory");
            modelBuilder.Entity<FeeRules>().ToTable("FeeRules");

            modelBuilder.Entity<FeeRules>().HasData(
                new FeeRules
                {
                    RuleId = "1",
                    RuleType = "POS",
                    Description = "Fixed fee for POS transactions",
                    Criteria = "{\"maxAmount\": 100}",
                    Action = "fixed_fee",
                    Amount = 0.2m,
                    IsActive = true
                },
                new FeeRules
                {
                    RuleId = "2",
                    RuleType = "E-commerce",
                    Description = "Percentage fee for e-commerce transactions",
                    Criteria = "{\"minAmount\": 10}",
                    Action = "percentage_fee",
                    Amount = 0.18m,
                    IsActive = true
                }
            );
        }
    }
}
