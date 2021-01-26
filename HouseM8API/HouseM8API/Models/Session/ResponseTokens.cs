using System.ComponentModel.DataAnnotations;

namespace HouseM8API.Models
{
    public class ResponseTokens
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}