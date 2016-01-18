using System;
using DAS.Domain.GoDaddy.Users;

namespace DAS.Domain.GoDaddy
{
    public class Auction
    {
        [System.ComponentModel.Browsable(false)]
        public Guid AuctionId { get; set; }
        public string DomainName { get; set; }
        public string AuctionRef { get; set; }
        public int MinBid { get; set; }
        public DateTime EndDate { get; set; }
        [System.ComponentModel.Browsable(false)]
        public Guid AccountId { get; set; }
        public int? MyBid { get; set; }
        public bool Processed { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int Bids { get; set; }

        [System.ComponentModel.Browsable(false)]
        public string AccountUsername {
            get { return GoDaddyAccount == null ? "" :  GoDaddyAccount.AccountUsername;  }
        }
        [System.ComponentModel.Browsable(false)]
        public GoDaddyAccount GoDaddyAccount { get; set; }
    }
}
