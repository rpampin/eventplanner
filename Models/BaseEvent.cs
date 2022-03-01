﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
    public class BaseEvent : IWithAttachments
    {
        public BaseEvent()
        {
            Date = DateTime.Now.Date;
            Attachments = new List<Attachment>();
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        public EventType Type { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }
        [Required]
        public DateTime? ReceptionVenue { get; set; }
        [Required]
        public TimeSpan? ReceptionTime { get; set; }
        [Required]
        public DateTime? PreparationVenue { get; set; }
        [Required]
        public TimeSpan? PreparationTime { get; set; }
        [Required]
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