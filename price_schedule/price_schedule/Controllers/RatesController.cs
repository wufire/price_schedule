using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RatesSchedule.Models;

namespace RatesSchedule.Controllers
{
  [Route("api/[controller]")]
  public class RatesController : ControllerBase
  {
    readonly RatesContext _context;

    public RatesController(RatesContext context)
    {
      _context = context;

      if (_context.RateItems.Count() == 0)
      {

        var newItem = new RateItem
        {
          Price = 2000,
          Days = "mon,tues",
          Times = "900-2000"
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
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      return Ok(item);
    }

    [HttpPost]
    public IActionResult Create([FromBody]RateItem item)
    {
      if (item == null)
      {
        return BadRequest();
      }
      if (ModelState.IsValid)
      {
        _context.RateItems.Add(item);
        _context.SaveChanges();

        return CreatedAtRoute("GetRate", new { id = item.Id }, item);
      }
      return BadRequest(ModelState);
    }

    [HttpPut("{id}")]
    public IActionResult Update(long id, [FromBody] RateItem item)
    {
      if (item == null || item.Id != id)
      {
        return BadRequest();
      }

      var rate = _context.RateItems.Find(id);
      if (rate == null)
      {
        return NotFound();
      }

      rate.Days = item.Days;
      rate.Times = item.Times;
      rate.Price = item.Price;

      _context.RateItems.Update(rate);
      _context.SaveChanges();
      return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
      var rate = _context.RateItems.Find(id);
      if (rate == null)
      {
        return NotFound();
      }

      _context.RateItems.Remove(rate);
      _context.SaveChanges();
      return NoContent();
    }
  }
}
