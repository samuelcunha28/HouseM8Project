using System.ComponentModel.DataAnnotations;


namespace HouseM8API.Models
{
    /// <summary>
    /// Classe modelo para atualização de password
    /// </summary>
    public class PasswordUpdateModel
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string OldPassword {get; set;}
    }
}
