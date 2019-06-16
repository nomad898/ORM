namespace NorthwindEF.ado
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EmployeeCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CardID { get; set; }

        [Column(TypeName = "date")]
        public DateTime ExperationDate { get; set; }

        [Required]
        [StringLength(50)]
        public string CardHolder { get; set; }

        public int EmployeeID { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
