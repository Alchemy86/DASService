namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Chart_PopularDomainsThisMonth
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(150)]
        public string AuctionRef { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(250)]
        public string DomainName { get; set; }

        public int? Bids { get; set; }
    }
}
