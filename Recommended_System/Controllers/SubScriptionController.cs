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
    public class SubScriptionController : Controller
    {
        private SubScription_KindtDBContext db = new SubScription_KindtDBContext();

        //
        // GET: /SubScription/

        public ActionResult Index()
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "no")
                return RedirectToAction("Index", "Customers");
            return View(db.subscription_kind.ToList());
        }

        //
        // GET: /SubScription/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "no")
                return RedirectToAction("Index", "Customers");

            SubScription_Kind2 subscription_kind2 = db.subscription_kind.Find(id);
            if (subscription_kind2 == null)
            {
                return HttpNotFound();
            }
            return View(subscription_kind2);
        }

        //
        // GET: /SubScription/Create

        public ActionResult Create()
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "no")
                return RedirectToAction("Index", "Customers");
            return View();
        }

        //
        // POST: /SubScription/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubScription_Kind2 subscription_kind2)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "no")
                return RedirectToAction("Index", "Customers");
            if (ModelState.IsValid)
            {
                db.subscription_kind.Add(subscription_kind2);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subscription_kind2);
        }

        //
        // GET: /SubScription/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "no")
                return RedirectToAction("Index", "Customers");
            SubScription_Kind2 subscription_kind2 = db.subscription_kind.Find(id);
            if (subscription_kind2 == null)
            {
                return HttpNotFound();
            }
            return View(subscription_kind2);
        }

        //
        // POST: /SubScription/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SubScription_Kind2 subscription_kind2)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "no")
                return RedirectToAction("Index", "Customers");
            if (ModelState.IsValid)
            {
                db.Entry(subscription_kind2).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subscription_kind2);
        }

        //
        // GET: /SubScription/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "no")
                return RedirectToAction("Index", "Customers");
            SubScription_Kind2 subscription_kind2 = db.subscription_kind.Find(id);
            if (subscription_kind2 == null)
            {
                return HttpNotFound();
            }
            return View(subscription_kind2);
        }

        //
        // POST: /SubScription/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "no")
                return RedirectToAction("Index", "Customers");
            SubScription_Kind2 subscription_kind2 = db.subscription_kind.Find(id);
            db.subscription_kind.Remove(subscription_kind2);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}