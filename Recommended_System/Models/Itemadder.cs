using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Recommended_System.Models;

namespace Recommended_System.Models
{
    public class Itemadder
    {
        public int item_id { set; get; }
        public int shop_id { get; set; }
        public string item_name { set; get; }
        public int counter { set; get; }
        public string cat_name { set; get; }
        public int up { set; get; }
        public int down { set; get; }
        public string message { set; get; }
        public int cat_id { set; get; }
        public List<Properites> p { set; get; }
        public List<int> ind { set; get; }
        public List<string> pv { set; get; }
        public HttpPostedFileBase file { set; get; }
        

        //-------------
        //new
        public List<string> name_admin { set; get; }
        public DateTime time_added { set; get; }
        
        public ShopLoc shop { set; get; }
        public string shop_name { get; set; }



        public double LocLati { get; set; }
        public double LocLong { get; set; }
        public double distace { get; set; }




        //public Itemadder()
        //{
        //    p = new List<Properites>();
        //    ind = new List<int>();
        //    pv = new List<string>();

        //}

        public Itemadder()
        {       }

        public Itemadder(int item_id, string item_name, int counter, string cat_name, int cat_id, List<Properites> p, List<int> ind, List<string> pv)
        {

            this.item_id = item_id; this.item_name = item_name; this.counter = counter; this.cat_name = cat_name;
            this.cat_id = cat_id; this.p = p; this.ind = ind; this.pv = pv;
        
        }
        public Itemadder(Items itm , int counter ,Categories cat , List<Properites> p, List<int> ind, List<string> pv)
        {

            this.item_id = itm.T_Id; this.item_name = itm.T_Name; this.counter = counter; 
            this.cat_name = cat.Name; this.cat_id = cat.Cat_Id; 
            this.p = p; this.ind = ind; this.pv = pv;
        }

        public Itemadder(Items itm , Categories cat, ShopLoc shop , double myDistance )
        {

            this.item_id = itm.T_Id; this.item_name = itm.T_Name; 
            this.cat_name = cat.Name; this.cat_id = cat.Cat_Id;
            this.shop = shop;
            this.LocLati = shop.LocLati;
            this.LocLong = shop.LocLong;
            this.distace = myDistance;
            
        }
    }
}