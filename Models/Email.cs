using System.Collections.Generic;

namespace EventPlanner.Models
{
    public record Email
    {
        public Email()
        {
            Attachments = new List<EAttachment>();
        }

        public string Body { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public IList<EAttachment> Attachments { get; set; }
    }

    public record EAttachment(string Name, string Base64);
}
