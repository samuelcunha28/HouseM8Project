using System.ComponentModel.DataAnnotations;

namespace HouseM8API.Models
{
    /// <summary>
    /// Classe que representa o modelo do perfil do Employer
    /// </summary>
    public class EmployerProfileModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public float AverageRating { get; set; }
    }
}
