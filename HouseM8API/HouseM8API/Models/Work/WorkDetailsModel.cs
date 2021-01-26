using Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace HouseM8API.Models
{
    public class WorkDetailsModel
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public Mate Mate { get; set; }
        //public Invoice Invoice { get; set; }
        [Required]
        public JobPost JobPost { get; set; }
        public int InvoiceId { get; set; }
        [Required]
        public bool FinishedConfirmedByEmployer { get; set; }
        [Required]
        public bool FinishedConfirmedByMate { get; set; }
        [Required]
        public Employer Employer { get; set; }
    }
}
