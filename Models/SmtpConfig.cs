using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
    public class SmtpConfig
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Host { get; set; }
        [Required]
        public int Port { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}