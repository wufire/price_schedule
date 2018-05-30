using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RatesSchedule.Models
{
  public class RateScheduleData
  {
    public long Id { get; set; }

    [Required]
    public List<RateItem> Rates { get; set; }

  }
}
