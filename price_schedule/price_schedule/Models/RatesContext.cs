﻿using Microsoft.EntityFrameworkCore;

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
      modelBuilder.Entity<RateItem>().ToTable("RateItems");
      modelBuilder.Entity<RateDomainItem>().ToTable("RateDomainItems");
    }

    public void AddRateItem(RateItem item)
    {
      var domainItem = new RateDomainItem();
      domainItem.Days = domainItem.ConvertDays(item.Days);
      domainItem.SetTimes(item.Times);
      domainItem.Price = (int)item.Price;

      domainItem.RateItemId = item.Id;

      item.DomainItem = domainItem;

      RateItems.Add(item);
      RateDomainItems.Add(domainItem);

      SaveChanges();
    }

    public DbSet<RateItem> RateItems { get; set; }
    public DbSet<RateDomainItem> RateDomainItems { get; set; }
  }
}
