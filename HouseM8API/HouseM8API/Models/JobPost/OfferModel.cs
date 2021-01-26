using System.ComponentModel.DataAnnotations;

namespace HouseM8API.Models
{
    public class OfferModel
    {
        [Required]
        public double Price { get; set; }
        [Required]
        public bool Approved { get; set; }
    }
}
