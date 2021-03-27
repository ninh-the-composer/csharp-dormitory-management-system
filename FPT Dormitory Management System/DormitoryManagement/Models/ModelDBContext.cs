using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DormitoryManagement.Models {
    public partial class ModelDBContext : DbContext {
        public ModelDBContext()
            : base("name=ModelDBContext") {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Bed> Beds { get; set; }
        public virtual DbSet<Dom> Doms { get; set; }
        public virtual DbSet<Floor> Floors { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceType> InvoiceTypes { get; set; }
        public virtual DbSet<Price> Prices { get; set; }
        public virtual DbSet<PriceType> PriceTypes { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<RequestType> RequestTypes { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomUsage> RoomUsages { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<InvoiceType>()
                .HasMany(e => e.Invoices)
                .WithOptional(e => e.InvoiceType)
                .HasForeignKey(e => e.TypeId);

            modelBuilder.Entity<Price>()
                .Property(e => e.StandardPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PriceType>()
                .HasMany(e => e.Prices)
                .WithOptional(e => e.PriceType1)
                .HasForeignKey(e => e.PriceType);

            modelBuilder.Entity<RequestType>()
                .HasMany(e => e.Requests)
                .WithOptional(e => e.RequestType)
                .HasForeignKey(e => e.TypeId);
        }
    }
}
