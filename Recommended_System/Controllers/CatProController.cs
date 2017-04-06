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
    public class CatProController : Controller
    {

        private catproDBContext db = new catproDBContext();

        //
        // GET: /catpro/

        public ActionResult Index()
        {
            return View(db.catpro.ToList());
        }

        //
        // GET: /catpro/Details/5

        public ActionResult Details(int id = 0)
        {
            Cat_pro cat_pro = db.catpro.Find(id);
            if (cat_pro == null)
            {
                return HttpNotFound();
            }
            return View(cat_pro);
        }

        //
        // GET: /catpro/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /catpro/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cat_pro cat_pro)
        {
            if (ModelState.IsValid)
            {
                db.catpro.Add(cat_pro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cat_pro);

        }

        //
        // GET: /catpro/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Cat_pro cat_pro = db.catpro.Find(id);
            if (cat_pro == null)
            {
                return HttpNotFound();
            }
            return View(cat_pro);
        }

        //
        // POST: /catpro/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cat_pro cat_pro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cat_pro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cat_pro);
        }

        //
        // GET: /catpro/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Cat_pro cat_pro = db.catpro.Find(id);
            if (cat_pro == null)
            {
                return HttpNotFound();
            }
            return View(cat_pro);
        }

        //
        // POST: /catpro/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cat_pro cat_pro = db.catpro.Find(id);
            db.catpro.Remove(cat_pro);
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