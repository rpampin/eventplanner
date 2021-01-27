using System;
using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
    public class ProgramStep
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.Html)]
        public string Description { get; set; }
    }
}