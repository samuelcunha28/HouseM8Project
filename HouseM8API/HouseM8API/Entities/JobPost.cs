using Enums;

namespace Models
{
    public class JobPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Categories Category { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public bool Tradable { get; set; }
        public double InitialPrice { get; set; }
        public string Address { get; set; }
        public Payment[] PaymentMethod { get; set; }
        public bool IsDone { get; set; }
    }
}
