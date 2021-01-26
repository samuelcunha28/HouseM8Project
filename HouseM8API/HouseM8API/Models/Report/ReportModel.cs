using HouseM8API.Entities.Enums;
using System.ComponentModel.DataAnnotations;


namespace HouseM8API.Models
{
    public class ReportModel
    {
        [Required]
        public string Comment { get; set; }
        [Required]
        public Reasons Reason { get; set; }
    }
}
