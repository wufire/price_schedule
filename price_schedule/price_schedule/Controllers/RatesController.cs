using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RatesSchedule.Models;

namespace RatesSchedule.Controllers
{
  [Route("api/[controller]")]
  public class RatesController : ControllerBase
  {
    private readonly RatesContext _context;

    public RatesController(RatesContext context)
    {
      _context = context;

      if (_context.RateItems.Count() == 0)
      {

        RateItem newItem = new RateItem
        {
          Name = "test",
          StartTime = new TimeSpan(9, 0, 0),
          EndTime = new TimeSpan(21, 0, 0),
          Monday = true
        };

        _context.RateItems.Add(newItem);
        _context.SaveChanges();
      }
    }

    [HttpGet]
    public List<RateItem> GetAll()
    {
      return _context.RateItems.ToList();
    }

    [HttpGet("{id}", Name = "GetRate")]
    public IActionResult GetById(long id)
    {
      var item = _context.RateItems.Find(id);
      if (item == null)
      {
        return NotFound();
      }
      return Ok(item);
    }

    [HttpPost]
    public IActionResult Create([FromBody] RateItem item)
    {
      if (item == null)
      {
        return BadRequest();
      }

      _context.RateItems.Add(item);
      _context.SaveChanges();

      return CreatedAtRoute("GetRate", new { id = item.Id }, item);
    }
  }
}
