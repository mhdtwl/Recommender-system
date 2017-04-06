using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Recommended_System.Models;
using System.Web.Services;
using System.Web.Script.Serialization;

namespace Recommended_System.Controllers
{
    public class HomeController : Controller
    {
        ItemDBContext dbitem = new ItemDBContext();
        catproDBContext dbc = new catproDBContext();
        CatDBContext dbcat = new CatDBContext();
        ProperitesDBContext dbprp = new ProperitesDBContext();
        private RRDBContext ratdb = new RRDBContext();
        private ShopItemDBContext shopit = new ShopItemDBContext();
        private shopDBContext shopdb = new shopDBContext();
        //
        // GET: /Home/

        //public ActionResult search( string keyWord) {
        //    ItemsController itmCntr = new ItemsController();
        //    var itms = itmCntr.SearchItemsByName(keyWord);

        //    //ViewBag.itemsSearched = itms;
        //    return View(itms);
        //}
        public ActionResult Index()
        {
            // = new LinqRecSysDataContext() ; 

            //LinqRecSysDataContext db = new LinqRecSysDataContext();
            //db.Categories


            ///ViewData.ModelMetadata.DisplayName = "Quick Search..";
            //ViewData["categories"] = db.Categories;
            //ViewData["properties"] = db.Properties;
            //ViewData["products"] = db.Items;
            //return View();

            //var dbCatgroies = from m in Categories select m;
            //List<Categories> LsCtg = new List<Categories>();

            //var LsCtg = dbcat.category.ToList();
            //ViewBag.category = LsCtg; //// dbCatgroies.ToList(); //// dbCtg.category.ToList();
            //ViewBag.item = dbItm.item.ToList();
            Newest_Product();
            MostRated_Product();
            //var Category = from m in db.m                             select m;
            return View();
        }


        public ActionResult SearchProduct(string keyword)
        {
            var myItems = from u in dbitem.item select u;
            if (!String.IsNullOrEmpty(keyword))
            {
                myItems = myItems.Where(c => c.T_Name.Contains(keyword));
                return Json(myItems.ToList(), JsonRequestBehavior.AllowGet); //myItems  "Hii"
            }
            else
                return Json(new List<Items>(), JsonRequestBehavior.AllowGet); //myItems  "Hii"
        }
        public ActionResult Newest_Product()
        {

            List<Itemadder> itmAdr_lst = new List<Itemadder>();
            List<Items> itm_lst = new List<Items>();
            //---------------
            List<Categories> cat_lst = (from a in dbcat.category select a).ToList();
            List<Items> it_lst = (from a in dbitem.item select a).ToList();
            if (it_lst.Count != 0)
            {
                foreach (var i in cat_lst)
                {
                    var reslt = (from t in dbitem.item where t.Cat_Id == i.Cat_Id select t.T_Id).ToList();
                    if (reslt != null && reslt.Count != 0)
                    {
                        int itm_last_id = (from t in dbitem.item where t.Cat_Id == i.Cat_Id select t.T_Id).Max();
                        //-----------make itm
                        Items it = new Items();
                        it.Cat_Id = i.Cat_Id;
                        it.T_Id = itm_last_id;
                        it.T_Name = (from a in dbitem.item where itm_last_id == a.T_Id select a.T_Name).First();
                        itm_lst.Add(it);
                        //-----------make prop
                        ItemController asd = new ItemController();
                        Itemadder itmAd = asd.add_prop_item(it);

                        var g = from a in ratdb.itrat where a.Id == itmAd.item_id select a;
                        foreach (var b in g)
                        {
                            itmAd.up = b.Up;
                            itmAd.down = b.Down;
                        }
                        int shop_id = (from a in shopit.shopit where a.T_Id == itmAd.item_id select a.S_Id).First();
                        itmAd.shop_name = (from a in shopdb.shop where a.S_Id == shop_id select a.About).First();
                        itmAd.shop_id = shop_id;
                        //var q_date = from n in dbitem.item
                        //        group n by n.T_Id into g
                        //        select new { AccountId = g.Key, Date = g.Max(t => t.) };
                        //itmAd.time_added = q_date;
                        itmAdr_lst.Add(itmAd);
                    }
                }
            }
            ViewBag.Newest_Product_Got = itmAdr_lst;
            return View();
        }
        public ActionResult MostRated_Product()
        {
            List<Itemadder> itmAdr_lst = new List<Itemadder>();
            List<Items> itm_lst = new List<Items>();
            //---------------
            List<Categories> cat_lst = (from a in dbcat.category select a).ToList();
            List<Items> it_lst = (from a in dbitem.item select a).ToList();
            if (it_lst.Count != 0)
            {
                foreach (var i in cat_lst)
                {
                    var reslt = (from t in dbitem.item where t.Cat_Id == i.Cat_Id select t.T_Id).ToList();
                    if (reslt != null && reslt.Count != 0)
                    {

                        var itms_dd = from t in dbitem.item where t.Cat_Id == i.Cat_Id select t.T_Id;
                        int maxRate = int.MinValue;
                        int maxRate_itmID = 0;
                        bool isAnyRated = false;
                        int id_lst_itm = 0;
                        foreach (var id in itms_dd)
                        {
                            var iteeasd = from a in ratdb.itrat where id == a.Id select a;
                            if (iteeasd != null && iteeasd.ToList().Count != 0)
                            {
                                Item_rate itrt = (from a in ratdb.itrat where id == a.Id select a).First();
                                int r = itrt.Up - itrt.Down;
                                if (maxRate < r)
                                {
                                    maxRate = r;
                                    maxRate_itmID = id;
                                }
                                isAnyRated = true;
                            }
                            id_lst_itm = id;
                        }
                        if (!isAnyRated)
                        {
                            maxRate_itmID = id_lst_itm;
                        }
                        //-----------make itm
                        Items it = new Items();
                        it.Cat_Id = i.Cat_Id;
                        if (maxRate_itmID != null && maxRate_itmID != 0)
                        {
                            it.T_Id = maxRate_itmID;
                            it.T_Name = (from a in dbitem.item where maxRate_itmID == a.T_Id select a.T_Name).First();
                            itm_lst.Add(it);
                            //-----------make prop
                            ItemController asd = new ItemController();
                            Itemadder itmAd = asd.add_prop_item(it);

                            var g = from a in ratdb.itrat where a.Id == itmAd.item_id select a;
                            foreach (var b in g)
                            {
                                itmAd.up = b.Up;
                                itmAd.down = b.Down;
                            }
                            int shop_id = (from a in shopit.shopit where a.T_Id == itmAd.item_id select a.S_Id).First();
                            itmAd.shop_name = (from a in shopdb.shop where a.S_Id == shop_id select a.About).First();
                            itmAd.shop_id = shop_id;
                            //itmAd.time_added = 
                            itmAdr_lst.Add(itmAd);
                        }
                    }
                }
            }
            ViewBag.MostRated_Product_Got = itmAdr_lst;
            return View();
        }


    }

}
