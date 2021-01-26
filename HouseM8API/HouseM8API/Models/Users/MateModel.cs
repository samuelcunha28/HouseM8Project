using Enums;
using System.ComponentModel.DataAnnotations;

namespace HouseM8API.Models
{
    /// <summary>
    /// Classe que representa o modelo de um Mate
    /// </summary>
    public class MateModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public Roles Role { get; set; }
    }
}
