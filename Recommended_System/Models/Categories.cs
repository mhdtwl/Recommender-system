using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Recommended_System.Models
{

     [Table("Categories")]
    public class Categories
    {

        [Display(Name = "Quick Search..", Prompt = " ")]

        [Key]
        public int Cat_Id { get; set; }
        public string Name { get; set; }
        public Categories(string n)
        {
            Name = n;

        }
        public Categories()
        {
            
           
        }
        
    }
    public class CatDBContext : DbContext
    {
        public DbSet<Categories> category { get; set; }
    }
}