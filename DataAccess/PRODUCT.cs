namespace GuitarManagement.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PRODUCT")]
    public partial class PRODUCT
    {
        [StringLength(15)]
        public string ID { get; set; }

        [StringLength(100)]
        public string MNAME { get; set; }

        [StringLength(50)]
        public string CATEGORY { get; set; }

        [StringLength(100)]
        public string MANUFACTURER { get; set; }

        public int? PRICE { get; set; }

        public int? NUMBER { get; set; }

        [Column(TypeName = "ntext")]
        public string DESCRIPTIONS { get; set; }

        public int? STATUSS { get; set; }

        [StringLength(100)]
        public string IMAGES { get; set; }
    }
}
