using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Recommended_System.Models
{
    [Table("Properties")]
    public class Properites
    {
       
       
   
         [Key]
        public int P_Id { get; set; }
         
        public string P_Name { get; set; }

        public Properites( string n)
        {
           
            P_Name = n;

        }
        public Properites()
        {

        }

    }


    public class ProperitesDBContext : DbContext
    {
        public DbSet<Items> item { get; set; }

        public DbSet<Properites> Properites { get; set; }
    }
    }
