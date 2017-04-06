using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Recommended_System.Models
{
    [Table("SubScription-Kind")]
    public class SubScription_Kind2
    {
        [Key]
        public int Sub_Id { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public int Time_Period { get; set; }
        public SubScription_Kind2() { }
        public SubScription_Kind2(string Type, decimal Price, int Time_Period)
        {

            this.Type = Type;
            this.Price = Price;
            this.Time_Period = Time_Period;
        }
    }

    public class SubScription_KindtDBContext : DbContext
    {

        public DbSet<SubScription_Kind2> subscription_kind { get; set; }

    }
}