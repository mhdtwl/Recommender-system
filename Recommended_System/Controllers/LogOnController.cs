using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Recommended_System.Models;
using System.Web.Script.Serialization;

namespace Recommended_System.Controllers
{
    public class LogOnController : Controller
    {
        private LogOnDBContext db = new LogOnDBContext();
        private CustomersDBContext av = new CustomersDBContext();
        public ActionResult Index()
        {
            if (Session["swi"] != null)
                return RedirectToAction("Index", "Customers");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LogOn logon)
        {



            if (ModelState.IsValid)
            {
                Customers avv = new Customers();

                var result = from b in av.customer where logon.E_Mail == b.E_mail select b;
                foreach (var t in result)
                {
                    avv = t;
                    if (avv.Password == logon.Password)
                    {
                        Cs_SuDBContext csdb = new Cs_SuDBContext();
                        var q = from k in csdb.cs_su where k.Customer == avv.Cust_Id select k.Finish;
                        DateTime dt = DateTime.Now;
                        foreach (var item in q)
                        {
                            dt = item;

                        }
                        TimeSpan ts = dt.Subtract(DateTime.Now);
                        if (ts.Days < 0)
                        {
                            CustomersController cs = new CustomersController();
                            cs.rm(avv.Cust_Id);

                            return RedirectToAction("Index", "Home");
                        }
                        if (ts.Days < 4)
                            Session["renew"] = "Your profile just have " + ts.Days + " Days to expired !!";

                        Session["swi"] = avv.Cust_Id;
                        if (avv.Admin == 0)
                            Session["isadmin"] = "no";
                        else
                            Session["isadmin"] = "yes";

                        return RedirectToAction("Index", "Customers");
                    }
                }



                //TODO open session
                ViewBag.loginerror = true;
                return RedirectToAction("Index", "Home");
            }

            return View(logon);
        }
        private LogOn_Status Logic_LogOn(LogOn logon)
        {
            if (ModelState.IsValid)
            {
                Customers avv = new Customers();
                // var result = (from a in new Av_CustDBContext().av_cust
                //             where (a.E_mail == logon.E_Mail && a.Password == logon.Password)
                //           select a).ToList();
                var result = from b in av.customer where logon.E_Mail == b.E_mail select b;
                foreach (var t in result)
                {
                    avv = t;
                    if (avv.Password == logon.Password)
                    {
                        Session["swi"] = avv.Cust_Id;
                        if (avv.Admin == 0)
                            Session["isadmin"] = "no";
                        else
                            Session["isadmin"] = "yes";

                        return LogOn_Status.Custm;
                    }
                }
                //TODO open session
                ViewBag.loginerror = true;
                return LogOn_Status.Error; ///RedirectToAction("Index", "Home");
            }
            return LogOn_Status.Invld; //View(logon);
        }
        private string CutomerName_LogOn(LogOn logon)
        {
            var result = from b in av.customer where logon.E_Mail == b.E_mail select b.Cust_Nme;
            return result.First().ToString();
        }
        public LogOn LogOnMake(string username, string password)
        {
            LogOn lg = new LogOn();
            lg.E_Mail = username;
            lg.Password = password;
            return lg;
        }

        public ActionResult CheckLogin(string Login_Inputs)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            List<string> mystring = json.Deserialize<List<string>>(Login_Inputs);

            string user = mystring[0];
            string pass = mystring[1];

            ////////////////////////////////////////////////////////////////


            Customers avv = new Customers();

            var rqp = from b in av.customer where user == b.E_mail select b;
            foreach (var t in rqp)
            {
                avv = t;
                if (avv.Password == pass)
                {
                    Cs_SuDBContext csdb = new Cs_SuDBContext();
                    var q = from k in csdb.cs_su where k.Customer == avv.Cust_Id select k.Finish;
                    DateTime dt = DateTime.Now;
                    foreach (var item in q)
                    {
                        dt = item;

                    }
                    TimeSpan ts = dt.Subtract(DateTime.Now);
                    if (ts.Days < 0)
                    {
                        CustomersController cs = new CustomersController();
                        cs.rm(avv.Cust_Id);

                        return RedirectToAction("Index", "Home");
                    }
                    if (ts.Days < 4)
                        Session["renew"] = "Your profile just have " + ts.Days + " Days to expired !!";
                }
            }

            ////////////////////////////////////////////////////////////////
            LogOn_Status result = Logic_LogOn(LogOnMake(user, pass));
            ViewBag.CustmerLogName = null;
            if (result == LogOn_Status.Custm)
            {
                //ViewBag.CustmerLogName = nm;
                string nm = CutomerName_LogOn(LogOnMake(user, pass));
                //this.ViewBag.CustmerLogName = nm;
                Session["MyCust_N"] = nm;
                return Json((int)LogOn_Status.Custm, JsonRequestBehavior.AllowGet);
            }
            else if (result == LogOn_Status.Error)
                return Json((int)LogOn_Status.Error, JsonRequestBehavior.AllowGet);
            else if (result == LogOn_Status.Invld)
                return Json((int)LogOn_Status.Invld, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }
        private enum LogOn_Status { Custm, Error, Invld };
        //============================================================
        public ActionResult CheckLogout2()
        {
            Session.Remove("swi");
            Session.Remove("renew");
            return RedirectToAction("Index","Home");
        }
        public ActionResult CheckLogout(string Logout_Inputs)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            List<string> mystring = json.Deserialize<List<string>>(Logout_Inputs);
            Session.Remove("swi");
            Session.Remove("renew");
            return Json("Ok_Logout", JsonRequestBehavior.AllowGet);
        }

    }
}