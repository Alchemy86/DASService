namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Chart_AuctionsEnding
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(150)]
        public string AuctionRef { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(250)]
        public string DomainName { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime EndDate { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(150)]
        public string Username { get; set; }

        public int? MyBid { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MinBid { get; set; }
    }
}
