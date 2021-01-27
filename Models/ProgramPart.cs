using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
    public class ProgramPart
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public IEnumerable<ProgramStep> Steps { get; set; }
    }
}