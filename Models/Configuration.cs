using System;
using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
    public class Configuration
    {
        [Key]
        public Guid Id { get; set; }
        public string EmailSignature { get; set; }
        public string EventProgramTemplate { get; set; }
    }
}
