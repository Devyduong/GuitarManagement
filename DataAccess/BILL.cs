namespace GuitarManagement.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BILL")]
    public partial class BILL
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string SENDER { get; set; }

        [StringLength(50)]
        public string RECEIVER { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DATECREATE { get; set; }

        public int? STATUSS { get; set; }
    }
}
