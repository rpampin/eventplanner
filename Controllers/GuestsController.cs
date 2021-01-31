using EventPlanner.Data;
using EventPlanner.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Util;
// using MailKit.Net.Smtp;
// using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
// using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace EventPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public GuestsController(
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("event-guests/{eventId}")]
        public async Task<ActionResult<IEnumerable<Guest>>> GetEventGuests(Guid eventId)
        {
            var guest = await _context.Guests
            .Where(g => g.Event.Id == eventId)
            .ToListAsync();

            if (guest == null)
            {
                return NotFound();
            }

            return guest;
        }

        // GET: api/Guests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Guest>> GetGuest(Guid id)
        {
            var guest = await _context.Guests
            .Include(g => g.Event).ThenInclude(e => e.Type)
            .FirstOrDefaultAsync(g => g.Id == id);

            if (guest == null)
            {
                return NotFound();
            }

            return guest;
        }

        // PUT: api/Guests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGuest(Guid id, Guest guest)
        {
            if (id != guest.Id)
            {
                return BadRequest();
            }

            _context.Entry(guest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GuestExists(id))
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

        // POST: api/Guests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Guest>> PostGuest(Guest guest)
        {
            guest.Event = await _context.Events.FirstOrDefaultAsync(e => e.Id == guest.Event.Id);
            _context.Guests.Add(guest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGuest", new { id = guest.Id }, guest);
        }

        // DELETE: api/Guests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGuest(Guid id)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
            {
                return NotFound();
            }

            _context.Guests.Remove(guest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GuestExists(Guid id)
        {
            return _context.Guests.Any(e => e.Id == id);
        }

        [HttpGet("send-invitations/{eventId}")]
        public async Task<IActionResult> SendInvitations(Guid eventId, [FromQuery] bool resend, [FromQuery] Guid? guestId)
        {
            var smtpConfig = await _context.SmtpConfig.FirstAsync();
            if (smtpConfig == null)
                return BadRequest("There's no SMTP configured. Please configure one to send emails.");

            var @event = await _context.Events.FindAsync(eventId);
            var guestLinq = _context.Guests.Where(g => g.Event.Id == eventId);
            IList<Guest> guests;

            if (guestId == null)
                guests = await guestLinq.ToListAsync();
            else
                guests = await guestLinq.Where(g => g.Id == guestId.Value).ToListAsync();

            if ((!resend && guests.Where(g => !g.InvitationSent).Count() == 0) || (resend && guests.Count == 0))
                return BadRequest("There's no guests to send the invitation to");

            // var secrets = new ClientSecrets
            // {
            //     ClientId = _configuration["GmailStmp:Key"],
            //     ClientSecret = _configuration["GmailStmp:Secret"]
            // };

            // var googleCredentials = await GoogleWebAuthorizationBroker.AuthorizeAsync(secrets, new[] { GmailService.Scope.MailGoogleCom }, _configuration["GmailStmp:Email"], CancellationToken.None);
            // if (googleCredentials.Token.IsExpired(SystemClock.Default))
            // {
            //     await googleCredentials.RefreshTokenAsync(CancellationToken.None);
            // }

            // using (var client = new SmtpClient())
            // {
            //     client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

            //     var oauth2 = new SaslMechanismOAuth2(googleCredentials.UserId, googleCredentials.Token.AccessToken);
            //     client.Authenticate(oauth2);

            //     var message = new MimeMessage();
            //     message.From.Add(new MailboxAddress(_configuration["GmailStmp:Email"].Split("@").First(), _configuration["GmailStmp:Email"]));

            //     foreach (var g in guests)
            //     {
            //         if (!g.InvitationSent || (g.InvitationSent && resend))
            //         {
            //             g.InvitationSent = true;
            //             message.To.Add(new MailboxAddress(g.Name, g.Email));
            //         }
            //     }

            //     message.Subject = "How you doin?";
            //     message.Body = new TextPart("html")
            //     {
            //         Text = @"<h1>Hey Alice</h1>,

            //         What are you up to this weekend? Monica is throwing one of her parties on
            //         Saturday and I was hoping you could make it.

            //         Will you be my +1?

            //         -- Joey
            //         "
            //     };

            //     await client.SendAsync(message);
            //     client.Disconnect(true);
            // }

            var smtpClient = new SmtpClient(smtpConfig.Host)
            {
               Port = smtpConfig.Port,
               Credentials = new NetworkCredential(smtpConfig.Username, smtpConfig.Password),
               EnableSsl = true,
               UseDefaultCredentials = false
            };

            foreach (var g in guests)
            {
               var mailMessage = new MailMessage
               {
                   From = new MailAddress(smtpConfig.Username),
                   Subject = "subject",
                   Body = "<h1>Hello</h1>",
                   IsBodyHtml = true,
               };
               mailMessage.To.Add(g.Email);

               smtpClient.Send(mailMessage);
            }
            
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
