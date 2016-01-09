namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Auctions
    {
        [Key]
        public Guid AuctionID { get; set; }

        [Required]
        [StringLength(250)]
        public string DomainName { get; set; }

        [Required]
        [StringLength(150)]
        public string AuctionRef { get; set; }

        public int BidCount { get; set; }

        public int Traffic { get; set; }

        public int Valuation { get; set; }

        public int Price { get; set; }

        public int MinBid { get; set; }

        public int MinOffer { get; set; }

        public int BuyItNow { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime? EstimateEndDate { get; set; }

        public Guid AccountID { get; set; }

        [StringLength(100)]
        public string Status { get; set; }

        public int? MyBid { get; set; }

        public bool Processed { get; set; }

        public virtual GoDaddyAccount GoDaddyAccount { get; set; }
    }
}
