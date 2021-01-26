using System;
using System.ComponentModel.DataAnnotations;

namespace HouseM8API.Models
{
    public class WorkModel
    {
        public int Id {get; set;}
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int Mate { get; set; }
        //public Invoice Invoice { get; set; }
        [Required]
        public int JobPost { get; set; }
        public int Employer { get; set; }
    }
}
