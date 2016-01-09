namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AuctionHistory")]
    public partial class AuctionHistory
    {
        [Key]
        public Guid HistoryID { get; set; }

        [Required]
        [StringLength(200)]
        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid AuctionLink { get; set; }
    }
}
