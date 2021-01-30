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
    public class PlanStepsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlanStepsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PlanSteps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlanStep>>> GetPlanSteps()
        {
            return await _context.PlanSteps.ToListAsync();
        }

        // GET: api/PlanSteps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlanStep>> GetPlanStep(Guid id)
        {
            var planStep = await _context.PlanSteps.FindAsync(id);

            if (planStep == null)
            {
                return NotFound();
            }

            return planStep;
        }

        // PUT: api/PlanSteps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlanStep(Guid id, PlanStep planStep)
        {
            if (id != planStep.Id)
            {
                return BadRequest();
            }

            _context.Entry(planStep).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanStepExists(id))
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

        // POST: api/PlanSteps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{planPartId}")]
        public async Task<ActionResult<PlanStep>> PostPlanStep(Guid planPartId)
        {
            var stepCount = _context.PlanSteps.Where(p => p.PlanPart.Id == planPartId).Count();
            var planStep = new PlanStep
            {
                Description = "",
                Index = stepCount + 1,
                Title = $"Step {stepCount + 1}",
                PlanPart = await _context.PlanParts.FindAsync(planPartId)
            };

            _context.PlanSteps.Add(planStep);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlanStep", new { id = planStep.Id }, planStep);
        }

        // DELETE: api/PlanSteps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlanStep(Guid id)
        {
            var planStep = await _context.PlanSteps.FindAsync(id);
            if (planStep == null)
            {
                return NotFound();
            }

            _context.PlanSteps.Remove(planStep);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlanStepExists(Guid id)
        {
            return _context.PlanSteps.Any(e => e.Id == id);
        }
    }
}
