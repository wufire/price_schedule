﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    }

    [HttpGet]
    public List<RateItem> GetAll()
    {
      var itemList = _context.RateItems
                             .Include(r => r.DomainItem)
                             .ToList();

      return itemList;
    }

    [HttpGet("{id}", Name = "GetRate")]
    public IActionResult GetById(long id)
    {
      var item = _context.RateItems
                         .Where(i => i.Id == id)
                         .Include(r => r.DomainItem)
                         .Single();
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
        _context.AddRateItem(item);

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

    [HttpGet("domain/{id}", Name = "GetDomainRate")]
    public IActionResult GetDomainById(long id)
    {
      var domainItem = _context.RateDomainItems.Find(id);
      if (domainItem == null)
      {
        return NotFound();
      }
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      return Ok(domainItem);
    }
  }
}
