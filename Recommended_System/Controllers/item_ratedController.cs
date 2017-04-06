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
    public class item_ratedController : Controller
    {
        private RRDBContext db = new RRDBContext();

        //
        // GET: /item_rated/

        public ActionResult Index()
        {
            return View(db.itrat.ToList());
        }

        //
        // GET: /item_rated/Details/5

        public ActionResult Details(int id = 0)
        {
            Item_rate item_rate = db.itrat.Find(id);
            if (item_rate == null)
            {
                return HttpNotFound();
            }
            return View(item_rate);
        }

        //
        // GET: /item_rated/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /item_rated/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item_rate item_rate)
        {
            if (ModelState.IsValid)
            {
                db.itrat.Add(item_rate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(item_rate);
        }

        //
        // GET: /item_rated/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Item_rate item_rate = db.itrat.Find(id);
            if (item_rate == null)
            {
                return HttpNotFound();
            }
            return View(item_rate);
        }

        //
        // POST: /item_rated/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item_rate item_rate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item_rate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item_rate);
        }

        //
        // GET: /item_rated/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Item_rate item_rate = db.itrat.Find(id);
            if (item_rate == null)
            {
                return HttpNotFound();
            }
            return View(item_rate);
        }

        //
        // POST: /item_rated/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item_rate item_rate = db.itrat.Find(id);
            db.itrat.Remove(item_rate);
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