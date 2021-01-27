using System;
using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
    public class SupplierType
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}