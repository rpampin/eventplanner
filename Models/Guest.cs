using System;
using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
    public class Guest
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }
        public bool? WillAttend { get; set; }
        public Event Event { get; set; }
    }
}