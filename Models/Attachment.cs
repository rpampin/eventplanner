using System;
using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
    public class Attachment
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Path { get; set; }
        public Supplier Supplier { get; set; }
        public BaseEvent Event { get; set; }
    }
}
