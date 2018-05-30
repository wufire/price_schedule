using System;
using System.ComponentModel.DataAnnotations;

namespace RatesSchedule.Models
{
  public class RateRequest
  {
    [Required]
    public DateTime StartTime
    { get; set; }

    [Required]
    public DateTime EndTime
    { get; set; }
  }
}
