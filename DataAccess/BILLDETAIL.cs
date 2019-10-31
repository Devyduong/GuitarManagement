namespace GuitarManagement.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BILLDETAIL")]
    public partial class BILLDETAIL
    {
        public int ID { get; set; }

        public int? BILLID { get; set; }

        [StringLength(15)]
        public string PRODUCTID { get; set; }

        public int? NUMBER { get; set; }

        public int? STATUSS { get; set; }
    }
}
