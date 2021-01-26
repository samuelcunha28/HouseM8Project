using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Job
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int Mate { get; set; }
        //public Invoice Invoice { get; set; }
        [Required]
        public int JobPost { get; set; }
        [Required]
        public bool FinishedConfirmedByEmployer { get; set; }
        [Required]
        public bool FinishedConfirmedByMate { get; set; }
        [Required]
        public int Employer { get; set; }
    }
}
