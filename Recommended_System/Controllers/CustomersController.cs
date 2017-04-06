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
    public class CustomersController : Controller
    {
        private CustomersDBContext db = new CustomersDBContext();
        private Av_Cust2DBContext db2 = new Av_Cust2DBContext();
        private Av_CustDBContext db3 = new Av_CustDBContext();
        private SubScription_KindtDBContext db4 = new SubScription_KindtDBContext();
        private Cs_SuDBContext db5 = new Cs_SuDBContext();

        public void rm(int customer_id)
        {
            RRDBContext ratedb = new RRDBContext();
            ItemvalueDBContext itemvaluedb = new ItemvalueDBContext();
            ShopItemDBContext shopitemdb = new ShopItemDBContext();
            ItemDBContext itemdb = new ItemDBContext();
            Cust_ShopDBContext custshopdb = new Cust_ShopDBContext();
            shopDBContext shopdb = new shopDBContext();
            //Cs_SuDBContext cssudb = new Cs_SuDBContext();
            //CustomersDBContext customerdb = new CustomersDBContext();

            int shop_id;
            var q1 = from a1 in custshopdb.cust_shop where a1.Cust_Id == customer_id select a1;
            foreach (var item1 in q1.ToList())
            {
                shop_id = item1.S_Id;
                var q2 = from a2 in shopitemdb.shopit where a2.S_Id == shop_id select a2;
                int item_id;
                Shop_item si;
                foreach (var item2 in q2.ToList())
                {
                    item_id = item2.T_Id;
                    var q3 = from a3 in itemvaluedb.item_val where item_id == a3.T_Id select a3;
                    Item_value iv;
                    foreach (var item3 in q3.ToList())
                    {
                        iv = item3;
                        itemvaluedb.item_val.Remove(iv);
                        itemvaluedb.SaveChanges();
                    }
                    si = item2;
                    shopitemdb.shopit.Remove(si);
                    shopitemdb.SaveChanges();

                    var q4 = from a4 in itemdb.item where a4.T_Id == item_id select a4;
                    Items it;
                    foreach (var item4 in q4.ToList())
                    {
                        it = item4;
                        itemdb.item.Remove(it);
                        itemdb.SaveChanges();

                        Item_rate r = ratedb.itrat.Find(it.T_Id);
                        ratedb.itrat.Remove(r);
                        ratedb.SaveChanges();



                    }

                }
                Cust_Shop cs = item1;
                custshopdb.cust_shop.Remove(cs);
                custshopdb.SaveChanges();

                Shop s;
                var q5 = from a5 in shopdb.shop where a5.S_Id == shop_id select a5;
                foreach (var item5 in q5.ToList())
                {
                    s = item5;
                    shopdb.shop.Remove(s);
                    shopdb.SaveChanges();

                }

            }

            var q6 = from a6 in db5.cs_su where a6.Customer == customer_id select a6;
            Cs_Su cssu;
            foreach (var item6 in q6.ToList())
            {
                cssu = item6;
                db5.cs_su.Remove(cssu);
                db5.SaveChanges();

            }

            Customers custome = db.customer.Find(customer_id);
            db.customer.Remove(custome);
            db.SaveChanges();

        }
        //private Financial2DBContext f = new Financial2DBContext();
        //
        // GET: /Customers/

        public ActionResult Index()
        {

            if (TempData["counters"] != null)
                ViewBag.counters = TempData["counters"];


            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            int id = (int)Session["swi"];
            ViewBag.id = id;
            var s = from i in db.customer where i.Cust_Id == id select i.Cust_Nme;

            foreach (var item in s)
            {
                ViewBag.nam = item;


            }
            if (Session["isadmin"] == "yes")
            {
                //admin work
                int x = db2.av_cust2.Count();
                ViewBag.wlst = x;
            }
            else if (Session["isadmin"] == "no")
            {
                //customer work
            }

            return View();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////


        //////////////////////////////////////////////////////////////////////////////////////////
        //
        // GET: /Customers/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            Customers customers = db.customer.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        //
        // GET: /Customers/Create/6

        public ActionResult Create(int id = 0)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");

            if (Session["isadmin"] == "no")
                return RedirectToAction("../LogOn");
            Av_Cust2 avc = db2.av_cust2.Find(id);
            Customers cust = new Customers(avc.Cust_Nme, avc.Phone, avc.E_mail, avc.Account_count, avc.Start_Time, avc.Password);
            cust.Cust_Id = id;
            var q = from a in db4.subscription_kind where a.Type == avc.Sup_Type select a;
            int sub_id = 0;
            decimal price = 0;
            int tim = 0;

            foreach (var item in q)
            {
                sub_id = item.Sub_Id;
                price = item.Price;
                tim = item.Time_Period;

            }

            //     Financial2 fn = new Financial2(id, price);
            //   f.fin.Add(fn);
            // f.SaveChanges();
            cust.Payment = (int)price;
            db.customer.Add(cust);
            db.SaveChanges();

            var ew = from t in db.customer orderby t.Cust_Id descending select t;

            Customers cs234 = ew.First();
            id = cs234.Cust_Id;
            DateTime Finish = avc.Start_Time.AddDays(tim);
            Cs_Su cssu = new Cs_Su(id, sub_id, avc.Start_Time, Finish);
            db5.cs_su.Add(cssu);
            db5.SaveChanges();

            db2.av_cust2.Remove(avc);
            db2.SaveChanges();

            return RedirectToAction("Index");
        }


        /////////////////////////////////////////////////////////////////////////////////


        //
        // GET: /Customers/Renew/6

        public ActionResult Renew(int id = 0)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");

            if (Session["isadmin"] == "no")
                return RedirectToAction("../LogOn");
            Av_Cust2 avc = db2.av_cust2.Find(id);
            Customers cust = new Customers() ;

            var qsd = from kli in db.customer where kli.E_mail == avc.E_mail select kli;
            foreach (var item in qsd)
            {
                cust = item;
                
            }


            var q = from a in db4.subscription_kind where a.Type == avc.Sup_Type select a;
            int sub_id = 0;
            decimal price = 0;
            int tim = 0;

            foreach (var item in q)
            {
                sub_id = item.Sub_Id;
                price = item.Price;
                tim = item.Time_Period;

            }

            cust.Payment += (int)price*cust.Account_count;

            db.Entry(cust).State = EntityState.Modified;          
            db.SaveChanges();


            
            var ew = from t in db5.cs_su where t.Customer==cust.Cust_Id orderby t.ID descending select t;
            Cs_Su cssu = ew.First();
         cssu.Finish=   cssu.Finish.AddDays(tim);
            cssu.Sub = sub_id;
            db5.Entry(cssu).State = EntityState.Modified;
            db5.SaveChanges();

            db2.av_cust2.Remove(avc);
            db2.SaveChanges();

            return RedirectToAction("Index");
        }


        /////////////////////////////////////////////////////////////////////////////////

        //
        // POST: /Customers/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customers customers)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                db.customer.Add(customers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customers);
        }

        //
        // GET: /Customers/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            id = (int)Session["swi"];
            Customers customers = db.customer.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        //
        // POST: /Customers/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customers customers)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            customers.Cust_Id = (int)Session["swi"];
            var x = from a in db.customer where a.Cust_Id == customers.Cust_Id select a.Start_Time;
            DateTime t = new DateTime();
            foreach (var item in x)
            {
                t = item;

            }
            customers.Start_Time = t;

            if (ModelState.IsValid)
            {
                db.Entry(customers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customers);
        }

        //
        // GET: /Customers/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["isadmin"] == "no")
                return RedirectToAction("Index");
            Av_Cust2 customers = db2.av_cust2.Find(id);
            db2.av_cust2.Remove(customers);
            db2.SaveChanges();

            if (customers == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        //
        // POST: /Customers/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["isadmin"] == "no")
                return RedirectToAction("Index");
            rm(id);
            return RedirectToAction("Index");
        }

        //
        // GET: /Customers/Waiting

        public ActionResult Waiting()
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["isadmin"] == "no")
                return RedirectToAction("../LogOn");

            List<Av_Cust2> av_cu = new List<Av_Cust2>();
            bool flag = true;
            var q3 = from o in db2.av_cust2 select o;
            foreach (var item in q3)
            {
                var qss = from qs in db.customer where qs.E_mail == item.E_mail select qs;
                bool isqssempty = true;
                foreach (var item2 in qss)
                {
                    isqssempty = false;

                }
                if (!isqssempty)
                    item.Cust_Id = item.Cust_Id - 1000000000;


                av_cu.Add(item);
                flag = false;
            }
            if (flag)
                return RedirectToAction("Index");


            return View(av_cu);
        }


        //
        // GET: /Customers/ListCustomers

        public ActionResult ListCustomers()
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["isadmin"] == "no")
                return RedirectToAction("../LogOn");

            List<Customers> customers = new List<Customers>();
            bool flag = true;
            var q3 = from o in db.customer where o.Cust_Id > 0 select o;
            foreach (var item in q3)
            {
                customers.Add(item);
                flag = false;
            }
            if (flag)
                return RedirectToAction("Index");


            return View(customers);
        }

        //
        // GET: /Customers/AdminMaker/5

        public ActionResult AdminMaker(int id = 0)
        {

            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["isadmin"] == "no")
                return RedirectToAction("../LogOn");
            Customers cs = db.customer.Find(id);
            cs.Admin = 1;
            db.Entry(cs).State = EntityState.Modified;
            db.SaveChanges();
            TempData["counters"] = "You have a new Admin now";
            return RedirectToAction("Index");

        }


        //
        // GET: /Customers/AdminBreaker/5

        public ActionResult AdminBreaker(int id = 0)
        {

            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["isadmin"] == "no")
                return RedirectToAction("../LogOn");
            if ((int)Session["swi"] == id)
            {
                int count = (from a in db.customer where a.Admin == 1 select a).Count();
                if (count == 1)
                {
                    TempData["counters"] = "You can't Return as Customer";

                    return RedirectToAction("Index");

                }


            }
            Customers cs = db.customer.Find(id);
            cs.Admin = 0;
            db.Entry(cs).State = EntityState.Modified;
            db.SaveChanges();
            TempData["counters"] = "You have Remove an Admin now";
            if ((int)Session["swi"] == id)
            {
                Session["isadmin"] = "no";
                TempData["counters"] = "You Return as Customer";
            }
            return RedirectToAction("Index");

        }


        //
        // GET: /Customers/Remove/5

        public ActionResult Remove(int id = 0)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["isadmin"] == "no")
                return RedirectToAction("../LogOn");

            rm(id);


            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}