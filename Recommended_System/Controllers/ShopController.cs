using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Recommended_System.Models;

namespace Recommended_System.Controllers
{
    public class ShopController : Controller
    {
        private shopDBContext db = new shopDBContext();
        private ShopItemDBContext sdb = new ShopItemDBContext();
        private ItemDBContext itdb = new ItemDBContext();
        private CatDBContext catdb = new CatDBContext();
        private ItemvalueDBContext itp = new ItemvalueDBContext();
        private RRDBContext ratdb = new RRDBContext();
        private Cust_ShopDBContext ccc = new Cust_ShopDBContext();
        //
        // GET: /Shop/

        public ActionResult Index()
        {
            if (Session["swi"] == null)
                return View(db.shop.ToList());

            else if (Session["isadmin"] == "no")
            {
                int id = (int)Session["swi"];
                var s = from b in ccc.cust_shop where b.Cust_Id == id select b;
                int shopid = 0;
                List<Shop> sh = new List<Shop>();
                foreach (var item in s)
                {
                    shopid = item.S_Id;
                    var q = from a in db.shop where a.S_Id == shopid select a;
                    foreach (var item2 in q)
                    {
                        sh.Add(item2);

                    }

                }

                return View(sh);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult guest_show(int id)
        {

            Shop_adder s = new Shop_adder();
            s.indx = new List<int>();
            s.it = new List<Itemadder>();
            s.sv = new List<string>();
            s.categ = new List<string>();
            s.shp = new Shop();


            var q1 = from a in sdb.shopit where a.S_Id == id select a;

            foreach (var sh in q1)
            {
                int x = sh.T_Id;
                var t = from b in itdb.item where b.T_Id == x select b;
                Items it = t.First();

                s.sv.Add(it.T_Name);
                s.indx.Add(it.T_Id);
                int c = it.Cat_Id;
                var m = from a in catdb.category where a.Cat_Id == c select a;
                Categories ca = m.First();
                s.categ.Add(ca.Name);
            }
            var q2 = from a in db.shop where a.S_Id == id select a;
            s.shp = q2.First();
            s.shop_name = q2.First().About;
            s.shop_id = id;
            return View(s);


        }



        public ActionResult edmap(List<string> shopmap)
        {
            if (shopmap.Count != 0)
            {
                int i = Convert.ToInt16(shopmap[3]);

                Shop x = new Shop();
                var q = from a in db.shop where a.S_Id == i select a;

                if (String.IsNullOrEmpty( shopmap[0])) shopmap[0] = "0.0" ;
                if (String.IsNullOrEmpty(shopmap[0])) shopmap[1] = "0.0" ;
                if (String.IsNullOrEmpty(shopmap[0])) shopmap[1] = "Sorry!! not found .";
                foreach (var h in q)
                {
                    h.Location = shopmap[0] + "|" + shopmap[1] + "|" + shopmap[2];

                }
                db.SaveChanges();


                return RedirectToAction("Index");
            }
            else
                return Json(new List<Items>(), JsonRequestBehavior.AllowGet); //myItems  "Hii"
        }


        public ActionResult detmap(int id)
        {
            Shop_adder s = new Shop_adder();
            var sh = from a in db.shop where a.S_Id == id select a;
            Shop sp = sh.First();

            string[] x = sp.Location.Split('|');
            s.first = x[0];
            s.secound = x[1];
            s.shop_id = id;
            s.shop_name = sp.About;




            return View(s);



        }

        public ActionResult mapping(List<string> shopmap)
        {
            if (shopmap.Count != 0)
            {
                int id = 0;
                Shop x = new Shop();
                var q = from a in db.shop select a;

                foreach (var sh in q)
                {
                    id = sh.S_Id;

                }
                var result = from t in db.shop
                             orderby t.S_Id descending
                             select t;

                Shop f = result.First();
                id = f.S_Id;
                /* var xx = from a in ratdb.itrat where a.Id == tid select a;
                    foreach (var h in xx)
                    {
                        h.Down = h.Down + 1;

                    }
                    ratdb.SaveChanges();*/

                var xx = from a in db.shop where a.S_Id == id select a;
                foreach (var h in xx)
                {
                    h.Location = shopmap[0] + "|" + shopmap[1] + "|" + shopmap[2];

                }



                db.SaveChanges();


                return RedirectToAction("Index");
            }
            else
                return Json(new List<Items>(), JsonRequestBehavior.AllowGet); //myItems  "Hii"


        }

        public ActionResult addmap(List<string> shopmap)
        {
            int id = 0;
            Shop x = new Shop();
            var q = from a in db.shop select a;

            foreach (var sh in q)
            {
                id = sh.S_Id;
                x.About = sh.About;
                x.Delevery = sh.Delevery;
                x.Phone = sh.Phone;
            }

            x.Location = shopmap[0] + "|" + shopmap[1] + "|" + shopmap[2];

            db.Entry(x).State = EntityState.Modified;
            db.SaveChanges();


            return RedirectToAction("Index");
        }

        public ActionResult mapv()
        {
            //take care h3 h3 h3
            var result = from t in db.shop

                         orderby t.S_Id descending
                         select t;
            Shop x = result.First();
            if (x.Location == "")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        //
        // GET: /Shop/Details/5

        public ActionResult Details(int id = 0)
        {
            Shop shop = db.shop.Find(id);
            if (shop == null)
            {
                return HttpNotFound();
            }
            return View(shop);
        }

        //
        // GET: /Shop/Create

        public ActionResult Create()
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "yes")
                return RedirectToAction("Index", "Customers");
            return View();
        }


        //
        // POST: /Shop/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Shop shop)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "yes")
                return RedirectToAction("Index", "Customers");

            if (ModelState.IsValid)
            {
                if (shop != null)
                {
                    Shop x = new Shop();

                    x.Delevery = shop.Delevery;
                    x.Phone = shop.Phone;
                    x.About = shop.About;
                    x.Location = "";
                    Cust_ShopDBContext csss = new Cust_ShopDBContext();
                    int id = (int)Session["swi"];
                    int count = (from a in csss.cust_shop where a.Cust_Id == id select a).Count();
                    CustomersDBContext cst = new CustomersDBContext();
                    var qq2 = from b in cst.customer where id == b.Cust_Id select b;
                    int accountcount = 0;
                    foreach (var item in qq2)
                    {
                        accountcount = item.Account_count;
                    }
                    if (count < accountcount)
                    {
                        if (shop.About ==null || shop.Delevery ==null)
                        {
                            return RedirectToAction("Index");
                        }
                        db.shop.Add(x);
                        db.SaveChanges();

                        Cust_ShopDBContext cs = new Cust_ShopDBContext();
                        Cust_Shop cos = new Cust_Shop();
                        cos.Cust_Id = id;
                        cos.S_Id = x.S_Id;
                        cs.cust_shop.Add(cos);
                        cs.SaveChanges();



                        return RedirectToAction("mapv");
                    }

                    TempData["counters"]= "Sorry You already have the limit shops ";
                    return RedirectToAction("Index", "Customers");


                }
                else
                {
                    Shop s = new Shop();

                    return View(s);








                }
            }

            return View(shop);
        }



        public ActionResult shop_it(int id)
        {

            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "yes")
                return RedirectToAction("Index", "Customers");

            Cust_ShopDBContext csd = new Cust_ShopDBContext();
            var qwee = from o in csd.cust_shop where o.S_Id == id select o;
            int opq = 0;
            foreach (var item in qwee)
            {
                opq = item.Cust_Id;

            }
            int swint = (int)Session["swi"];
            if (opq != swint)
                return RedirectToAction("Index", "Customers");

            Shop_adder s = new Shop_adder();
            s.categ = new List<string>();
            s.indx = new List<int>();
            s.it = new List<Itemadder>();
            s.sv = new List<string>();
            s.shop_id = id;
            var q1 = from a in sdb.shopit where a.S_Id == id select a;

            foreach (var sh in q1)
            {
                int x = sh.T_Id;
                var t = from b in itdb.item where b.T_Id == x select b;
                Items it = t.First();

                s.sv.Add(it.T_Name);
                s.indx.Add(it.T_Id);
                int c = it.Cat_Id;
                var m = from a in catdb.category where a.Cat_Id == c select a;
                Categories ca = m.First();
                s.categ.Add(ca.Name);
            }
            var q2 = from a in db.shop where a.S_Id == id select a;
            s.shop_name = q2.First().About;
            return View(s);









        }



        //
        // GET: /Shop/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "yes")
                return RedirectToAction("Index", "Customers");

            Cust_ShopDBContext csd = new Cust_ShopDBContext();
            var qwee = from o in csd.cust_shop where o.S_Id == id select o;
            int opq = 0;
            foreach (var item in qwee)
            {
                opq = item.Cust_Id;

            }
            int swint = (int)Session["swi"];
            if (opq != swint)
                return RedirectToAction("Index", "Customers");


            Shop shop = db.shop.Find(id);
            if (shop == null)
            {
                return HttpNotFound();
            }
            return View(shop);
        }

        //
        // POST: /Shop/Edit/5

        public ActionResult Prop(Shop xx)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "yes")
                return RedirectToAction("Index", "Customers");
            Shop_adder s = new Shop_adder();
            var sh = from a in db.shop where a.S_Id == xx.S_Id select a;
            Shop sp = sh.First();

            string[] x = sp.Location.Split('|');
            s.first = x[0];
            s.secound = x[1];
            s.shop_id = xx.S_Id;
            s.shop_name = sp.About;




            return View(s);

           
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Shop shop)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "yes")
                return RedirectToAction("Index", "Customers");

            if (ModelState.IsValid)
            {
                /* var xx = from a in db.shop where a.S_Id == id select a;
                foreach (var h in xx)
                {
                    h.Location = shopmap[0] + "|" + shopmap[1] + "|" + shopmap[2]; 
                   
                }

               

                db.SaveChanges();
                 *
*/
                if (shop.About == null || shop.Delevery == null||shop.Phone==null)
                {
                    return RedirectToAction("Index");
                }
                var xx = from a in db.shop where a.S_Id == shop.S_Id select a;
                foreach (var h in xx)
                {
                    h.Phone = shop.Phone;
                    h.About = shop.About;
                    h.Delevery = shop.Delevery;

                }

                db.SaveChanges();
                return RedirectToAction("Prop", shop);

            }
            return View(shop);
        }

        //
        // GET: /Shop/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "yes")
                return RedirectToAction("Index", "Customers");

            Cust_ShopDBContext csd = new Cust_ShopDBContext();
            var qwee = from o in csd.cust_shop where o.S_Id == id select o;
            int opq = 0;
            foreach (var item in qwee)
            {
                opq = item.Cust_Id;

            }
            int swint = (int)Session["swi"];
            if (opq != swint)
                return RedirectToAction("Index", "Customers");
            Shop shop = db.shop.Find(id);
            //new 
            if (!String.IsNullOrEmpty(shop.Location))
            { 
                string[] str = shop.Location.Split('|');
                shop.Location = str[2];
            }
            
            if (shop == null)
            {
                return HttpNotFound();
            }
            return View(shop);
        }

        //
        // POST: /Shop/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "yes")
                return RedirectToAction("Index", "Customers");

            Cust_ShopDBContext csd = new Cust_ShopDBContext();
            Cust_Shop csop = new Cust_Shop();
            var qwee = from o in csd.cust_shop where o.S_Id == id select o;
            int opq = 0;
            foreach (var item in qwee)
            {
                opq = item.Cust_Id;
                csop = item;

            }
            int swint = (int)Session["swi"];
            if (opq != swint)
                return RedirectToAction("Index", "Customers");
            csd.cust_shop.Remove(csop);
            csd.SaveChanges();

            var s = from a in sdb.shopit where a.S_Id == id select a;
            foreach (var q in s)
            {
                Shop_item x = new Shop_item();
                x = q;
                sdb.shopit.Remove(q);
                var result = from t in itdb.item
                             where t.T_Id == x.T_Id
                             orderby t.T_Id descending
                             select t;
                Items ti = result.First();
                itdb.item.Remove(ti);
                var q2 = from a in itp.item_val
                         where a.T_Id == x.T_Id
                         select a;
                foreach (var item in q2)
                {
                    itp.item_val.Remove(item);
                }
                var q3 = from a in ratdb.itrat where a.Id == ti.T_Id select a;
                foreach (var item in q3)
                {
                    ratdb.itrat.Remove(item);
                }



            }


            Shop shop = db.shop.Find(id);
            db.shop.Remove(shop);
            /*itdp.SaveChanges();
            shopit.SaveChanges();
            db.SaveChanges();
            ratdb.SaveChanges();*/
            itp.SaveChanges();
            sdb.SaveChanges();
            itdb.SaveChanges();
            
            db.SaveChanges();
            ratdb.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}