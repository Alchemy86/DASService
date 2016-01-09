namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SearchLayout")]
    public partial class SearchLayout
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string SearchValue { get; set; }

        [Required]
        [StringLength(50)]
        public string FieldName { get; set; }

        public int Order { get; set; }

        [Required]
        [StringLength(50)]
        public string ControlType { get; set; }
    }
}
