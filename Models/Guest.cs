using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
    public class Guest
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Table { get; set; }
        public string Remarks { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }
        public bool? WillAttend { get; set; }
        public bool InvitationSent { get; set; }
        public Event Event { get; set; }
    }
}