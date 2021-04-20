namespace DormitoryManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoomUsage")]
    public partial class RoomUsage
    {
        public int Id { get; set; }

        public int? RoomId { get; set; }

        public int WaterUsage { get; set; }

        public int ElectricUsage { get; set; }

        public DateTime? Date { get; set; }

        public virtual Room Room { get; set; }
    }
}
