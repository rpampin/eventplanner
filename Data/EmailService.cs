using EventPlanner.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace EventPlanner.Data
{
    public class EmailService
    {
        private readonly AppDBContext _context;
        private readonly AttachmentService _attachmentService;

        public EmailService(AppDBContext context, AttachmentService attachmentService)
        {
            _context = context;
            _attachmentService = attachmentService;
        }

        static Random random = new Random();
        static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task SendEmail(Email email)
        {
            var memoryStreams = new List<MemoryStream>();
            var smtpConfig = await _context.SmtpConfig.FirstOrDefaultAsync();
            if (smtpConfig == null)
                throw new ApplicationException("There's no SMTP configured. Please configure one to be able to send emails.");

            var smtpClient = new SmtpClient(smtpConfig.Host)
            {
                Port = smtpConfig.Port,
                Credentials = new NetworkCredential(smtpConfig.Username, smtpConfig.Password),
                EnableSsl = true,
                UseDefaultCredentials = false
            };

            var signature = await _context.Templates.Select(c => c.EmailSignature).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(signature))
                email.Body += "<hr>" + signature;

            string pattern = @"<img.*?src=""(?<url>.*?)"".*?>";
            Regex rx = new Regex(pattern);
            foreach (Match m in rx.Matches(email.Body))
            {
                string matchString = Regex.Match(m.ToString(), "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                string newName = RandomString(10);
                email.Attachments.Add(new Models.EAttachment(newName, matchString));
                email.Body = email.Body.Replace(matchString, $"cid:{newName}");
            }

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpConfig.Username),
                IsBodyHtml = true,
                Subject = email.Subject,
                Body = email.Body
            };

            if (!string.IsNullOrEmpty(email.To))
                foreach (var e in email.To.Split(','))
                    mailMessage.To.Add(e);
            if (!string.IsNullOrEmpty(email.Cc))
                foreach (var e in email.Cc.Split(','))
                    mailMessage.CC.Add(e);
            if (!string.IsNullOrEmpty(email.Bcc))
                foreach (var e in email.Bcc.Split(','))
                    mailMessage.Bcc.Add(e);

            foreach (var a in email.Attachments)
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

        public async Task SendInvitations(Guid eventId, bool resend, Guid? guestId)
        {
            var smtpConfig = await _context.SmtpConfig.FirstOrDefaultAsync();
            if (smtpConfig == null)
                throw new ApplicationException("There's no SMTP configured. Please configure one to be able to send emails.");

            var @event = await _context.Events.Include(e => e.Type).FirstAsync(e => e.Id == eventId);

            if (string.IsNullOrWhiteSpace(@event.EmailSubject) || string.IsNullOrWhiteSpace(@event.EmailTemplate))
                throw new ApplicationException("No Invitation Subject/Template configured for this event.");

            var guestLinq = _context.Guests.Where(g => g.Event.Id == eventId);
            IList<Guest> guests;

            if (guestId == null)
                guests = await guestLinq.ToListAsync();
            else
                guests = await guestLinq.Where(g => g.Id == guestId.Value).ToListAsync();

            if ((!resend && guests.Where(g => !g.InvitationSent).Count() == 0) || (resend && guests.Count == 0))
                throw new ApplicationException("There's no guests pending to send the invitation to");

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

            var signature = await _context.Templates.Select(c => c.EmailSignature).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(signature))
                template += "<hr>" + signature;

            var attachments = new List<Models.EAttachment>();
            string pattern = @"<img.*?src=""(?<url>.*?)"".*?>";
            Regex rx = new Regex(pattern);
            foreach (Match m in rx.Matches(template))
            {
                string matchString = Regex.Match(m.ToString(), "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                string newName = RandomString(10);
                attachments.Add(new Models.EAttachment(newName, matchString));
                template = template.Replace(matchString, $"cid:{newName}");
            }

            template = template.Replace("[event.type]", @event.Type.Name);
            template = template.Replace("[event.date]", @event.Date.Value.ToLongDateString());
            template = template.Replace("[event.celebrant]", @event.Celebrant);

            subject = subject.Replace("[event.type]", @event.Type.Name);
            subject = subject.Replace("[event.date]", @event.Date.Value.ToLongDateString());
            subject = subject.Replace("[event.celebrant]", @event.Celebrant);

            if (@event is Wedding)
            {
                var wedding = @event as Wedding;
                template = template.Replace("[event.brideName]", wedding.BrideName);
                template = template.Replace("[event.groomName]", wedding.GroomName);
                template = template.Replace("[event.ceremonyVenue]", wedding.CeremonyVenue.HasValue ? wedding.CeremonyVenue.Value.ToLongDateString() : "");
                template = template.Replace("[event.ceremonyTime]", wedding.CeremonyTime.HasValue ? wedding.CeremonyTime.Value.ToString() : "");
                template = template.Replace("[event.receptionVenue]", wedding.ReceptionVenue.HasValue ? wedding.ReceptionVenue.Value.ToLongDateString() : "");
                template = template.Replace("[event.receptionTime]", wedding.ReceptionTime.HasValue ? wedding.ReceptionTime.Value.ToString() : "");

                subject = subject.Replace("[event.brideName]", wedding.BrideName);
                subject = subject.Replace("[event.groomName]", wedding.GroomName);
                subject = subject.Replace("[event.ceremonyVenue]", wedding.CeremonyVenue.HasValue ? wedding.CeremonyVenue.Value.ToLongDateString() : "");
                subject = subject.Replace("[event.ceremonyTime]", wedding.CeremonyTime.HasValue ? wedding.CeremonyTime.Value.ToString() : "");
                subject = subject.Replace("[event.receptionVenue]", wedding.ReceptionVenue.HasValue ? wedding.ReceptionVenue.Value.ToLongDateString() : "");
                subject = subject.Replace("[event.receptionTime]", wedding.ReceptionTime.HasValue ? wedding.ReceptionTime.Value.ToString() : "");
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

                g.InvitationSent = true;

                foreach (var s in memoryStreams)
                    s.Close();
            }

            await _context.SaveChangesAsync();
        }
    }
}
