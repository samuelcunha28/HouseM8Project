using System.ComponentModel.DataAnnotations;

namespace HouseM8API.Models
{
    public class ReviewsModel
    {
        [Required]
        public float Rating { get; set; }
    }
}
