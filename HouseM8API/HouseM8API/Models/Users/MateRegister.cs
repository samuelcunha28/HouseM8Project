using Enums;
using System.ComponentModel.DataAnnotations;

namespace HouseM8API.Models
{
    /// <summary>
    /// Modelo usado pelo utilizador para se
    /// registar como Mate
    /// </summary>
    public class MateRegister : UserRegister
    {
        [Required]
        public Categories[] Categories { get; set; }
        [Required]
        public int Range { get; set; }
    }
}
