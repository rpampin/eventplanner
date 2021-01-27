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
        [DataType(DataType.Time)]
        public TimeSpan CeremonyTime { get; set; }
        [Required]
        public string ReceptionVenue { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public TimeSpan ReceptionTime { get; set; }
    }
}