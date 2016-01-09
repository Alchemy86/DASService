namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AdvSearch")]
    public partial class AdvSearch
    {
        public Guid AdvSearchID { get; set; }

        [Required]
        [StringLength(200)]
        public string DomainName { get; set; }

        [StringLength(500)]
        public string DomainLink { get; set; }

        public int? Age { get; set; }

        public int? PageRank { get; set; }

        public int? MOZDA { get; set; }

        public int? MOZPA { get; set; }

        public decimal? MozRank { get; set; }

        public decimal? MozTrust { get; set; }

        public int? CF { get; set; }

        public int? TF { get; set; }

        public decimal? AlexARank { get; set; }

        public int? MozDom { get; set; }

        public int? MajDom { get; set; }

        [StringLength(200)]
        public string SimilarWeb { get; set; }

        [StringLength(50)]
        public string SemTarf { get; set; }

        public int? DomainPrice { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? EsitmateEnd { get; set; }

        public Guid UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string Ref { get; set; }

        public bool IsAuction { get; set; }

        public bool IsGOdaddy { get; set; }
    }
}
