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
    // TODO: Weird quirk with this test. 
    // DateTime.Parse converts to GMT, while the model [FromBody] parser does not
    // This causes this test to fail (when it should succeed) ¯\_(ツ)_/¯
    // Quick fix would be to ignore all those pesky timezones
    // Real fix would be to accept timezones and also make rate time spans Date agnostic
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
