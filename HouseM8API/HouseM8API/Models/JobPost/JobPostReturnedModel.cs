using Enums;
using HouseM8API.Entities;


namespace HouseM8API.Models
{
    public class JobPostReturnedModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Categories Category { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public bool Tradable { get; set; }
        public double InitialPrice { get; set; }
        public string Address { get; set; }
        public int EmployerId { get; set; }
        public Payment[] PaymentMethod { get; set; }
        public int Range { get; set; }
    }
}
