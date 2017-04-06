using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Resources;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Recommended_System.Models
{
    [Table("Item-Value")]
    public class Item_value
    {
        [Key]
        [Column(Order = 1)]
        public int T_Id { get; set; }


        [Key]
        [Column(Order = 2)]
        public int P_id { get; set; }

          public string value { get; set; }

     
   
    }
    public class ItemvalueDBContext : DbContext
    {

       
        public DbSet<Item_value> item_val { get; set; }
    }
}