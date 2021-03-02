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
        public string brideSocial { get; set; }
        public string groomSocial { get; set; }
    }
}