using System;
using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
    public class Wedding : BaseEvent
    {
        [Required]
        public string BrideName { get; set; }
        [Required]
        public string GroomName { get; set; }
        [Required]
        public DateTime? CeremonyVenue { get; set; }
        [Required]
        public TimeSpan? CeremonyTime { get; set; }
        public string BrideAddress { get; set; }
        [Phone]
        public string BrideMobile { get; set; }
        [EmailAddress]
        public string BrideEmail { get; set; }
        public string GroomAddress { get; set; }
        [Phone]
        public string GroomMobile { get; set; }
        [EmailAddress]
        public string GroomEmail { get; set; }
        public string BrideSocial { get; set; }
        public string GroomSocial { get; set; }
    }
}