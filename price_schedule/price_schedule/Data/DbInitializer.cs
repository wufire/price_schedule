using System;
using System.Linq;
using RatesSchedule.Models;

namespace RatesSchedule.Data
{
  public static class DbInitializer
  {
    public static void Initialize(RatesContext context)
    {
      if (context.RateItems.Any())
      {
        //Data already populated
        return;
      }

      var rateItems = new RateItem[]
      {
        new RateItem
          {
            Days = "mon,tues,thurs",
            Times = "0900-2100",
            Price = 1500
        },
        new RateItem
        {
            Days = "fri,sat,sun",
            Times = "0900-2100",
            Price = 2000
        },
        new RateItem
        {
            Days = "wed",
            Times = "0600-1800",
            Price = 1750
        },
        new RateItem
        {
            Days = "mon,wed,sat",
            Times = "0100-0500",
            Price = 1000
        },
        new RateItem
        {
            Days = "sun,tues",
            Times = "0100-0700",
            Price = 925
        }
      };

      foreach (var rateItem in rateItems)
      {
        context.AddRateItem(rateItem);
      }
    }
  }
}
