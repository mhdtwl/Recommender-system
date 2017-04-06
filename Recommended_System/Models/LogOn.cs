using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Recommended_System.Models
{
    [Table("Av_Cust")]
    public class LogOn
    {
        [Key]
        [Required]
        [Display(Name = "E-Mail")]
        public string E_Mail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string Customer_name { get; set; }

        
    }
    public class LogOnDBContext : DbContext
    {
        public DbSet<Av_Cust> av_cust { get; set; }
    }
}