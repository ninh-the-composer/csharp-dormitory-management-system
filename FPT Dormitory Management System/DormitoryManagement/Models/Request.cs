namespace DormitoryManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Request")]
    public partial class Request
    {
        public int Id { get; set; }

        public int? StudentId { get; set; }

        public int? TypeId { get; set; }

        [StringLength(255)]
        public string Title { get; set; }

        public string Purpose { get; set; }

        public string Reply { get; set; }

        public bool? IsDone { get; set; }

        public DateTime? DateCreated { get; set; }

        public virtual RequestType RequestType { get; set; }

        public virtual Student Student { get; set; }
    }
}
