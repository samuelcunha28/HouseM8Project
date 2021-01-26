using Enums;
using HouseM8API.Entities;
using System.ComponentModel.DataAnnotations;

namespace HouseM8API.Models
{
    public class JobPostModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public Categories Category { get; set; }
        public string ImagePath { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool Tradable { get; set; }
        [Required]
        public double InitialPrice { get; set; }
        [Required]
        public Address Address { get; set; }
        [Required]
        public Payment[] PaymentMethod { get; set; }
    }
}
