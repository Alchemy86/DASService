using DAS.Domain.GoDaddy;

namespace DAL
{
    public partial class Auctions
    {
        public Auction ToDomainObject()
        {
            return new Auction
            {
                AccountId = AccountID,
                AuctionId = AuctionID,
                AuctionRef = AuctionRef,
                DomainName = DomainName,
                EndDate = EndDate,
                MinBid = MinBid,
                MyBid = MyBid,
                Processed = Processed,
                GoDaddyAccount = GoDaddyAccount.ToDomainObject()
            };
        }
    }
}
