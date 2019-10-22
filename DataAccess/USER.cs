namespace GuitarManagement.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("USERS")]
    public partial class USER
    {
        [Key]
        [StringLength(20)]
        public string USERNAME { get; set; }

        [StringLength(30)]
        public string PASSWORDS { get; set; }

        [StringLength(50)]
        public string FULLNAME { get; set; }

        public int? ROLES { get; set; }

        public int? STATUSS { get; set; }
    }
}
