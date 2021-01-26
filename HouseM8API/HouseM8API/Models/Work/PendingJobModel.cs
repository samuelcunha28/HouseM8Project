using System.ComponentModel.DataAnnotations;

namespace HouseM8API.Models
{
    /// <summary>
    /// Classe que representa um trabalho pendente
    /// </summary>
    public class PendingJobModel
    {
        /// <summary>
        /// Modelo para trabalho pendente
        /// </summary>

        [Required]
        public int JobId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Category { get; set; }
        public string Descritpion { get; set; }
    }
}
