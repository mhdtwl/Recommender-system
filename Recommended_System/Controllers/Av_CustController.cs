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
    public class Av_CustController : Controller
    {
        private Av_CustDBContext db = new Av_CustDBContext();
        private Av_Cust2DBContext db2 = new Av_Cust2DBContext();
        private CustomersDBContext db3 = new CustomersDBContext();
        private Cs_SuDBContext db4 = new Cs_SuDBContext();
        private SubScription_KindtDBContext susc = new SubScription_KindtDBContext();
        //
        // GET: /Av_Cust/

        public ActionResult Index()
        {
            return View(db.av_cust.ToList());
        }

        //
        // GET: /Av_Cust/Details/5

        public ActionResult Details(int id = 0)
        {
            Av_Cust av_cust = db.av_cust.Find(id);
            if (av_cust == null)
            {
                return HttpNotFound();
            }
            return View(av_cust);
        }

        //
        // GET: /Av_Cust/Create

        public ActionResult Create()
        {
            SubScription_KindtDBContext sbdb = new SubScription_KindtDBContext();
            //Database.SetInitializer<SubScription_KindtDBContext>(null);
            var q = from a in sbdb.subscription_kind select a;
            List<SubScription_Kind2> sb = new List<SubScription_Kind2>();
            foreach (var item in q)
            {
                sb.Add(item);

            }
            ViewBag.sss = sb;
            return View();
        }

        //
        // POST: /Av_Cust/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Av_Cust av_cust)
        {

            SubScription_KindtDBContext sbdb = new SubScription_KindtDBContext();
            var q = from a in sbdb.subscription_kind select a;
            //var io = from hp in db2.av_cust2 select hp.Cust_Id;
            //var w = from aa in db3.customer select aa.Cust_Id;
            //int wasd = 0;

            //foreach (var item in io)
            //{
            //    if (item > wasd)
            //        wasd = item;

            //}
            //int x = 0;
            //foreach (var item in w)
            //{
            //    if (item > x)
            //        x = item;
            //}
            //wasd = Math.Max(wasd, x);
            List<SubScription_Kind2> sb = new List<SubScription_Kind2>();
            foreach (var item in q)
            {
                sb.Add(item);

            }
            ViewBag.sss = sb;

            if (ModelState.IsValid)
            {
                if (av_cust.Password != av_cust.Passwordco)
                {
                    TempData["pass"] = "Password And Confirmation are not compatible";
                    return View(av_cust);
                }
                av_cust.Start_Time = DateTime.Now;
                string snam = Request.Form["snam"];
                //Cs_Su cssu = new Cs_Su(wasd + 1, snam);
                //var qq = from a in sbdb.subscription_kind where a.Sub_Id == snam select a;
                //foreach (var item in qq)
                //{
                //    cssu.Start = DateTime.Now;
                //    cssu.Start = cssu.Start.AddDays(1);

                //    cssu.Finish = DateTime.Now;
                //    cssu.Finish = cssu.Finish.AddDays(item.Time_Period + 1);

                //}
                Av_Cust2 av2 = new Av_Cust2(av_cust.Cust_Nme, av_cust.Phone, av_cust.E_mail, av_cust.Account_count, av_cust.Start_Time, av_cust.Password, snam);
                db2.av_cust2.Add(av2);
                db2.SaveChanges();
                int pr = 0;
                foreach (var item in q)
                {
                    if (item.Type == av2.Sup_Type)
                    {
                        pr = (int)item.Price;
                        pr = pr * ((int)av2.Account_count);
                    }

                }



                //db4.cs_su.Add(cssu);              
                //db4.SaveChanges();
                TempData["pr"] = pr;
                return RedirectToAction("Wait");
            }

            return View(av_cust);
        }
        //////////////////////////////////////////////////////////////////////////////


        //
        // GET: /Av_Cust/Renew

        public ActionResult Renew()
        {
              if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            int id = (int)Session["swi"];
            SubScription_KindtDBContext sbdb = new SubScription_KindtDBContext();
           
            var q = from a in sbdb.subscription_kind select a;
            List<SubScription_Kind2> sb = new List<SubScription_Kind2>();
            foreach (var item in q)
            {
                sb.Add(item);

            }
            ViewBag.sss = sb;

           
            return View();
        }

        //
        // POST: /Av_Cust/Renew

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Renew(Av_Cust av_cust)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            int id = (int)Session["swi"];
            
            Customers cus = new Customers();
            var q2 = from d in db3.customer where d.Cust_Id == id select d;
            foreach (var item in q2)
            {
                cus = item;

            }
            SubScription_KindtDBContext sbdb = new SubScription_KindtDBContext();
            var q = from a in sbdb.subscription_kind select a;
                   
                av_cust.Start_Time = cus.Start_Time;
                string snam = Request.Form["snam"];
                av_cust = new Av_Cust(cus.Cust_Nme, cus.Phone, cus.E_mail, cus.Account_count, cus.Start_Time, cus.Password);
              
                Av_Cust2 av2 = new Av_Cust2(av_cust.Cust_Nme, av_cust.Phone, av_cust.E_mail, av_cust.Account_count, av_cust.Start_Time, av_cust.Password, snam);
                db2.av_cust2.Add(av2);
                db2.SaveChanges();
                int pr = 0;
                foreach (var item in q)
                {
                    if (item.Type == av2.Sup_Type)
                    {
                        pr = (int)item.Price;
                        pr = pr * ((int)av2.Account_count);
                    }
                }

                TempData["pr"] = pr;
                return RedirectToAction("Wait");
            

            
        }



        //////////////////////////////////////////////////////////////////////////////
        //
        // GET: /Av_Cust/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Av_Cust av_cust = db.av_cust.Find(id);
            if (av_cust == null)
            {
                return HttpNotFound();
            }
            return View(av_cust);
        }

        //
        // POST: /Av_Cust/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Av_Cust av_cust)
        {
            if (ModelState.IsValid)
            {
                db.Entry(av_cust).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(av_cust);
        }

        //
        // GET: /Av_Cust/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Av_Cust av_cust = db.av_cust.Find(id);
            if (av_cust == null)
            {
                return HttpNotFound();
            }
            return View(av_cust);
        }

        //
        // POST: /Av_Cust/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Av_Cust av_cust = db.av_cust.Find(id);
            db.av_cust.Remove(av_cust);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /Customers/Waiting

        public ActionResult Wait()
        {
            
            return View();
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}