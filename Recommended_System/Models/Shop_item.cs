using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Recommended_System.Models
{
     [Table("Shop-Items")]
    public class Shop_item
    {
        
   
        [Key]
        [Column(Order = 1)]
        public int S_Id { get; set; }


        [Key]
        [Column(Order = 2)]
        public int T_Id { get; set; }
    }

     public class ShopItemDBContext : DbContext
     {
         public DbSet<Shop_item> shopit { get; set; }
     }
}