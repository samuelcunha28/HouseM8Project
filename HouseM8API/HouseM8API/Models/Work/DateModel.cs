using System;
using System.ComponentModel.DataAnnotations;

namespace HouseM8API.Models
{
    public class DateModel
    {
        [Required]
        public DateTime Date { get; set; }
    }
}