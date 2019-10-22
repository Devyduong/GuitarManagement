namespace GuitarManagement.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=DBAccess")
        {
        }

        public virtual DbSet<BILL> BILLs { get; set; }
        public virtual DbSet<BILLDETAIL> BILLDETAILs { get; set; }
        public virtual DbSet<ORDER> ORDERS { get; set; }
        public virtual DbSet<PRODUCT> PRODUCTs { get; set; }
        public virtual DbSet<USER> USERS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ORDER>()
                .Property(e => e.BUYERPHONENUMBER)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.USERNAME)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.PASSWORDS)
                .IsUnicode(false);
        }
    }
}
