using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
    public class Event
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public EventType Type { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required]
        public string Celebrant { get; set; }
        public string Address { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Package { get; set; }
        [DataType(DataType.Currency)]
        public decimal DownPayment { get; set; }
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }
        public IEnumerable<Guest> Guests { get; set; }
        public IEnumerable<Supplier> Suppliers { get; set; }
        public Program Program { get; set; }
    }
}