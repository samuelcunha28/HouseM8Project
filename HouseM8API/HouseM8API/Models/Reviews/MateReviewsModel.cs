using System.ComponentModel.DataAnnotations;

namespace HouseM8API.Models
{
    public class MateReviewsModel : ReviewsModel
    {
        [Required]
        public string Comment { get; set; }
        public string Author {get; set;}
    }
}
