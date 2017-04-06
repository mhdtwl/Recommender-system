using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Recommended_System.Models
{
    [Table("Cust-Shop")]
    public class Cust_Shop
    {
        [Key]
        [Column(Order = 1)]
        public int S_Id { get; set; }


        [Key]
        [Column(Order = 2)]
        public int Cust_Id { get; set; }

        public Cust_Shop(int S_Id, int Cust_Id)
        {
            this.Cust_Id = Cust_Id;
            this.S_Id = S_Id;
        }

        public Cust_Shop()
        {
            
        }
    }
    public class Cust_ShopDBContext : DbContext
    {


        public DbSet<Cust_Shop> cust_shop { get; set; }
    }
}