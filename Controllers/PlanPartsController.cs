using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventPlanner.Data;
using EventPlanner.Models;

namespace EventPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanPartsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlanPartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PlanParts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlanPart>>> GetPlanParts()
        {
            return await _context.PlanParts.ToListAsync();
        }

        // GET: api/PlanParts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlanPart>> GetPlanPart(Guid id)
        {
            var planPart = await _context.PlanParts.FindAsync(id);

            if (planPart == null)
            {
                return NotFound();
            }

            return planPart;
        }

        // PUT: api/PlanParts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlanPart(Guid id, PlanPart planPart)
        {
            if (id != planPart.Id)
            {
                return BadRequest();
            }

            _context.Entry(planPart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanPartExists(id))
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

        // POST: api/PlanParts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{planId}")]
        public async Task<ActionResult<PlanPart>> PostPlanPart(Guid planId)
        {
            var partCount = _context.PlanParts.Where(p => p.Plan.Id == planId).Count();
            var planPart = new PlanPart
            {
                Index = partCount + 1,
                Steps = new List<PlanStep>(),
                Title = $"Part {partCount + 1}",
                Plan = await _context.Plans.FindAsync(planId)
            };

            _context.PlanParts.Add(planPart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlanPart", new { id = planPart.Id }, planPart);
        }

        // DELETE: api/PlanParts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlanPart(Guid id)
        {
            var planPart = await _context.PlanParts.FindAsync(id);
            if (planPart == null)
            {
                return NotFound();
            }

            _context.PlanParts.Remove(planPart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlanPartExists(Guid id)
        {
            return _context.PlanParts.Any(e => e.Id == id);
        }
    }
}
