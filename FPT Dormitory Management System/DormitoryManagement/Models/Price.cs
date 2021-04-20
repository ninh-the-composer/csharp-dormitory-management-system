namespace DormitoryManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Price")]
    public partial class Price
    {
        public int Id { get; set; }

        public int? TypeId { get; set; }

        [Column(TypeName = "money")]
        public decimal? StandardPrice { get; set; }

        public int? StandardUsage { get; set; }

        public DateTime? Date { get; set; }

        public virtual PriceType PriceType { get; set; }
    }
}
