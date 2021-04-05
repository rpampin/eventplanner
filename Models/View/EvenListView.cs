using System;

namespace EventPlanner.Models.View
{
    public class EvenListView
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Celebrant { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public Package Package { get; set; }
        public decimal DownPayment { get; set; }
        public decimal Balance { get; set; }
        public int GuestsCount { get; set; }
        public int SuppliersCount { get; set; }
    }
}
