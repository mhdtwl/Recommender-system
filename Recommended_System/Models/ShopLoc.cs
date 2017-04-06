using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Recommended_System.Models
{

    public class ShopLoc
    {

        public string name { get; set; }
        public int id { get; set; }
        public double LocLati { get; set; }
        public double LocLong { get; set; }
        public string Locat { get; set; }

        public ShopLoc(int shop_id, string shop_name, string Location, double LocLati, double LocLong)
        {
            this.id = shop_id;
            this.name = shop_name;
            this.Locat = Location;
            this.LocLong = LocLong; this.LocLati = LocLati; 
          
        }

    }
}