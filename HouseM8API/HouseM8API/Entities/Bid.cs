using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseM8API.Entities
{
    public class Bid
    {
        public int AuctionId { get; set; }
        public Auction Auction { get; set; }
        public double BidAmount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
