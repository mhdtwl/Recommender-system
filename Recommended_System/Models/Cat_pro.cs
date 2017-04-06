using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Recommended_System.Models
{
   
    [Table("Cat-Pro")]
    public class Cat_pro
    {
        [Key]
        [Column(Order = 1)]
        public int Cat_Id { get; set; }


        [Key]
        [Column(Order = 2)]
        public int P_Id { get; set; }

    }
    public class catproDBContext : DbContext
    {


        public DbSet<Cat_pro> catpro { get; set; }
    }
}