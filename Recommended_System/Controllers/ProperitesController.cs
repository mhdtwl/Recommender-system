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
    public class ProperitesController : Controller
    {
        private ProperitesDBContext db = new ProperitesDBContext();
        // GET: /Properites/
        public ActionResult Index()
        {
            return View(db.Properites.ToList());
        }

        //
        // GET: /Properites/Details/5

        public ActionResult Details(int id = 0)
        {
            Properites properites = db.Properites.Find(id);
            if (properites == null)
            {
                return HttpNotFound();
            }
            return View(properites);
        }

        //
        // GET: /Properites/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Properites/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Properites properites)
        {
            if (ModelState.IsValid)
            {
                db.Properites.Add(properites);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(properites);
        }

        //
        // GET: /Properites/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Properites properites = db.Properites.Find(id);
            if (properites == null)
            {
                return HttpNotFound();
            }
            return View(properites);
        }

        //
        // POST: /Properites/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Properites properites)
        {
            if (ModelState.IsValid)
            {
                db.Entry(properites).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(properites);
        }

        //
        // GET: /Properites/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Properites properites = db.Properites.Find(id);
            if (properites == null)
            {
                return HttpNotFound();
            }
            return View(properites);
        }

        //
        // POST: /Properites/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Properites properites = db.Properites.Find(id);
            db.Properites.Remove(properites);
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