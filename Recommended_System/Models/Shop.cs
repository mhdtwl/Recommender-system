using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Recommended_System.Models
{
    [Table("Shop")]
    public class Shop
    {
         [Key]
    public    int S_Id { get; set; }
  public      string Location { get; set; }
  public int Phone { get; set; }
     public   string Delevery {get;set;}
     public string About { get; set; }



     public Shop(int s, string loca, int phon, string del, string ab)
     {
         S_Id = s;
         Location = loca;
         Phone = phon;
         Delevery = del;
         About = ab;
     }

     public Shop(int s, string loc, string del)
     {
         S_Id = s;
         Location = loc;
         Delevery = del;
     }
     public Shop() { }
    }


    

    public class shopDBContext : DbContext
    {
        public DbSet<Shop> shop { get; set; }
    }
}