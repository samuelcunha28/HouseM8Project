using System.ComponentModel.DataAnnotations;

namespace HouseM8API.Models
{
    public class ChatConnection
    {
        [Required]
        public int UserId {get; set;}
        [Required]
        public string Connection {get; set;}
    }
}