using HouseM8API.Entities;

namespace Models
{
    public class Employer : User
    {
        public Mate[] Favourites { get; set; }
        public JobPost[] JobPosts { get; set; }
        public Review[] Reviews { get; set; }
        public Job[] JobsToBeDone {get; set; }
    }
}
