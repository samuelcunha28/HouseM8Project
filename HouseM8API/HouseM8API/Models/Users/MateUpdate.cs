using HouseM8API.Entities;

namespace HouseM8API.Models.Users
{
    public class MateUpdate
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public int Range { get; set; }
    }
}