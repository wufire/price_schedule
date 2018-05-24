using Microsoft.EntityFrameworkCore;

namespace RatesSchedule.Models
{
  public class RatesContext : DbContext
  {
    public RatesContext(DbContextOptions<RatesContext> options)
            : base(options)
    {
    }

    public DbSet<RateItem> RateItems { get; set; }
  }
}
