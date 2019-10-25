namespace GuitarManagement.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ORDERS")]
    public partial class ORDER
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string SELLER { get; set; }

        [StringLength(50)]
        public string BUYER { get; set; }

        [StringLength(20)]
        public string BUYERPHONENUMBER { get; set; }

        public string PRODUCT { get; set; }

        public int? PRODUCTNUMBER { get; set; }

        public int? MONEYRECEIVED { get; set; }

        public int? EXCESSCASH { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DATECREATE { get; set; }

        public int? STATUSS { get; set; }
    }
}
