using System;
using System.Collections.Generic;
using RatesSchedule.Controllers;
using RatesSchedule.Models;
using Xunit;

namespace RatesScheduleTests
{
  public class CalculateRatesTest
  {
    private readonly string startTime = "2015-07-01T07:00:00Z";
    private readonly string endTime = "2015-07-01T12:00:00Z";

    private readonly RateItem[] rateItems = new RateItem[]
    {
      new RateItem
      {
        Days = "mon,tues,wed,thurs,fri",
        Times = "0600-1800",
        Price = 1500
      },
      new RateItem
      {
        Days = "sat,sun",
        Times = "0600-2000",
        Price = 2000
      }
    };

    [Fact]
    public void RateTimeTest()
    {

      List<RateDomainItem> domainItems = new List<RateDomainItem>();
      foreach (var item in rateItems)
      {
        domainItems.Add(RatesContext.CreateRateDomainItem(item));
      }

      long? result = RatesController.RateGivenTime(
        DateTime.Parse(startTime),
        DateTime.Parse(endTime),
        domainItems);

      Assert.Equal(1500, result);
    }
  }
}
