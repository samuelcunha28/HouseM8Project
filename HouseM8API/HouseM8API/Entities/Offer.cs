namespace Models
{
    public class Offer
    {
        public int Id { get; set; }
        public JobPost JobPost { get; set; }
        public double Price { get; set; }
        public bool Approved { get; set; }
    }
}
