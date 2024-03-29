﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventPlanner.Data;
using EventPlanner.Models;
using EventPlanner.Models.View;
using System.Threading;

namespace EventPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EvenListView>>> GetEvent([FromQuery] bool pastEvents = false)
        {
            var events = await _context.Events
                .Where(e => e.Date >= (pastEvents ? DateTime.MinValue : DateTime.Now.Date))
                .OrderByDescending(e => e.Date)
                .Select(e => new EvenListView
                {
                    Id = e.Id,
                    Date = e.Date,
                    Type = e.Type.Name,
                    Celebrant = e.Celebrant,
                    Address = e.Address,
                    Mobile = e.Mobile,
                    Email = e.Email,
                    Package = e.Package,
                    DownPayment = e.PackagePrice,
                    Balance = e.Balance,
                    GuestsCount = e.Guests.Count(),
                    SuppliersCount = e.Suppliers.Count()
                })
                .ToListAsync();

            return events;
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(Guid id)
        {
            var ev = await _context.Events
                .Include(e => e.Type)
                .Include(e => e.Attachments)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null)
            {
                return NotFound();
            }

            return ev;
        }

        // GET: api/Guests
        [HttpGet("base-data/{eventId}")]
        public async Task<ActionResult<object>> GetEventData(Guid eventId)
        {
            return await _context.Events
            .Where(e => e.Id == eventId)
            .Select(e => new
            {
                type = e.Type.Name,
                date = e.Date,
                celebrant = e.Celebrant,
                emailSubject = e.EmailSubject,
                emailTemplate = e.EmailTemplate
            })
            .FirstOrDefaultAsync();
        }

        public class UpdateProgramData
        {
            public string emailSubject { get; set; }
            public string emailTemplate { get; set; }
        }

        [HttpPost("{eventId}/program")]
        public async Task<IActionResult> UpdateEventProgram(Guid eventId, UpdateProgramData data)
        {
            var @event = await _context.Events.FindAsync(eventId);
            @event.EmailSubject = data.emailSubject;
            @event.EmailTemplate = data.emailTemplate;

            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(Guid id, Event ev)
        {
            if (id != ev.Id)
            {
                return BadRequest();
            }

            _context.Entry(ev).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event ev)
        {
            ev.Type = await _context.EventTypes.Where(et => et.Id == ev.Type.Id).SingleAsync();
            ev.Plan = await _context.Configurations.Select(c => c.EventProgramTemplate).FirstOrDefaultAsync();
            ev.Date = ev.Date.Date;
            foreach (var s in ev.Suppliers)
            {
                s.Type = await _context.SupplierTypes.Where(st => st.Id == s.Type.Id).SingleAsync();
            }

            _context.Events.Add(ev);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEventType", new { id = ev.Id }, ev);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var ev = await _context.Events
                .SingleAsync(e => e.Id == id);
            if (ev == null)
            {
                return NotFound();
            }
            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(Guid id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

        // WEDDING
        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("wedding/{id}")]
        public async Task<IActionResult> PutWeddingEvent(Guid id, Wedding ev)
        {
            if (id != ev.Id)
            {
                return BadRequest();
            }

            _context.Entry(ev).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("wedding")]
        public async Task<ActionResult<Wedding>> PostWeddingEvent(Wedding ev)
        {
            ev.Type = await _context.EventTypes.Where(et => et.Id == ev.Type.Id).SingleAsync();
            ev.Plan = await _context.Configurations.Select(c => c.EventProgramTemplate).FirstOrDefaultAsync();
            ev.Date = ev.Date.Date;
            _context.Events.Add(ev);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEventType", new { id = ev.Id }, ev);
        }

        [HttpGet("{eventId}/plan")]
        public async Task<ActionResult<object>> GetEventPlan(Guid eventId)
        {
            var @event = await _context.Events.FindAsync(eventId);
            return Ok(new
            {
                plan = @event.Plan
            });
        }

        public class EventPlan
        {
            public string Plan { get; set; }
        }

        [HttpPut("{eventId}/plan")]
        public async Task<ActionResult<string>> GetEventPlan(Guid eventId, EventPlan eventPlan)
        {
            var @event = await _context.Events.FindAsync(eventId);

            @event.Plan = eventPlan.Plan;

            await _context.SaveChangesAsync();

            return Ok(@event.Plan);
        }
    }
}
