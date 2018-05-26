using Microsoft.EntityFrameworkCore;

namespace RatesSchedule.Models
{
  public class RatesContext : DbContext
  {
    public RatesContext(DbContextOptions<RatesContext> options)
            : base(options)
    {
    }

    protected override void OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<RateItem>().HasOne(p => p.DomainItem).WithOne(p => p.RateItem);
    }

    public DbSet<RateItem> RateItems { get; set; }
    public DbSet<RateDomainItem> RateDomainItems { get; set; }
  }
}
