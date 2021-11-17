using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
    public class Supplier
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public SupplierType Type { get; set; }
        public string ContactPerson { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public IList<Attachment> Attachments { get; set; }
        public bool DownPaymentRequired { get; set; }
        [DataType(DataType.Currency)]
        public decimal PackagePrice { get; set; }
        [DataType(DataType.Currency)]
        public decimal Discount { get; set; }
        [DataType(DataType.Currency)]
        public decimal OtherPayments { get; set; }
        [DataType(DataType.Currency)]
        public decimal FirstDownPayment { get; set; }
        [DataType(DataType.Currency)]
        public decimal SecondDownPayment { get; set; }
        [DataType(DataType.Currency)]
        public decimal ThirdDownPayment { get; set; }
        [DataType(DataType.Currency)]
        public decimal TotalDown { get; set; }
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }
        public string Remarks { get; set; }
        public Event Event { get; set; }
    }
}