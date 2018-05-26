using System.ComponentModel.DataAnnotations;

namespace RatesSchedule.Models
{
  public class RateItem
  {
    public long Id { get; set; }

    [Required]
    public string Times
    { get; set; }

    [Required]
    public string Days
    { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int? Price
    { get; set; }

    public virtual RateDomainItem DomainItem { get; set; }
  }
}
