using Enums;

namespace HouseM8API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class MateModelExtended
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public double AverageRating { get; set; }
        public int Range { get; set; }
        public Ranks Rank { get; set; }
        public Categories[] Categories { get; set; }
    }
}
