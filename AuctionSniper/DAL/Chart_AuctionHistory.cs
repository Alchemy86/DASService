namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Chart_AuctionHistory
    {
        [Key]
        [StringLength(150)]
        public string Username { get; set; }

        public int? TotalAuctions { get; set; }

        public int? AuctionsThisMonth { get; set; }

        public int? AuctionsWonTotal { get; set; }
    }
}
