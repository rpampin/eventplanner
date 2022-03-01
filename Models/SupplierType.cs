using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class SupplierType
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}