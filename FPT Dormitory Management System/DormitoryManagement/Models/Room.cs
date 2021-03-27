namespace DormitoryManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Room")]
    public partial class Room
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Room()
        {
            Beds = new HashSet<Bed>();
            RoomUsages = new HashSet<RoomUsage>();
        }

        public int Id { get; set; }

        public int? FloorId { get; set; }

        [StringLength(5)]
        public string RoomNumber { get; set; }

        public bool? RoomGender { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bed> Beds { get; set; }

        public virtual Floor Floor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoomUsage> RoomUsages { get; set; }
    }
}
