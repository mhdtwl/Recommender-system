using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Recommended_System.Models;
using tester.Controllers;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.IO;

namespace Recommended_System.Controllers
{
    public class ItemController : Controller
    {
        private ItemDBContext db = new ItemDBContext();
        private ProperitesDBContext pdb = new ProperitesDBContext();
        private ItemvalueDBContext itdp = new ItemvalueDBContext();
        private CatDBContext catdb = new CatDBContext();
        private catproDBContext catpro = new catproDBContext();
        private ShopItemDBContext shopit = new ShopItemDBContext();
        private RRDBContext ratdb = new RRDBContext();

        private shopDBContext shopdb = new shopDBContext();

        private List<Itemadder> itmAdrLst = new List<Itemadder>();


        // GET: /Item/
        public ActionResult Index() //(string sortOrder)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "no")
                return RedirectToAction("Index", "Customers");

            return View(db.item.ToList());
        }

        // GET: /Item/Details/5
        public ActionResult Details(int id = 0)
        {
            Items items = db.item.Find(id);
            if (items == null)
            {
                return HttpNotFound();
            }
            Itemadder it = new Itemadder();
            it.pv = new List<string>();
            it.p = new List<Properites>();
            it.ind = new List<int>();
            it.item_id = items.T_Id;
            it.item_name = items.T_Name;
            it.cat_id = items.Cat_Id;

            var shpt = (from a in shopit.shopit where a.T_Id == it.item_id select a.S_Id).ToList().First();
            var shp = (from a in shopdb.shop where a.S_Id == shpt select a).ToList().First();

            it.shop_name = shp.About;
            it.shop_id = shp.S_Id;

            var q = from a in catdb.category
                    where a.Cat_Id == items.Cat_Id
                    select a;
            foreach (var a1 in q)
            {
                it.cat_name = a1.Name;
            }

            var q2 = from a1 in catpro.catpro where a1.Cat_Id == it.cat_id select a1;
            foreach (var a2 in q2)
            {
                it.ind.Add(a2.P_Id);
                int i = a2.P_Id;
                var q3 = from a3 in pdb.Properites where a3.P_Id == i select a3;
                foreach (var itm in q3)
                {
                    it.p.Add(itm);
                }
                var q4 = from a4 in itdp.item_val where a4.P_id == i && a4.T_Id == it.item_id select a4;
                foreach (var j in q4)
                {
                    it.pv.Add(j.value);
                }

            }


            var g = from a in ratdb.itrat where a.Id == it.item_id select a;
            foreach (var b in g)
            {
                it.up = b.Up;
                it.down = b.Down;
            }





            return View(it);
        }
        public ActionResult CategoryShow()
        {

            CategoryController cc = new CategoryController();
            return PartialView(cc.Index());
        }

        public string GetCategoryByid(int id)
        {
            var catgs = from c in catdb.category select c;

            catgs = catgs.Where(s => s.Cat_Id == id);
            Categories ctg = catgs.Single();
            return ctg.Name;
        }

        // GET: /Item/Create
        public ActionResult Create(int id = 0)
        {
            Cust_ShopDBContext cs = new Cust_ShopDBContext();
            var q = from a in cs.cust_shop where id == a.S_Id select a;
            int idd = 0;
            foreach (var item in q)
            {
                idd = item.Cust_Id;
            }
            if (Session["swi"] != null && (int)Session["swi"] == idd)
            {

                ViewBag.shop_id = id;
                var myCatgs = from m in catdb.category select m;
                ViewBag.message = "";
                List<Select_Item> lst_Select_Item = new List<Select_Item>();
                foreach (var item in myCatgs)
                    lst_Select_Item.Add(new Select_Item(item.Cat_Id, item.Name, false));
                ViewBag.CategoryID = lst_Select_Item;

                return View();
            }
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");

            return RedirectToAction("Index", "Customers");
        }

        //
        // POST: /Item/Create


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Itemadder items, string CategoryID)
        {
            ViewBag.message = "";
            if (ModelState.IsValid)
            {
                try
                {
                    items.message = "";
                    ViewBag.msg = "";
                    // Categories cat = catdb.category.Find(items.cat_name);
                    int cid = 0;
                    int.TryParse(CategoryID, out cid);



                    int catid = 0;
                    int id = 0;

                    items.cat_id = cid;
                    /* var result = from t in db.shop
                             orderby t.S_Id descending
                             select t;

                Shop f = result.First();
                id = f.S_Id;*/

                    id = 0;


                    Items x = new Items(items.item_name, items.cat_id);
                    x.T_Id = id + 1;


                    items.item_id = x.T_Id;

                    db.item.Add(x);
                    //var q2 = from a in catpro.catpro
                    //         where a.Cat_Id ==catid
                    //         select a;
                    Shop_item si = new Shop_item();
                    si.T_Id = items.item_id;
                    si.S_Id = items.shop_id;


                    //var q2 = from a in pdb.Properites
                    //         where a..Contains(items.cat_name)
                    //         select a;
                    //db2.Properites.Find()
                    //  Items t= new Items(items.item_name)
                    //  db.Movies.Add(items);
                    db.SaveChanges();
                    var s2 = from a in db.item
                             orderby a.T_Id descending
                             select a;
                    Items so = s2.First();

                    id = so.T_Id;
                    si.T_Id = so.T_Id;
                    shopit.shopit.Add(si);

                    shopit.SaveChanges();
                    items.item_id = id;

                    return RedirectToAction("Prop", items);
                }
                catch
                {
                    ViewBag.message = "INVALID ENTRY";
                    ViewBag.msg = "INVALID ENTRY";

                    return RedirectToAction("Index", "Shop", items.shop_id);
                }
            }

            return View(items);
        }


        public ActionResult rate(int id = 0, int tid = 0)
        {

            if (Session["swi"] != null)
                return RedirectToAction("Index", "Customers");
            string x = System.Web.HttpContext.Current.Session.SessionID + tid;
            var myval = System.Web.HttpContext.Current.Session[x];
            if (id == 0)
            {
                //Items t = new Items(it.item_name, it.cat_id);
                //t.T_Id = it.item_id;
                //db.Entry(t).State = EntityState.Modified;
                //db.SaveChanges();

                if (myval == null)
                {


                    var xx = from a in ratdb.itrat where a.Id == tid select a;
                    foreach (var h in xx)
                    {
                        h.Down = h.Down + 1;

                    }
                    ratdb.SaveChanges();
                    //x.DOWN = down_number + 1;
                    //x.T_Id = tid;
                    //x.UP = up_number;
                    //ratdb.Entry(x).State = EntityState.Modified;
                    //ratdb.SaveChanges();

                    System.Web.HttpContext.Current.Session[x] = "abc";
                }
            }
            else
            {
                if (myval == null)
                {

                    var xx = from a in ratdb.itrat where a.Id == tid select a;
                    foreach (var h in xx)
                    {
                        h.Up = h.Up + 1;

                    }
                    ratdb.SaveChanges();
                    System.Web.HttpContext.Current.Session[x] = "abc";

                }

            }

            return RedirectToAction("Details", new { id = tid });

        }
        [WebMethod()]
        public ActionResult CompareSearchProduct(string cmp_srch)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            List<string> mystring = json.Deserialize<List<string>>(cmp_srch);

            string keyword = mystring[0]; // cmp_srch.item;  
            int categoryId = int.Parse(mystring[1]); // cmp_srch.category;

            var myItems = from u in db.item select u;
            if (!String.IsNullOrEmpty(keyword))
            {
                myItems = myItems.Where(c => c.T_Name.Contains(keyword) && c.Cat_Id == categoryId);
                return Json(myItems.ToList(), JsonRequestBehavior.AllowGet); //myItems  "Hii"
            }
            else
                return Json(new List<Items>(), JsonRequestBehavior.AllowGet); //myItems  "Hii"
        }

        public ActionResult CompareItems(string CategoryID)
        {

            List<string> itmsNameLst = itmsCmprNameLstGet();
            List<Items> itmLst = new List<Items>();
            List<Itemadder> itmAddrLst = new List<Itemadder>();
            List<Properites> itmPropLst = new List<Properites>();

            var myItems = from m in db.item select m;
            var myCatgs = from m in catdb.category select m;

            if (itmLst == null) return View(new List<Itemadder>());
            ViewBag.CompareCategory = "Compare";
            ViewBag.CategoryID = make_Category_Selectors_ViewBag(CategoryID);

            int cid = 0; string category_name = "";
            int.TryParse(CategoryID, out cid);
            if (itmsNameLst != null)
            {
                if (CategoryID != null) category_name = myCatgs.Where(s => s.Cat_Id == cid).First().Name;

                var myCat = myCatgs.Where(s => s.Name.Contains(category_name)).ToList();
                int myCat_Id = myCat[0].Cat_Id;
                Categories myCateg = myCat[0];

                ViewBag.CompareCategory = myCateg.Name;
                foreach (var item in itmsNameLst)
                {
                    var tmp = myItems.Where(s => (s.T_Name.Contains(item) && s.Cat_Id == myCat_Id));

                    //new
                    List<Items> v = (tmp).ToList();
                    itmLst.AddRange(v.Where(s => s.T_Name == item));
                    //itmLst.AddRange((tmp).ToList());
                }
                foreach (var item in itmLst)
                {
                    Itemadder itmAd = add_prop_item(item);
                    var g = from a in ratdb.itrat where a.Id == itmAd.item_id select a;
                    foreach (var b in g)
                    {
                        itmAd.up = b.Up;
                        itmAd.down = b.Down;
                    }
                    itmPropLst = itmAd.p;
                    itmAddrLst.Add(itmAd);
                }
            }
            //GetBorderRate();
            ViewBag.CompareList = itmAddrLst.ToList();
            return View(itmAddrLst);
        }
        
        public void GetBorderRate(int Cat_id)
        {
            //var max_Query = (from tab1 in ratdb.itrat  select tab1.Up ).Max();
            //var min_Query = (from tab1 in ratdb.itrat  select tab1.Down).Max();
            //ViewBag.MaxRate =  max_Query ; 
            //ViewBag.MinRate = min_Query ;
        }
        
        public ActionResult itmsSrchNameLstSet(List<Itemadder> itmsCmprLst)
        {

            if (itmsCmprLst.Count != 0)
            {
                //db.CompareList = itmsCmprLst;
                Session["user_search"] = itmsCmprLst;

                //Session["user_compare_CategoryId"] = CategoryId;
                return Json(itmsCmprLst, JsonRequestBehavior.AllowGet); //myItems  "Hii"
            }
            else
                return Json(new List<Items>(), JsonRequestBehavior.AllowGet); //myItems  "Hii"


        }
        public List<string> itmsSrchNameLstGet()
        {
            List<string> details = Session["user_search"] as List<string>;

            return details;
        }
        //--------------
        public ActionResult set_guest_locat(List<string> latlon)
        {

            if (latlon.Count != 0)
            {
                Session["user_locat"] = latlon;
                return Json(latlon, JsonRequestBehavior.AllowGet); //myItems  "Hii"
            }
            else
                return Json(new List<Items>(), JsonRequestBehavior.AllowGet); //myItems  "Hii"


        }
        public List<string> get_guest_locat()
        {
            List<string> details = Session["user_locat"] as List<string>;

            return details;
        }

        //===========================================
        public ActionResult itmsCmprNameLstSet(List<string> itmsCmprLst)
        {

            if (itmsCmprLst.Count != 0)
            {
                //db.CompareList = itmsCmprLst;
                Session["user_compare"] = itmsCmprLst;
                //Session["user_compare_CategoryId"] = CategoryId;
                return Json(itmsCmprLst, JsonRequestBehavior.AllowGet); //myItems  "Hii"
            }
            else
            {
                List<string> details_lst = Session["user_compare"] as List<string>;
                if (details_lst.Count == 0)
                    return Json(new List<Items>(), JsonRequestBehavior.AllowGet); //myItems  "Hii"
                else
                    return Json(details_lst, JsonRequestBehavior.AllowGet); //myItems  "Hii"
            }


        }
        public List<string> itmsCmprNameLstGet()
        {
            List<string> details = Session["user_compare"] as List<string>;

            return details;
        }
        //----------
        public ActionResult itmsSrchNameLstSetasClear(List<Itemadder> itmsCmprLst)
        {
            if (itmsCmprLst == null)
                itmsCmprLst = new List<Itemadder>();
            Session.Remove("user_compare");
            Session["user_compare"] = new List<Itemadder>();
            //List<string> details = itmsSrchNameLstGet();
            return Json(new List<Itemadder>(), JsonRequestBehavior.AllowGet); //myItems  "Hii"
        }

        //==============  //==============  //==============
        // GET: /Item/Edit/5
        public ActionResult Edit(int id = 0)
        {///////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            ShopItemDBContext db1 = new ShopItemDBContext();
            var q1 = from a in db1.shopit where a.T_Id == id select a;
            int shopid = 0;
            foreach (var item in q1)
            {
                shopid = item.S_Id;

            }
            Cust_ShopDBContext cs = new Cust_ShopDBContext();
            var q2 = from b in cs.cust_shop where shopid == b.S_Id select b;
            int idd = 0;
            foreach (var item in q2)
            {
                idd = item.Cust_Id;
            }
            if (Session["swi"] != null && (int)Session["swi"] == idd)
            {
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                Items items = db.item.Find(id);
                if (items == null)
                {
                    return HttpNotFound();
                }
                Itemadder it = new Itemadder();
                it.p = new List<Properites>();
                it.pv = new List<string>();
                it.ind = new List<int>();
                it.item_id = id;
                it.item_name = items.T_Name;
                it.cat_id = items.Cat_Id;
                return View(it);
            }
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");

            return RedirectToAction("Index", "Customers");
            //  return RedirectToAction("Edit",it);
        }
        // POST: /Item/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Itemadder it)
        {
            ViewBag.message = "";
            //if (ModelState.IsValid)
            //{
            //    db.Entry(items).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(items);
            if (it.ind != null)
            {
                try
                {
                    if (it.item_name != null)
                    {
                        Items t = new Items(it.item_name, it.cat_id);
                        t.T_Id = it.item_id;
                        db.Entry(t).State = EntityState.Modified;
                        db.SaveChanges();
                    }


                    for (int i = 0; i < it.pv.Count; i++)
                    {
                        Item_value itv = new Item_value();
                        itv.P_id = it.ind[i];
                        itv.T_Id = it.item_id;
                        if (it.pv[i] == "")
                        {
                            itv.value = "Empty";
                        }
                        else
                        {
                            itv.value = it.pv[i];
                        }
                        itdp.Entry(itv).State = EntityState.Modified;

                    }
                    itdp.SaveChanges();



                    return RedirectToAction("img", it);
                }
                catch
                {
                    it.ind = null;
                    return View(it);
                }
            }
            else
            {

                it.p = new List<Properites>();
                it.pv = new List<string>();
                it.ind = new List<int>();
                int counter = 0;
                var q6 = from s in itdp.item_val
                         where s.T_Id == it.item_id

                         select s;


                foreach (var d in q6)
                {

                    counter++;
                    it.ind.Add(d.P_id);
                    it.pv.Add(d.value);

                }

                var q5 = from s in pdb.Properites

                         select s;

                foreach (var a in q5)
                {
                    if (it.ind.Contains(a.P_Id))
                    {
                        /* if (a.P_Name.ToLower() == "image")
                        {
                            Item_value i = new Item_value();
                            i.P_id = a.P_Id;
                            i.T_Id = it.item_id;
                            i.value = "/Content/Desktop/images/no-image.jpg";
                            itdp.item_val.Add(i);
                            itdp.SaveChanges();
                            for (int iv = 0; iv < it.ind.Count; iv++)
                            {
                                if (it.ind[iv] == a.P_Id)
                                {
                                    it.ind.RemoveAt(iv);
                                }
                            }


                        }*/
                        if (a.P_Name.ToLower() == "image" || a.P_Name.ToLower() == "date")
                        {
                            for (int iv = 0; iv < it.ind.Count; iv++)
                            {
                                if (it.ind[iv] == a.P_Id)
                                {
                                    it.ind.RemoveAt(iv);
                                    it.pv.RemoveAt(iv);
                                }
                            }
                        }
                        else
                        {

                            it.p.Add(a);
                        }

                    }


                }


                ViewBag.number = counter;
                return View(it);

            }
        }
        // GET: /Item/Delete/5
        public ActionResult Delete(int id = 0)
        {


            ShopItemDBContext db1 = new ShopItemDBContext();
            var q1 = from a in db1.shopit where a.T_Id == id select a;
            int shopid = 0;
            foreach (var item in q1)
            {
                shopid = item.S_Id;

            }
            Cust_ShopDBContext cs = new Cust_ShopDBContext();
            var q2 = from b in cs.cust_shop where shopid == b.S_Id select b;
            int idd = 0;
            foreach (var item in q2)
            {
                idd = item.Cust_Id;
            }
            if (Session["swi"] != null && (int)Session["swi"] == idd)
            {

                Items items = db.item.Find(id);
                if (items == null)
                {
                    return HttpNotFound();
                }
                return View(items);
            }
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");

            return RedirectToAction("Index", "Customers");
        }

        /// search

        public ActionResult SearchItemsNameUnsorted(string keyword, string Customer_name, string Category_name)
        {
            List<Itemadder> itmAddrLst = new List<Itemadder>();
            var myItems = from m in db.item select m;
            var myShopItems = from s in shopdb.shop select s;
            var myShops = from s in shopit.shopit select s;


            //var myCustm = from m in db
            var myCategories = from c in catdb.category select c;

            ViewBag.CategoryName = make_Category_Selectors_ViewBag(Category_name);
            ViewBag.Customer_name = make_Customer_Selectors_ViewBag(Customer_name);


            int cid = 0; string categoryName = "";
            int.TryParse(Category_name, out cid);
            if (Category_name != null && cid != 0)
                categoryName = myCategories.Where(s => s.Cat_Id == cid).First().Name;
            //ViewBag.myCategories = myCatg.ToList();
            //ViewBag.CustomerName = make_Category_Selectors_ViewBag(Customer_name);
            //if (Customer_name != null) categoryName = myCustm.Where(s => s.Cat_Id == cid).First().Name;

            int shpid = 0; string shpName = "";
            int.TryParse(Customer_name, out shpid);
            if (Customer_name != null && shpid != 0)
                shpName = myShopItems.Where(s => s.S_Id == shpid).First().About;


            if (!String.IsNullOrEmpty(keyword))
            {
                myItems = myItems.Where(s => s.T_Name.Contains(keyword));
                List<Itemadder> itmAdd_lst = new List<Itemadder>();
                foreach (var item in myItems.ToList())
                {
                    Categories c = myCategories.Where(s => s.Cat_Id == item.Cat_Id).ToList().First();
                    Shop_item si = myShops.Where(s => s.T_Id == item.T_Id).ToList().First();
                    Shop ss = myShopItems.Where(s => s.S_Id == si.S_Id).ToList().First();
                    string[] sLoc = ss.Location.Split('|');
                    double Lati = double.Parse(sLoc[0]);
                    double LocLong = double.Parse(sLoc[1]);
                    ShopLoc sl = new ShopLoc(si.S_Id, ss.About, sLoc[2], Lati, LocLong);
                    List<string> gust_ll = get_guest_locat();
                    double Lati2 = double.Parse(gust_ll[0]);
                    double LocLong2 = double.Parse(gust_ll[1]);
                    double mydis = distance(Lati, LocLong, Lati2, LocLong2);
                    mydis = double.Parse(String.Format("{0:0.000}", mydis));
                    itmAdd_lst.Add(new Itemadder(new Items(item.T_Name, item.T_Id), c, sl, mydis));

                }
                if (!String.IsNullOrEmpty(categoryName) || cid != 0)
                {
                    itmAdd_lst = itmAdd_lst.Where(s => s.cat_id == cid).ToList();
                    foreach (var itmAdd in itmAdd_lst) //  .ToList())
                    {
                        itmAddrLst = itmAdd_lst;
                        Categories c = myCategories.Where(s => s.Name == categoryName).ToList().First();
                        itmAdd.cat_id = c.Cat_Id;
                        itmAdd.cat_name = c.Name;
                    }
                    //itmAdd_lst = itmAddrLst.Where(s => s.cat_id == cid).ToList();
                    ///---------------Add Custmr---
                    if (!String.IsNullOrEmpty(shpName) || shpid != 0)
                    {
                        itmAdd_lst = itmAdd_lst.Where(s => s.shop.id == shpid).ToList();
                        foreach (var itmAdd in itmAdd_lst) //  .ToList())
                        {
                            itmAddrLst = itmAdd_lst;
                            //Shop c = myShopItems.Where(s => s.S_Id == shpid).ToList().First();
                            //itmAdd.shop_id = c.S_Id;
                            //itmAdd.cat_name = c.About;
                        }
                    }
                    ///===================
                    itmsSrchNameLstSet(itmAdd_lst); /// Session
                    return View(itmAdd_lst);
                }
                else
                {
                    ///---------------Add Custmr ---
                    if (!String.IsNullOrEmpty(shpName) || shpid != 0)
                    {
                        itmAdd_lst = itmAdd_lst.Where(s => s.shop.id == shpid).ToList();
                        foreach (var itmAdd in itmAdd_lst) //  .ToList())
                        {
                            itmAddrLst = itmAdd_lst;
                            //Shop c = myShopItems.Where(s => s.S_Id == shpid).ToList().First();
                            //itmAdd.shop_id = c.S_Id;
                            //itmAdd.cat_name = c.About;
                        }
                    }
                    ///===================
                    itmsSrchNameLstSet(itmAdd_lst); /// Session
                    itmAddrLst = itmAdd_lst;
                    return View(itmAddrLst);
                }
            }
            else
                return View(new List<Itemadder>());
        }
        //---------- with sort----------------------
        public ActionResult SearchItemsName(string keyword, string Customer_name, string Category_name, string sortBy)
        {
            List<Itemadder> SearchListReturn = new List<Itemadder>();

            ViewBag.NameSort = String.IsNullOrEmpty(sortBy) ? "Name desc" : "";
            ViewBag.CatgSort = (sortBy == "Category") ? "Category desc" : "Category";
            ViewBag.ShopNameSort = (sortBy == "ShopName") ? "ShopName desc" : "ShopName";
            ViewBag.ShopDistSort = (sortBy == "ShopDist") ? "ShopDist desc" : "ShopDist";


            List<Itemadder> itmAddrLst = new List<Itemadder>();
            var myItems = from m in db.item select m;
            var myShopItems = from s in shopdb.shop select s;
            var myShops = from s in shopit.shopit select s;


            //var myCustm = from m in db
            var myCategories = from c in catdb.category select c;

            ViewBag.CategoryName = make_Category_Selectors_ViewBag(Category_name);
            ViewBag.Customer_name = make_Customer_Selectors_ViewBag(Customer_name);


            int cid = 0; string categoryName = "";
            int.TryParse(Category_name, out cid);
            if (Category_name != null && cid != 0)
                categoryName = myCategories.Where(s => s.Cat_Id == cid).First().Name;
            //ViewBag.myCategories = myCatg.ToList();
            //ViewBag.CustomerName = make_Category_Selectors_ViewBag(Customer_name);
            //if (Customer_name != null) categoryName = myCustm.Where(s => s.Cat_Id == cid).First().Name;

            int shpid = 0; string shpName = "";
            int.TryParse(Customer_name, out shpid);
            if (Customer_name != null && shpid != 0)
                shpName = myShopItems.Where(s => s.S_Id == shpid).First().About;


            if (!String.IsNullOrEmpty(keyword))
            {
                myItems = myItems.Where(s => s.T_Name.Contains(keyword));
                List<Itemadder> itmAdd_lst = new List<Itemadder>();
                foreach (var item in myItems.ToList())
                {
                    Categories c = myCategories.Where(s => s.Cat_Id == item.Cat_Id).ToList().First();
                    Shop_item si = myShops.Where(s => s.T_Id == item.T_Id).ToList().First();
                    Shop ss = myShopItems.Where(s => s.S_Id == si.S_Id).ToList().First();
                    string[] sLoc = ss.Location.Split('|');
                    double Lati = double.Parse(sLoc[0]);
                    double LocLong = double.Parse(sLoc[1]);
                    ShopLoc sl = new ShopLoc(si.S_Id, ss.About, sLoc[2], Lati, LocLong);
                    List<string> gust_ll = get_guest_locat();
                    double mydis = 0;
                    if (gust_ll != null)
                    {
                        double Lati2 = double.Parse(gust_ll[0]);
                        double LocLong2 = double.Parse(gust_ll[1]);
                        mydis = distance(Lati, LocLong, Lati2, LocLong2);
                        mydis = double.Parse(String.Format("{0:0.000}", mydis));
                    }

                    itmAdd_lst.Add(new Itemadder(new Items(item.T_Name, item.T_Id), c, sl, mydis));
                }
                foreach (var cst in itmAdd_lst) // add price as count
                {
                    cst.item_id = (from j in db.item where j.T_Name == cst.item_name select j.T_Id).First();
                    var prop_id = (from j in pdb.Properites where j.P_Name == "Price" select j.P_Id).First();
                    var price_vl = (from j in itdp.item_val where j.T_Id == cst.item_id select j).ToList();
                    var price_val = from j in price_vl where j.P_id == prop_id select j.value;
                    List<string> price_l = price_val.ToList();
                    int out_pric = 0; bool b = int.TryParse(price_l.First(), out out_pric);
                    cst.counter = out_pric;
                }
                if (!String.IsNullOrEmpty(categoryName) || cid != 0)
                {
                    itmAdd_lst = itmAdd_lst.Where(s => s.cat_id == cid).ToList();
                    foreach (var itmAdd in itmAdd_lst) //  .ToList())
                    {
                        itmAddrLst = itmAdd_lst;
                        Categories c = myCategories.Where(s => s.Name == categoryName).ToList().First();
                        itmAdd.cat_id = c.Cat_Id;
                        itmAdd.cat_name = c.Name;
                    }
                    //itmAdd_lst = itmAddrLst.Where(s => s.cat_id == cid).ToList();
                    ///---------------Add Custmr---
                    if (!String.IsNullOrEmpty(shpName) || shpid != 0)
                    {
                        itmAdd_lst = itmAdd_lst.Where(s => s.shop.id == shpid).ToList();
                        foreach (var itmAdd in itmAdd_lst) //  .ToList())
                        {
                            itmAddrLst = itmAdd_lst;
                            //Shop c = myShopItems.Where(s => s.S_Id == shpid).ToList().First();
                            //itmAdd.shop_id = c.S_Id;
                            //itmAdd.cat_name = c.About;
                        }
                    }
                    ///===================
                    itmsSrchNameLstSet(itmAdd_lst); /// Session
                    SearchListReturn = itmAdd_lst;
                    //return View(itmAdd_lst);
                }
                else
                {
                    ///---------------Add Custmr ---
                    if (!String.IsNullOrEmpty(shpName) || shpid != 0)
                    {
                        itmAdd_lst = itmAdd_lst.Where(s => s.shop.id == shpid).ToList();
                        foreach (var itmAdd in itmAdd_lst) //  .ToList())
                        {
                            itmAddrLst = itmAdd_lst;
                            //Shop c = myShopItems.Where(s => s.S_Id == shpid).ToList().First();
                            //itmAdd.shop_id = c.S_Id;
                            //itmAdd.cat_name = c.About;
                        }
                    }
                    ///===================
                    itmsSrchNameLstSet(itmAdd_lst); /// Session
                    itmAddrLst = itmAdd_lst;
                    SearchListReturn = itmAddrLst;
                    //return View(itmAddrLst);
                }
            }
            else
            {
                SearchListReturn = new List<Itemadder>();
                //   return View();
            }
            if (SearchListReturn.Count != 0)
            {
                switch (sortBy)
                {
                    case "Name desc":
                        SearchListReturn = SearchListReturn.OrderByDescending(s => s.item_name).ToList();
                        break;
                    case "Name":
                        SearchListReturn = SearchListReturn.OrderBy(s => s.item_name).ToList();
                        break;
                    case "Category desc":
                        SearchListReturn = SearchListReturn.OrderByDescending(s => s.cat_name).ToList();
                        break;
                    case "Category":
                        SearchListReturn = SearchListReturn.OrderBy(s => s.cat_name).ToList();
                        break;
                    case "ShopName desc":
                        SearchListReturn = SearchListReturn.OrderByDescending(s => s.shop_name).ToList();
                        break;
                    case "ShopName":
                        SearchListReturn = SearchListReturn.OrderBy(s => s.shop_name).ToList();
                        break;
                    case "ShopDist desc":
                        SearchListReturn = SearchListReturn.OrderByDescending(s => s.distace).ToList();
                        break;
                    case "ShopDist":
                        SearchListReturn = SearchListReturn.OrderBy(s => s.distace).ToList();
                        break;
                    default:
                        SearchListReturn = SearchListReturn.OrderBy(s => s.distace).ToList();
                        break;
                }
            }
            return View(SearchListReturn);
        }
        //--------------------------------
        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        private double distance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = DegreeToRadian(lat2 - lat1);  // deg2rad below
            var dLon = DegreeToRadian(lon2 - lon1);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(DegreeToRadian(lat1)) * Math.Cos(DegreeToRadian(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
            ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;

        }

        public List<Itemadder> sort_list_by_inc(List<Itemadder> list, double Lati, double Longi, bool inc)
        {
            List<Itemadder> lst_out = new List<Itemadder>();
            List<KeyValuePair<int, double>> lst_sort = new List<KeyValuePair<int, double>>();
            List<double> lst_sort_dob = new List<double>();
            foreach (var item in list)
            {
                double d = (Math.Pow((Lati - item.LocLati), 2) + Math.Pow((Longi - item.LocLong), 2));
                double myDis = Math.Sqrt(d);
                Dictionary<int, double> dic = new Dictionary<int, double>();
                dic.Add(item.item_id, myDis);
                lst_sort.Add(dic.First());
                lst_sort_dob.Add(myDis);
            }
            if (inc)
                lst_sort_dob.Sort();
            else
                lst_sort_dob.Reverse();
            foreach (var itm in lst_sort_dob)
            {

                foreach (var item in list)
                {
                    double d = (Math.Pow((Lati - item.LocLati), 2) + Math.Pow((Longi - item.LocLong), 2));
                    double myDis = Math.Sqrt(d);

                    KeyValuePair<int, double> k = new KeyValuePair<int, double>();
                    int t_id = lst_sort.Where(s => s.Value == itm).ToList().First().Key;
                    lst_out.Add(list.Where(s => s.item_id == t_id).ToList().First());

                }


            }


            return lst_out;
        }
        //-----------------
        private List<Select_Item> make_Customer_Selectors_ViewBag(string id_select_value)
        {
            var myShops = from m in shopdb.shop select m;
            List<Select_Item> lst_Select_Item = new List<Select_Item>();
            foreach (var item in myShops)
                lst_Select_Item.Add(new Select_Item(item.S_Id, item.About, false));

            var shp_id = 0;
            if (id_select_value != null) shp_id = int.Parse(id_select_value);//Session["user_compare_CategoryId"];
            if (id_select_value != null)
            {
                if (shp_id != 0)
                    lst_Select_Item.Where(S => S.id == (int)shp_id).First().selected = true;
            }
            return lst_Select_Item;
        }
        private List<Select_Item> make_Category_Selectors_ViewBag(string id_select_value)
        {

            var myCatgs = from m in catdb.category select m;

            List<Select_Item> lst_Select_Item = new List<Select_Item>();
            foreach (var item in myCatgs)
                lst_Select_Item.Add(new Select_Item(item.Cat_Id, item.Name, false));

            var cat_id = 0;
            if (id_select_value != null) cat_id = int.Parse(id_select_value);//Session["user_compare_CategoryId"];
            if (id_select_value != null)
            {
                if (cat_id != 0)
                    lst_Select_Item.Where(S => S.id == (int)cat_id).First().selected = true;
            }
            return lst_Select_Item;

        }

        public Itemadder add_prop_item(Items item) //it here is old one , [More Active], it has itm_id,name  
        {
            var myCategories = from c in catdb.category select c;
            Itemadder it = new Itemadder();
            var ctg = myCategories.Where(s => s.Cat_Id == item.Cat_Id);
            List<Categories> ctgg = ctg.ToList();
            it.item_id = item.T_Id;
            it.item_name = item.T_Name;
            it.cat_id = item.Cat_Id;
            it.cat_name = ctgg[0].Name;
            //------------------

            //it.cat_id = , Cat_id,name
            //it.cat_name = 
            it.p = new List<Properites>();
            it.pv = new List<string>();
            it.ind = new List<int>();
            //it.pv.Clear();
            var q1 = from a in catpro.catpro
                     where a.Cat_Id == it.cat_id
                     select a;

            int counter = 0;
            //int c = 0;
            foreach (var a in q1)
            {
                counter++;
                it.ind.Add(a.P_Id);
            }
            string str = "";
            var q5 = from s in pdb.Properites
                     select s;

            foreach (var a in q5)
            {
                if (it.ind.Contains(a.P_Id))
                {
                    it.p.Add(a);

                    var ItmVal = from b in itdp.item_val
                                 where (a.P_Id == b.P_id && b.T_Id == it.item_id)
                                 select b;
                    List<Item_value> ls = ItmVal.ToList();
                    if (ls.Count != 0)
                        it.pv.Add(ls[0].value);
                }
            }
            it.counter = counter;
            return (it);
        }

        // POST: /Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShopItemDBContext db1 = new ShopItemDBContext();
            var q9001 = from a in db1.shopit where a.T_Id == id select a;
            int shopid = 0;
            foreach (var item in q9001)
            {
                shopid = item.S_Id;

            }
            Cust_ShopDBContext cs = new Cust_ShopDBContext();
            var q9002 = from b in cs.cust_shop where shopid == b.S_Id select b;
            int idd = 0;
            foreach (var item in q9002)
            {
                idd = item.Cust_Id;
            }
            if (Session["swi"] != null && (int)Session["swi"] == idd)
            {
                Items items = db.item.Find(id);
                db.item.Remove(items);
                var q = from a in itdp.item_val
                        where a.T_Id == id
                        select a;
                foreach (var item in q)
                {
                    itdp.item_val.Remove(item);
                }
                var q2 = from a in shopit.shopit
                         where a.T_Id == id
                         select a;
                foreach (var item in q2)
                {
                    shopit.shopit.Remove(item);
                }

                var q3 = from a in ratdb.itrat where a.Id == id select a;
                foreach (var item in q3)
                {
                    ratdb.itrat.Remove(item);
                }

                itdp.SaveChanges();
                shopit.SaveChanges();
                db.SaveChanges();
                ratdb.SaveChanges();
                return RedirectToAction("Index");
            }
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");

            return RedirectToAction("Index", "Customers");

        }
        //........................................................
        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Save_Img_Upload(Itemadder it)
        {

            if (Request != null)
            {
                it.file = Request.Files["file"];

                if (it.file != null)
                {
                    string pic = System.IO.Path.GetFileName(it.file.FileName);
                    //new 
                    it.shop_id = (from a in shopit.shopit where a.T_Id == it.item_id select a.S_Id).First();
                    it.cat_id = (from a in db.item where a.T_Id == it.item_id select a.Cat_Id).First();
                    it.cat_name = (from a in catdb.category where a.Cat_Id == it.cat_id select a.Name).First();
                    string shop_name = (from a in shopdb.shop where a.S_Id == it.shop_id select a.About).First();
                    string dirc_path = Server.MapPath("/Content/Desktop/images/" + shop_name + "/" + it.cat_name);
                    bool isExists = System.IO.Directory.Exists(dirc_path);
                    if (!isExists)
                        System.IO.Directory.CreateDirectory((dirc_path));
                    string path = System.IO.Path.Combine(dirc_path, pic);
                    // file is uploaded
                    it.file.SaveAs(path);

                    // save the image path path to the database or you can send image 
                    // directly to database
                    // in-case if you want to store byte[] ie. for DB
                    using (MemoryStream ms = new MemoryStream())
                    {
                        it.file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                    var q1 = from b in pdb.Properites where b.P_Name.ToLower() == "image" select b;
                    var pp = q1.ToList();
                    Properites p = pp.First();

                    //string categ_name = (from b in catdb.category where b.Cat_Id == it.item_id select b.Name).First();

                    var q = from a in itdp.item_val where a.T_Id == it.item_id && a.P_id == p.P_Id select a;
                    var qq = q.ToList();

                    foreach (var item in qq)
                    {
                        item.value = "/Content/Desktop/images/" + shop_name + "/" + it.cat_name + "/" + pic;
                    }
                    itdp.SaveChanges();
                    return RedirectToAction("Index", "Shop", it.shop_id);

                }
            }
            // after successfully uploading redirect the user

            return View(it);



        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Skip_Img_Upload(Itemadder it)
        {
            if (Request != null)
            {
                it.file = Request.Files["file"];

                if (it.file != null)
                {
                    return RedirectToAction("Index", "Shop", it.shop_id);

                }
            }
            // after successfully uploading redirect the user

            return View(it);



        }
        //..................................
        public ActionResult img(Itemadder it)
        {
            if (Request != null)
            {
                it.file = Request.Files["file"];

                if (it.file != null)
                {
                    string pic = System.IO.Path.GetFileName(it.file.FileName);
                    //new 
                    it.cat_id = (from a in db.item where a.T_Id == it.item_id select a.Cat_Id).First();
                    it.cat_name = (from a in catdb.category where a.Cat_Id == it.cat_id select a.Name).First();
                    string shop_name = (from a in shopdb.shop where a.S_Id == it.shop_id select a.About).First();
                    string dirc_path = Server.MapPath("/Content/Desktop/images/" + shop_name + "/" + it.cat_name);
                    bool isExists = System.IO.Directory.Exists(dirc_path);
                    if (!isExists)
                        System.IO.Directory.CreateDirectory((dirc_path));
                    string path = System.IO.Path.Combine(dirc_path, pic);
                    // file is uploaded
                    it.file.SaveAs(path);

                    // save the image path path to the database or you can send image 
                    // directly to database
                    // in-case if you want to store byte[] ie. for DB
                    using (MemoryStream ms = new MemoryStream())
                    {
                        it.file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                    var q1 = from b in pdb.Properites where b.P_Name.ToLower() == "image" select b;
                    var pp = q1.ToList();
                    Properites p = pp.First();

                    //string categ_name = (from b in catdb.category where b.Cat_Id == it.item_id select b.Name).First();

                    var q = from a in itdp.item_val where a.T_Id == it.item_id && a.P_id == p.P_Id select a;
                    var qq = q.ToList();

                    foreach (var item in qq)
                    {
                        item.value = "/Content/Desktop/images/" + shop_name + "/" + it.cat_name + "/" + pic;
                    }
                    itdp.SaveChanges();
                    return RedirectToAction("Index", "Shop", it.shop_id);

                }
            }
            // after successfully uploading redirect the user

            return View(it);






            //  return RedirectToAction("actionname", "controller name");
        }
        public ActionResult Prop(Itemadder it)
        {


            ViewBag.message = "";
            if (it.pv != null)
            {
                try
                {
                    bool en = false;
                    var t = from a in itdp.item_val where a.T_Id == it.item_id select a;
                    foreach (var j in t)
                    {
                        en = true;
                    }

                    if (it.pv.Contains("") == false || en == true)
                    {
                        
                        var q4 = from a in db.item
                                 where a.T_Name == it.item_name
                                 select a;
                        int tid = 0;
                        foreach (var a in q4)
                        {
                            tid = a.T_Id;
                        }

                        for (int i = 0; i < it.ind.Count; i++)
                        {
                            Item_value itm = new Item_value();
                            itm.T_Id = tid;
                            itm.P_id = it.ind[i];
                            if (it.pv[i] == "")
                            {
                                itm.value = "Empty";

                            }
                            else
                            {
                                itm.value = it.pv[i];
                            }
                            itdp.item_val.Add(itm);


                        }
                        itdp.SaveChanges();
                        int y = it.item_id;
                        Item_rate f = new Item_rate(y);
                        f.Id = y;
                        f.Down = 0;
                        f.Up = 0;
                        ratdb.itrat.Add(f);

                        ratdb.SaveChanges();

                        return RedirectToAction("img", it);
                    }

                    else
                    {
                        it.p = new List<Properites>();
                        it.pv = new List<string>();
                        it.ind = new List<int>();
                        it.pv.Clear();
                        var q1 = from a in catpro.catpro
                                 where a.Cat_Id == it.cat_id
                                 select a;
                        int counter = 0;
                        int c = 0;
                        foreach (var a in q1)
                        {
                            counter++;
                            it.ind.Add(a.P_Id);
                        }
                        string str = "";

                        var q5 = from s in pdb.Properites

                                 select s;

                        foreach (var a in q5)
                        {
                            if (it.ind.Contains(a.P_Id))
                            {
                                if (a.P_Name.ToLower() == "image")
                                {
                                    it.distace = 0;
                                    Item_value i = new Item_value();
                                    i.P_id = a.P_Id;
                                    i.T_Id = it.item_id;
                                    i.value = "/Content/Desktop/images/no-image.jpg";
                                    itdp.item_val.Add(i);
                                    itdp.SaveChanges();
                                }
                                else if (a.P_Name.ToLower() == "date")
                                {
                                    Item_value i = new Item_value();
                                    i.P_id = a.P_Id;
                                    i.T_Id = it.item_id;
                                    i.value = DateTime.Now.ToString();
                                    itdp.item_val.Add(i);
                                    itdp.SaveChanges();
                                }
                                else
                                {
                                    it.p.Add(a);
                                    it.pv.Add("");
                                }

                            }
                            //foreach (var d in q4)
                            //{

                            //    //it.p.Add(d);
                            //}

                        }


                        ViewBag.number = counter;
                        ViewBag.message = "INVALID ENTRY";
                        return View(it);

                    }
                }

                catch
                {
                    ViewBag.message = "INVALID ENTRY";
                    it.pv = null;
                    return View(it);
                }

                //  return View(it);
            }
            else
            {
                /// why dont use add_prop_item function ??!!!
                it.p = new List<Properites>();
                it.pv = new List<string>();
                it.ind = new List<int>();
                it.pv.Clear();
                var q1 = from a in catpro.catpro
                         where a.Cat_Id == it.cat_id
                         select a;
                int counter = 0;
                int c = 0;
                foreach (var a in q1)
                {
                    counter++;
                    it.ind.Add(a.P_Id);
                }
                string str = "";

                var q5 = from s in pdb.Properites

                         select s;

                foreach (var a in q5)
                {
                    if (it.ind.Contains(a.P_Id))
                    {
                        if (a.P_Name.ToLower() == "image")
                        {
                            Item_value i = new Item_value();
                            i.P_id = a.P_Id;
                            i.T_Id = it.item_id;
                            i.value = "/Content/Desktop/images/no-image.jpg";
                            itdp.item_val.Add(i);
                            itdp.SaveChanges();
                            for (int iv = 0; iv < it.ind.Count; iv++)
                            {
                                if (it.ind[iv] == a.P_Id)
                                {
                                    it.ind.RemoveAt(iv);
                                }
                            }


                        }
                        else if (a.P_Name.ToLower() == "date")
                        {
                            Item_value i = new Item_value();
                            i.P_id = a.P_Id;
                            i.T_Id = it.item_id;
                            i.value = DateTime.Now.ToString();
                            itdp.item_val.Add(i);
                            itdp.SaveChanges();
                            for (int iv = 0; iv < it.ind.Count; iv++)
                            {
                                if (it.ind[iv] == a.P_Id)
                                {
                                    it.ind.RemoveAt(iv);
                                }
                            }
                        }
                        else
                        {
                            it.p.Add(a);
                            it.pv.Add("");
                        }
                    }


                    //foreach (var d in q4)
                    //{

                    //    //it.p.Add(d);
                    //}

                }


                ViewBag.number = counter;

                return View(it);
            }
        }

        private Itemadder add_prop_item(Itemadder it) //it here is old one 
        {

            it.p = new List<Properites>();
            it.pv = new List<string>();
            it.ind = new List<int>();
            //it.pv.Clear();
            var q1 = from a in catpro.catpro
                     where a.Cat_Id == it.cat_id
                     select a;

            int counter = 0;
            int c = 0;
            foreach (var a in q1)
            {
                counter++;
                it.ind.Add(a.P_Id);
            }
            string str = "";
            var q5 = from s in pdb.Properites
                     select s;

            foreach (var a in q5)
            {
                if (it.ind.Contains(a.P_Id))
                {
                    it.p.Add(a);
                    it.pv.Add("");
                }
            }
            it.counter = counter;
            return (it);
        }
        //...............................................................


        
        //...............................................................
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}