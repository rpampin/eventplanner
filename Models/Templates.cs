using System;
using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
    public class Templates
    {
        public Templates()
        {
            EmailSignature = "";
            EventProgramTemplate = "";
        }

        [Key]
        public Guid Id { get; set; }
        public string EmailSignature { get; set; }
        public string EventProgramTemplate { get; set; }
    }
}
