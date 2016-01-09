namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SearchTable")]
    public partial class SearchTable
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(50)]
        public string SearchValue { get; set; }

        [Required]
        [StringLength(50)]
        public string FieldName { get; set; }

        [StringLength(50)]
        public string Value { get; set; }

        [StringLength(50)]
        public string DataType { get; set; }

        public int ItemOrder { get; set; }

        public Guid UserID { get; set; }
    }
}
