using System;
using System.Collections.Generic;
namespace RatesSchedule.Models
{
  public class RateItem
  {
    public long Id { get; set; }
    public string Name { get; set; }
    //public RateDayOfWeek Days { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public bool Sunday { get; set; }
    public bool Monday { get; set; }
    public bool Tuesday { get; set; }
    public bool Wednesday { get; set; }
    public bool Thursday { get; set; }
    public bool Friday { get; set; }
    public bool Saturday { get; set; }
  }

  public class RateDayOfWeek
  {
    //public long Id { get; set; }
    //public DayOfWeek Day { get; set; }

  }
}
