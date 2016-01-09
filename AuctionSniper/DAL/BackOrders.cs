namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BackOrders
    {
        [Key]
        public Guid OrderID { get; set; }

        [Required]
        [StringLength(100)]
        public string DomainName { get; set; }

        [Required]
        [StringLength(100)]
        public string AlertEmail1 { get; set; }

        [StringLength(100)]
        public string AlertEmail2 { get; set; }

        public DateTime DateToOrder { get; set; }

        public int CreditsToUse { get; set; }

        public bool Private { get; set; }

        public Guid GoDaddyAccount { get; set; }

        public bool Processed { get; set; }

        public virtual GoDaddyAccount GoDaddyAccount1 { get; set; }
    }
}
