using Enums;
using HouseM8API.Entities;

namespace Models
{
    /// <summary>
    /// Entidade de Mate com os campos
    /// todos da base de dados
    /// </summary>
    public class Mate : User
    {
        public Categories[] Categories { get; set; }
        public Ranks Rank { get; set; }
        public Achievements[] Achievements { get; set; }
        public int Range { get; set; }
        public MateReview[] Reviews { get; set; }
        public JobPost[] IgnoredJobs { get; set; }
        public Job[] JobsToDo { get; set; }
        public Offer[] Offers { get; set; }
    }
}
