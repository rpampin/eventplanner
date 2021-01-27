using System;

namespace EventPlanner.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public EventType Type { get; set; }
        public DateTime Date { get; set; }
        public string Celebrant { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Package { get; set; }
        public decimal DownPayment { get; set; }
        public decimal Balance { get; set; }
        public Guest[] Guests { get; set; }
        public Supplier[] Suppliers { get; set; }
        public Program Program { get; set; }
    }
}