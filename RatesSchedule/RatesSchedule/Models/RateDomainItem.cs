using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RatesSchedule.Models
{
  public class RateDomainItem
  {
    public DayFlags Days { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public TimeSpan RateDuration
    {
      get
      {
        return EndTime - StartTime;
      }
    }
    public int Price { get; set; }

    [Key]
    [ForeignKey("RateItem")]
    public long RateItemId { get; set; }
    // Leaving this navigation property commented 
    // until I figure out the proper way to still have it without really messy JSON returns
    //public RateItem RateItem { get; set; }

    public bool CheckValidDay(DayOfWeek dayOfWeek)
    {
      switch (dayOfWeek)
      {
        case DayOfWeek.Monday:
          return Days.Monday;
        case DayOfWeek.Tuesday:
          return Days.Tuesday;
        case DayOfWeek.Wednesday:
          return Days.Wednesday;
        case DayOfWeek.Thursday:
          return Days.Thursday;
        case DayOfWeek.Friday:
          return Days.Friday;
        case DayOfWeek.Saturday:
          return Days.Saturday;
        case DayOfWeek.Sunday:
          return Days.Sunday;
        default:
          // Unknown day of the week
          throw new ArgumentException("Unknown Day of the Week");
      }
    }

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
            throw new ArgumentException("Unknown Day String");
        }
      }
      return flags;
    }

    public void SetTimes(string timeString)
    {
      var times = timeString.Split('-');

      StartTime = TimeSpanFromString(times[0]);
      EndTime = TimeSpanFromString(times[1]);
    }

    private TimeSpan TimeSpanFromString(string timeInhhmmFormat)
    {
      var span = timeInhhmmFormat.Insert(2, ":");
      // This is because TimeSpan.Parse does not handle 24:00 and up
      return new TimeSpan(int.Parse(span.Split(':')[0]),    // hours
                           int.Parse(span.Split(':')[1]),    // minutes
                           0);
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
