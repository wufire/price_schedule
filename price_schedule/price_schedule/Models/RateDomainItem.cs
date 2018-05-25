using System;

namespace RatesSchedule.Models
{
  public class RateDomainItem
  {
    public DayFlags Days { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public int Price { get; set; }

    public DayFlags ConvertDays(string dayString)
    {
      var flags = new DayFlags();
      var days = dayString.Split(',');
      foreach (string day in days)
      {
        switch (day)
        {
          case "mon":
            flags.Monday = true;
            break;
          case "tues":
            flags.Tuesday = true;
            break;
          case "wed":
            flags.Wednesday = true;
            break;
          case "thurs":
            flags.Thursday = true;
            break;
          case "fri":
            flags.Friday = true;
            break;
          case "sat":
            flags.Saturday = true;
            break;
          case "sun":
            flags.Sunday = true;
            break;
          default:
            // poorly formatted day string
            break;
        }
      }
      return flags;
    }
  }

  public struct DayFlags
  {
    public bool Sunday { get; set; }
    public bool Monday { get; set; }
    public bool Tuesday { get; set; }
    public bool Wednesday { get; set; }
    public bool Thursday { get; set; }
    public bool Friday { get; set; }
    public bool Saturday { get; set; }
  }
}
