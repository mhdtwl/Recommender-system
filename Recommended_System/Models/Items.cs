using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Recommended_System.Models
{
    [Table("Items")]
    public class Items
    {


        [Key]
        public int T_Id { get; set; }

        public string T_Name { get; set; }

        public int Cat_Id { get; set; }

        public Items(string n, int c)
        {

            T_Name = n;
            Cat_Id = c;

        }
        public Items()
        {


        }
        public int getid()
        {
            return this.T_Id;
        }

    }


    public class ItemDBContext : DbContext
    {
        public DbSet<Items> item { get; set; }
    }
}
