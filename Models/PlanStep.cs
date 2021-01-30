using System;
using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
    public class PlanStep
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Index { get; set; }
        [Required]
        [DataType(DataType.Html)]
        public string Description { get; set; }
        public PlanPart PlanPart { get; set; }
    }
}