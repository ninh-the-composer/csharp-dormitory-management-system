namespace DormitoryManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Student")]
    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            Invoices = new HashSet<Invoice>();
            Requests = new HashSet<Request>();
        }

        public int Id { get; set; }

        [StringLength(8)]
        public string StudentCode { get; set; }

        public int? BedId { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public bool Gender { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        public string Avatar { get; set; }

        public bool? HasInDorm { get; set; }

        public virtual Bed Bed { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request> Requests { get; set; }
    }
}
