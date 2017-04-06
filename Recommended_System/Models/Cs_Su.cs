using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Recommended_System.Models
{
    [Table("Cs_Su")]
    public class Cs_Su
    {
        [Key]
        public int ID { get; set; }
        public int Customer { get; set; }
        public int Sub { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }


        public Cs_Su() { }
        public Cs_Su(int Customer, int Sub,DateTime Start,DateTime Finish)
        {                       
            this.Customer = Customer;
            this.Sub = Sub;
            this.Start = Start;
            this.Finish = Finish;
        }
    }

    public class Cs_SuDBContext : DbContext
    {

        public DbSet<Cs_Su> cs_su { get; set; }

    }
}