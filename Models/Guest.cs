using System;

namespace EventPlanner.Models
{
    public class Guest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool? WillAttend { get; set; }
    }
}