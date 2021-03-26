using System;
using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
    public class Wedding : Event
    {
        [Required]
        public string BrideName { get; set; }
        [Required]
        public string GroomName { get; set; }
        [Required]
        public string CeremonyVenue { get; set; }
        [Required]
        public string CeremonyTime { get; set; }
        public string BrideAddress { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string BrideMobile { get; set; }
        [DataType(DataType.EmailAddress)]
        public string BrideEmail { get; set; }
        public string GroomAddress { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string GroomMobile { get; set; }
        [DataType(DataType.EmailAddress)]
        public string GroomEmail { get; set; }
        public string BrideSocial { get; set; }
        public string GroomSocial { get; set; }
    }
}