using System;
using System.ComponentModel;

namespace RatesSchedule.Models
{
  public class RateResponse
  {
    [DisplayName("Rate")]
    public string rateOrUnavailable { get; set; }

  }
}
