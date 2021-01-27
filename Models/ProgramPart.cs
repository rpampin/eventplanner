using System;

namespace EventPlanner.Models
{
    public class ProgramPart
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public ProgramStep[] Steps { get; set; }
    }
}