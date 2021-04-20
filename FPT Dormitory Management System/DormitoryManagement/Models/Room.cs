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
            Invoices = new HashSet<Invoice>();
            RoomUsages = new HashSet<RoomUsage>();
        }

        public int Id { get; set; }

        public int FloorId { get; set; }

        public int RoomNumber { get; set; }

        public bool? RoomGender { get; set; }

        public bool CanUse { get; set; }
        public string GetRoomName() {
            return Floor.Dom.Name + (Floor.Number * 100 + RoomNumber);
        }
        public int GetNumberStudentsInRoom() {
            int n = 0;
            foreach(Bed bed in Beds) {
                n += bed.IsAvailable ? 0 : 1;
            }
            return n;
        }
        public bool HasFull() {
            return GetNumberStudentsInRoom() == Beds.Count;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bed> Beds { get; set; }

        public virtual Floor Floor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoomUsage> RoomUsages { get; set; }
    }
}
