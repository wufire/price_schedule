using System.ComponentModel.DataAnnotations;

namespace RatesSchedule.Models
{
  public class RateItem
  {
    public long Id { get; set; }

    [Required]
    public string Times
    {
      get;
      set;
    }

    string _days;

    [Required]
    public string Days
    {
      get { return _days; }
      set
      {
        _days = value;
        item.Days = item.ConvertDays(value);
      }
    }

    [Required]
    [Range(0, int.MaxValue)]
    public int? Price
    {
      get { return item.Price; }
      set { item.Price = (int)value; }
    }

    RateDomainItem item { get; }

    public RateItem()
    {
      item = new RateDomainItem();
    }
  }
}
