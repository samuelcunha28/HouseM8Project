using Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseM8API.Entities
{
    public class Auction
    {
        public int Id { get; set; }
        public Categories Category { get; set; }
        public string Tittle { get; set; }
        public string Description { get; set; }
        public double MinimumAmount { get; set; }
        public DateTime? StartingTime { get; set; }
        public Nullable<DateTime> EndingTime { get; set; }
        public List<Bid> Bids { get; set; }
    }
}
