namespace DormitoryManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Account")]
    public partial class Account
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Username { get; set; }

        [StringLength(255)]
        public string Password { get; set; }

        public bool? IsManager { get; set; }

        public bool? IsActive { get; set; }
    }
}
