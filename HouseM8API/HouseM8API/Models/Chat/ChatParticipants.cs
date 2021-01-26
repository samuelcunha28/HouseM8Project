using System;
using System.ComponentModel.DataAnnotations;

namespace HouseM8API.Models
{
    public class ChatParticipants
    {
        [Required]
        public int[] Participants {get; set;}
    }
}