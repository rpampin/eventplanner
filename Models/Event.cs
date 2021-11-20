using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
    public class Event
    {
        public Event()
        {
            Date = DateTime.Now.Date;
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        public EventType Type { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }
        [Required]
        public string Celebrant { get; set; }
        [Required]
        public DateTime? ReceptionVenue { get; set; }
        [Required]
        public TimeSpan? ReceptionTime { get; set; }
        [Required]
        public DateTime? PreparationVenue { get; set; }
        [Required]
        public TimeSpan? PreparationTime { get; set; }
        public string Address { get; set; }
        [Phone]
        public string Mobile { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Social { get; set; }
        public Package Package { get; set; }
        public decimal PackagePrice { get; set; }
        public decimal Balance { get; set; }
        public decimal DownPayment { get; set; }
        public string Notes { get; set; }
        public decimal AdditionalCharges { get; set; }
        public string Plan { get; set; }
        public string EmailSubject { get; set; }
        public string EmailTemplate { get; set; }
        public IList<Guest> Guests { get; set; }
        public IList<Supplier> Suppliers { get; set; }
        public IList<Attachment> Attachments { get; set; }
    }
}