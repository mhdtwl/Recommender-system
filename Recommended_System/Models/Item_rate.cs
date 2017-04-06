using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Recommended_System.Models
{
    [Table("Rate")]
    public class Item_rate
    {
        [Key]
        public int Id { get; set; }

        [Column]
        public int Up { get; set; }

        [Column]
        public int Down { get; set; }


        public Item_rate(int t, int u, int d)
        {
            Id = t;
            Up = u;
            Down = d;
        }
        public Item_rate(int t) { Id = t; }
        public Item_rate() {  }
    }
   
    public class RRDBContext : DbContext
    {
       
        public DbSet<Item_rate> itrat { get; set; }
    }
}