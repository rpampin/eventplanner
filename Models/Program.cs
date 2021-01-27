using System;

namespace EventPlanner.Models
{
    public class Program
    {
        public Guid Id { get; set; }
        public ProgramPart[] Parts { get; set; }
    }
}