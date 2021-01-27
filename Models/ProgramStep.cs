using System;

namespace EventPlanner.Models
{
    public class ProgramStep
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}