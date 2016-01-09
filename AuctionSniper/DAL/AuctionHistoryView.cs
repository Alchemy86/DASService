namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AuctionHistoryView")]
    public partial class AuctionHistoryView
    {
        [Key]
        [Column(Order = 0)]
        public Guid HistoryID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(200)]
        public string Text { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime CreatedDate { get; set; }

        [Key]
        [Column(Order = 3)]
        public Guid AuctionLink { get; set; }
    }
}
