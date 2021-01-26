using HouseM8API.Entities;
using System.ComponentModel.DataAnnotations;

namespace HouseM8API.Models
{
    /// <summary>
    /// Modelo usado por um utilizador
    /// para resgistar os dados pessoais
    /// </summary>
    public abstract class UserRegister
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Description { get; set; }
        [Required]
        public Address Address { get; set; }
    }
}
