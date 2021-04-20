namespace DormitoryManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Invoice")]
    public partial class Invoice
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public int? TypeId { get; set; }

        public int? RoomId { get; set; }

        public double Amount { get; set; }

        public int? NumberOfUse { get; set; }

        public string Note { get; set; }

        public bool? IsPaid { get; set; }

        public DateTime? Deadline { get; set; }

        public DateTime? DateCreated { get; set; }

        public virtual InvoiceType InvoiceType { get; set; }

        public virtual Room Room { get; set; }

        public virtual Student Student { get; set; }
    }
}
