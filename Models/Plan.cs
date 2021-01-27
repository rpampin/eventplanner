using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
    public class Plan
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public IEnumerable<PlanPart> Parts { get; set; }
    }
}