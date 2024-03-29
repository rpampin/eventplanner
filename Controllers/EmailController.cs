﻿using EventPlanner.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EventPlanner.Controllers
{
    public class Email
    {
        public string Body { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public IList<EventPlanner.Models.Attachment> Attachments { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public EmailController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SendInvitations(Email email)
        {
            var memoryStreams = new List<MemoryStream>();
            var smtpConfig = await _context.SmtpConfig.FirstOrDefaultAsync();
            if (smtpConfig == null)
                return BadRequest("There's no SMTP configured. Please configure one to send emails.");

            var smtpClient = new SmtpClient(smtpConfig.Host)
            {
                Port = smtpConfig.Port,
                Credentials = new NetworkCredential(smtpConfig.Username, smtpConfig.Password),
                EnableSsl = true,
                UseDefaultCredentials = false
            };

            var signature = await _context.Configurations.Select(c => c.EmailSignature).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(signature))
                email.Body += "<hr>" + signature;

            string pattern = @"<img.*?src=""(?<url>.*?)"".*?>";
            Regex rx = new Regex(pattern);
            foreach (Match m in rx.Matches(email.Body))
            {
                string matchString = Regex.Match(m.ToString(), "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                string newName = RandomString(10);
                email.Attachments.Add(new Models.Attachment
                {
                    Name = newName,
                    Base64 = matchString
                });
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
                var attch = new Attachment(stream, a.Name);
                attch.ContentId = a.Name;
                mailMessage.Attachments.Add(attch);
            }

            smtpClient.Send(mailMessage);

            foreach (var s in memoryStreams)
                s.Close();

            return Ok();
        }
    }
}
