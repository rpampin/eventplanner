using System;

namespace EventPlanner.Models
{
    public class Wedding : Event
    {
        public string BrideName { get; set; }
        public string GroomName { get; set; }
        public string CeremonyVenue { get; set; }
        public TimeSpan CeremonyTime { get; set; }
        public string ReceptionVenue { get; set; }
        public TimeSpan ReceptionTime { get; set; }
    }
}