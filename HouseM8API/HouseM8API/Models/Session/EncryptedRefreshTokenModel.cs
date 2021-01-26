using System.ComponentModel.DataAnnotations;

namespace HouseM8API.Models
{
    public class EncryptedRefreshTokenModel
    {
        [Required]
        public string Email {get; set;}
        [Required]
        public string Hash {get; set;}
        [Required]
        public string Salt {get; set;}
    }
}