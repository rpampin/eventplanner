using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventPlanner.Data;
using EventPlanner.Models;
using System.IO;

namespace EventPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AttachmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        byte[] DecodeUrlBase64(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/').PadRight(4 * ((s.Length + 3) / 4), '=');
            return Convert.FromBase64String(s);
        }


        // GET: api/Attachments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Attachment>> GetAttachment(Guid id)
        {
            var attachment = await _context.Attachments.FindAsync(id);

            if (attachment == null)
            {
                return NotFound();
            }

            return attachment;
        }

        public class AttachmentPostData
        {
            public Attachment attachment { get; set; }
            public Guid supplierId { get; set; }
        }

        // POST: api/Attachments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Attachment>> PostAttachment(AttachmentPostData prm)
        {
            prm.attachment.Supplier = await _context.Suppliers.FindAsync(prm.supplierId);
            _context.Attachments.Add(prm.attachment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAttachment", new { id = prm.attachment.Id }, prm.attachment);
        }

        // DELETE: api/Attachments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttachment(Guid id)
        {
            var attachment = await _context.Attachments.FindAsync(id);
            if (attachment == null)
            {
                return NotFound();
            }

            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AttachmentExists(Guid id)
        {
            return _context.Attachments.Any(e => e.Id == id);
        }
    }
}
