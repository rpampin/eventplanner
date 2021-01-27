using System;

namespace EventPlanner.Models
{
    public class Supplier
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public SupplierType Type { get; set; }
        public string ContactPerson { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public byte[][] Attachments { get; set; }
        public bool DownPaymentRequired { get; set; }
        public decimal PackagePrice { get; set; }
        public decimal OtherPayments { get; set; }
        public decimal FirstDownPayment { get; set; }
        public decimal SecondDownPayment { get; set; }
        public decimal ThirdDownPayment { get; set; }
        public decimal TotalDown { get; set; }
        public decimal Balance { get; set; }
        public string Remarks { get; set; }
    }
}