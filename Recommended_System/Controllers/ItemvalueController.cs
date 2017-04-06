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
    public class ItemvalueController : Controller
    {

        private ItemvalueDBContext db = new ItemvalueDBContext();

        //
        // GET: /Itemvalue/

        public ActionResult Index()
        {
            return View(db.item_val.ToList());
        }

        //
        // GET: /Itemvalue/Details/5

        public ActionResult Details(int id = 0)
        {
            Item_value item_value = db.item_val.Find(id);
            if (item_value == null)
            {
                return HttpNotFound();
            }
            return View(item_value);
        }

        //
        // GET: /Itemvalue/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Itemvalue/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item_value item_value)
        {
            if (ModelState.IsValid)
            {
                db.item_val.Add(item_value);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(item_value);
        }

        //
        // GET: /Itemvalue/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Item_value item_value = db.item_val.Find(id);
            if (item_value == null)
            {
                return HttpNotFound();
            }
            return View(item_value);
        }

        //
        // POST: /Itemvalue/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item_value item_value)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item_value).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item_value);
        }

        //
        // GET: /Itemvalue/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Item_value item_value = db.item_val.Find(id);
            if (item_value == null)
            {
                return HttpNotFound();
            }
            return View(item_value);
        }

        //
        // POST: /Itemvalue/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item_value item_value = db.item_val.Find(id);
            db.item_val.Remove(item_value);
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