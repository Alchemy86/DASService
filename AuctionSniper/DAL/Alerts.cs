namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class Alerts
    {
        [Key]
        public Guid AlertID { get; set; }

        [Required]
        [StringLength(800)]
        public string Description { get; set; }

        public DateTime TriggerTime { get; set; }

        public bool Processed { get; set; }

        public Guid AuctionID { get; set; }

        public bool Custom { get; set; }

        [MaxLength(50)]
        public string AlertType { get; set; }

        public virtual Auctions Auction { get; set; }
    }
}
