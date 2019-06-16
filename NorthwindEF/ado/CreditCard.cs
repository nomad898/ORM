using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindEF.ado
{
    public class CreditCard
    {
        [Key]
        public int CardID { get; set; }
        [StringLength(60)]
        public string CardHolder { get; set; }
        public DateTime ExperationDate { get; set; }
        public int? EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
