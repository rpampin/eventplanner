using EventPlanner.Data;
using EventPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
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

        public class AttendanceData
        {
            public bool attending { get; set; }
        }

        [HttpPut("{id}/attendance")]
        public async Task<IActionResult> PutGuestAttendance(Guid id, AttendanceData data)
        {
            var guest = await _context.Guests.FindAsync(id);
            guest.WillAttend = data.attending;
            await _context.SaveChangesAsync();
            return Ok();
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

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpGet("send-invitations/{eventId}")]
        public async Task<IActionResult> SendInvitations(Guid eventId, [FromQuery] bool resend, [FromQuery] Guid? guestId)
        {
            var smtpConfig = await _context.SmtpConfig.FirstOrDefaultAsync();
            if (smtpConfig == null)
                return BadRequest("There's no SMTP configured. Please configure one to send emails.");

            var @event = await _context.Events.Include(e => e.Type).FirstAsync(e => e.Id == eventId);

            if (string.IsNullOrEmpty(@event.EmailSubject) || string.IsNullOrEmpty(@event.EmailTemplate))
                return BadRequest("No Subject/Template configured for this event.");

            var guestLinq = _context.Guests.Where(g => g.Event.Id == eventId);
            IList<Guest> guests;

            if (guestId == null)
                guests = await guestLinq.ToListAsync();
            else
                guests = await guestLinq.Where(g => g.Id == guestId.Value).ToListAsync();

            if ((!resend && guests.Where(g => !g.InvitationSent).Count() == 0) || (resend && guests.Count == 0))
                return BadRequest("There's no guests to send the invitation to");

            #region GOOGLE API
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
            #endregion

            var smtpClient = new SmtpClient(smtpConfig.Host)
            {
                Port = smtpConfig.Port,
                Credentials = new NetworkCredential(smtpConfig.Username, smtpConfig.Password),
                EnableSsl = true,
                UseDefaultCredentials = false
            };

            string template = @event.EmailTemplate;
            string subject = @event.EmailSubject;

            var signature = await _context.Configurations.Select(c => c.EmailSignature).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(signature))
                template += "<hr>" + signature;

            var attachments = new List<Models.Attachment>();
            string pattern = @"<img.*?src=""(?<url>.*?)"".*?>";
            Regex rx = new Regex(pattern);
            foreach (Match m in rx.Matches(template))
            {
                string matchString = Regex.Match(m.ToString(), "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                string newName = RandomString(10);
                attachments.Add(new Models.Attachment
                {
                    Name = newName,
                    Base64 = matchString
                });
                template = template.Replace(matchString, $"cid:{newName}");
            }

            template = template.Replace("[event.type]", @event.Type.Name);
            template = template.Replace("[event.date]", @event.Date.ToLongDateString());
            template = template.Replace("[event.celebrant]", @event.Celebrant);
            template = template.Replace("[event.address]", @event.Address);
            template = template.Replace("[event.mobile]", @event.Mobile);
            template = template.Replace("[event.email]", @event.Email);

            subject = subject.Replace("[event.type]", @event.Type.Name);
            subject = subject.Replace("[event.date]", @event.Date.ToLongDateString());
            subject = subject.Replace("[event.celebrant]", @event.Celebrant);
            subject = subject.Replace("[event.address]", @event.Address);
            subject = subject.Replace("[event.mobile]", @event.Mobile);
            subject = subject.Replace("[event.email]", @event.Email);

            if (@event is Wedding)
            {
                var wedding = @event as Wedding;
                template = template.Replace("[event.brideName]", wedding.BrideName);
                template = template.Replace("[event.groomName]", wedding.GroomName);
                template = template.Replace("[event.ceremonyVenue]", wedding.CeremonyVenue);
                template = template.Replace("[event.ceremonyTime]", wedding.CeremonyTime);
                template = template.Replace("[event.receptionVenue]", wedding.ReceptionVenue);
                template = template.Replace("[event.receptionTime]", wedding.ReceptionTime);

                subject = subject.Replace("[event.brideName]", wedding.BrideName);
                subject = subject.Replace("[event.groomName]", wedding.GroomName);
                subject = subject.Replace("[event.ceremonyVenue]", wedding.CeremonyVenue);
                subject = subject.Replace("[event.ceremonyTime]", wedding.CeremonyTime);
                subject = subject.Replace("[event.receptionVenue]", wedding.ReceptionVenue);
                subject = subject.Replace("[event.receptionTime]", wedding.ReceptionTime);
            }

            foreach (var g in guests)
            {
                var guestTemplate = template;
                var guestSubject = subject;
                guestTemplate = guestTemplate.Replace("[guest.name]", g.Name);
                guestTemplate = guestTemplate.Replace("[guest.email]", g.Email);
                guestTemplate = guestTemplate.Replace("[guest.mobile]", g.Mobile);

                guestSubject = guestSubject.Replace("[guest.name]", g.Name);
                guestSubject = guestSubject.Replace("[guest.email]", g.Email);
                guestSubject = guestSubject.Replace("[guest.mobile]", g.Mobile);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpConfig.Username),
                    Subject = guestSubject,
                    Body = guestTemplate,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(g.Email);

                var memoryStreams = new List<MemoryStream>();
                foreach (var a in attachments)
                {
                    var base64 = a.Base64.Split(',').Last();
                    base64 = base64.Replace('-', '+');
                    base64 = base64.Replace('_', '/');
                    var bytes = Convert.FromBase64String(base64);
                    var stream = new MemoryStream(bytes);
                    memoryStreams.Add(stream);
                    var attch = new System.Net.Mail.Attachment(stream, a.Name);
                    attch.ContentId = a.Name;
                    mailMessage.Attachments.Add(attch);
                }

                smtpClient.Send(mailMessage);

                foreach (var s in memoryStreams)
                    s.Close();
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
