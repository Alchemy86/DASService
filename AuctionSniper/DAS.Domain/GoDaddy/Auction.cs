using System;
using DAS.Domain.GoDaddy.Users;

namespace DAS.Domain.GoDaddy
{
    public class Auction
    {
        public Guid AuctionId { get; set; }
        public string DomainName { get; set; }
        public string AuctionRef { get; set; }
        public int MinBid { get; set; }
        public DateTime EndDate { get; set; }
        public Guid AccountId { get; set; }
        public int? MyBid { get; set; }
        public bool Processed { get; set; }

        public string AccountUsername {
            get { return GoDaddyAccount.AccountUsername;  }
        }
        public GoDaddyAccount GoDaddyAccount { get; set; }
    }
}
