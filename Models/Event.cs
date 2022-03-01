using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
    public class Event : BaseEvent
    {
        [Required]
        public string Celebrant { get; set; }
        public string Address { get; set; }
        [Phone]
        public string Mobile { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Social { get; set; }
    }
}