using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Recommended_System.Models
{
    public class Shop_adder
    {
        public List<Itemadder> it { get; set; }
        public int shop_id { get; set; }
        public string shop_name { get; set; }
        public Shop shp { get; set; }
        public int cust_id { get; set; }
        public int counter { get; set; }
        public List<string> sv { get; set; }
        public List<string> categ{ get; set; }
        public List<int> indx { get; set; }
        public string first { get; set; }
        public string secound { get; set; }
    }
}