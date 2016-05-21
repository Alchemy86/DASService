namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GoDaddyAccount")]
    public partial class GoDaddyAccount
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GoDaddyAccount()
        {
            Auctions = new HashSet<Auctions>();
            AuctionSearch = new HashSet<AuctionSearch>();
            BackOrders = new HashSet<BackOrders>();
            AccountID = Guid.NewGuid();
        }

        [Key]
        public Guid AccountID { get; set; }

        [Required]
        [StringLength(200)]
        public string GoDaddyUsername { get; set; }

        [Required]
        [StringLength(250)]
        public string GoDaddyPassword { get; set; }

        public Guid UserID { get; set; }

        public bool Verified { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Auctions> Auctions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuctionSearch> AuctionSearch { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BackOrders> BackOrders { get; set; }

        public virtual Users Users { get; set; }
    }
}
