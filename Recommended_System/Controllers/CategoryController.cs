using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Recommended_System.Models;

namespace tester.Controllers
{
    public class CategoryController : Controller
    {

        private ItemDBContext itdb = new ItemDBContext();
        private ProperitesDBContext pdb = new ProperitesDBContext();
        private ItemvalueDBContext itdp = new ItemvalueDBContext();
        private CatDBContext db = new CatDBContext();
        private catproDBContext catpro = new catproDBContext();
        private ShopItemDBContext shopit = new ShopItemDBContext();

        //
        // GET: /Category/

        public ActionResult Index()
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"]=="no")
                    return RedirectToAction("Index", "Customers");
            return View(db.category.ToList());
        }
        public ActionResult Prop(Itemadder it)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "no")
                return RedirectToAction("Index", "Customers");

           
            if (it.ind != null)
            {
                it.ind.Add(it.ind.Count) ;
                it.pv.Add("Date");
                it.ind.Add(it.ind.Count);
                it.pv.Add("Model");
                it.ind.Add(it.ind.Count);
                it.pv.Add("Image");
              

                try
                {
                    var s2 = from a in db.category
                             orderby a.Cat_Id descending
                             select a;
                    Categories so = s2.First();
                    it.cat_id = so.Cat_Id;
                    int id = 0;
                    var q3 = from a in pdb.Properites
                             select a;
                    foreach (var a in q3)
                    {
                        id = a.P_Id;
                    }
                    bool exist = false;

                    for (int i = 0; i < it.pv.Count; i++)
                    {
                        exist = false;
                        string x = it.pv[i];
                        var q = from a in pdb.Properites
                                where a.P_Name.ToLower() == x.ToLower()
                                select a;

                        foreach (var b in q)
                        {
                            exist = true;
                            Cat_pro cap = new Cat_pro();
                            cap.Cat_Id = it.cat_id;
                            cap.P_Id = b.P_Id;
                            catpro.catpro.Add(cap);


                        }
                        if (exist == false)
                        {

                            Properites p = new Properites(it.pv[i]);
                            id++;
                            p.P_Id = id;
                            pdb.Properites.Add(p);
                            pdb.SaveChanges();
                            var eq = from d in pdb.Properites where d.P_Name == p.P_Name select d;

                            Properites r = eq.First();
                            Cat_pro cap = new Cat_pro();
                            cap.Cat_Id = it.cat_id;
                            cap.P_Id = r.P_Id;
                            catpro.catpro.Add(cap);
                            


                        }


                    }
                    pdb.SaveChanges();
                    catpro.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception r)
                {
                    ViewBag.message = "INVALID ENTER";
                    it.ind = null;
                    return View(it);
                }
            }
            else
            {

                it.p = new List<Properites>();
                it.pv = new List<string>();
                it.ind = new List<int>();
                for (int i = 0; i < it.counter; i++)
                {
                    it.pv.Add("");
                    it.ind.Add(0);
                }
                ViewBag.counter = it.counter;
                return View(it);
            }

        }

        

        //
        // GET: /Category/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "no")
                return RedirectToAction("Index", "Customers");

            Categories categories = db.category.Find(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        //
        // GET: /Category/Create

        public ActionResult Create()
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "no")
                return RedirectToAction("Index", "Customers");

            return View();
        }

        //
        // POST: /Category/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Itemadder categories)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "no")
                return RedirectToAction("Index", "Customers");

            ViewBag.message = "";

            if (ModelState.IsValid)
            {
                //db.category.Add(categories);
                //db.SaveChanges();
                try
                {
                    int id = 0;
                    var q3 = from a in db.category
                             select a;
                    foreach (var a in q3)
                    {
                        id = a.Cat_Id;
                    }
                    Categories ca = new Categories(categories.cat_name);
                    ca.Cat_Id = id + 1;
                    db.category.Add(ca);
                    categories.cat_id = ca.Cat_Id;
                    db.SaveChanges();

                    return RedirectToAction("Prop", categories);
                }
                catch (Exception e)
                {
                    ViewBag.message = "INVALID ENTER";
                    return View(categories);
                }
            }

            return View(categories);
        }

        //
        // GET: /Category/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "no")
                return RedirectToAction("Index", "Customers");

            Categories categories = db.category.Find(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            Itemadder it = new Itemadder();

           
            it.p = new List<Properites>();
            it.pv = new List<string>();
            it.ind = new List<int>();

            it.item_name = categories.Name;
            it.cat_id = categories.Cat_Id;
            return View(it);
        }

        //
        // POST: /Category/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Itemadder it)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "no")
                return RedirectToAction("Index", "Customers");

            ViewBag.message = "";
            if (it.pv != null)
            {
                if (it.pv.Contains("") || it.pv.Contains(null))
                {
                    return RedirectToAction("Index");
                }
                try
                {
                    List<int> ignore = new List<int>();
                    int id = 0;
                    var e = from a in pdb.Properites
                            select a;
                    foreach (var a in e)
                    {
                        id = a.P_Id;
                    }
                    Categories cat = new Categories(it.cat_name);
                    cat.Cat_Id = it.cat_id;
                    db.Entry(cat).State = EntityState.Modified;
                    db.SaveChanges();

                    bool exist = false;
                    for (int i = 0; i < it.pv.Count; i++)
                    {
                        string x = it.pv[i];
                        var q1 = from a in pdb.Properites
                                 where a.P_Name.ToLower() == x.ToLower()
                                 select a;
                        foreach (var b in q1)
                        {
                            exist = true;
                            //Cat_pro cap = new Cat_pro();
                            //cap.Cat_Id = it.cat_id;
                            //cap.P_Id = b.P_Id;
                            //catpro.catpro.Add(cap);
                            ignore.Add(b.P_Id);


                        }
                        if (exist == false)
                        {

                            Properites p = new Properites(it.pv[i]);
                            id++;
                            p.P_Id = id;
                            ignore.Add(p.P_Id);
                            Cat_pro cap = new Cat_pro();
                            cap.Cat_Id = it.cat_id;
                            cap.P_Id = p.P_Id;
                            catpro.catpro.Add(cap);
                            pdb.Properites.Add(p);


                        }


                    }


                    var ee = from a in catpro.catpro
                             where a.Cat_Id == it.cat_id
                             select a;

                    foreach (var t in ee)
                    {
                        if (ignore.Contains(t.P_Id))
                        {

                        }

                        else
                        {
                            Properites d2 = (from a in pdb.Properites where a.P_Name.ToLower() == "model" select a).First();
                            Properites d3 = (from a in pdb.Properites where a.P_Name.ToLower() == "date" select a).First();
                            Properites d4 = (from a in pdb.Properites where a.P_Name.ToLower() == "image" select a).First();
                            if(t.P_Id!=d2.P_Id&&t.P_Id!=d3.P_Id&&t.P_Id!=d4.P_Id)
                            catpro.catpro.Remove(t);
                        }
                    }


                    
                 

                   
                             /* Categories cat = new Categories(it.cat_name);
                    cat.Cat_Id = it.cat_id;
                    db.Entry(cat).State = EntityState.Modified;
                    db.SaveChanges();*/



                    pdb.SaveChanges();
                    catpro.SaveChanges();
                    db.SaveChanges();

                    return RedirectToAction("Index");

                }
                catch { ViewBag.message = "INVALID ENTER"; return View(it); }
            }
            else
            {

                it.p = new List<Properites>();
                it.pv = new List<string>();
                it.ind = new List<int>();
                int counter = 0;

                it.item_name = it.cat_name;

                var q6 = from s in catpro.catpro
                         where s.Cat_Id == it.cat_id

                         select s;


                foreach (var d in q6)
                {
                    Properites ty = (from a in pdb.Properites where a.P_Id == d.P_Id select a).First();
                    if (ty.P_Name.ToLower() != "date" && ty.P_Name.ToLower() != "model" && ty.P_Name.ToLower() != "image")
                    {
                        counter++;
                        it.ind.Add(d.P_Id);
                    }


                }
                int y = 0;
                var q5 = from s in pdb.Properites

                         select s;

                foreach (var a in q5)
                {
                    if (it.ind.Contains(a.P_Id))
                    {
                        it.p.Add(a);
                        it.pv.Add(a.P_Name);
                    }


                }


                ViewBag.number = counter;
                return View(it);

            }
        }

        //
        // GET: /Category/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "no")
                return RedirectToAction("Index", "Customers");

            Categories categories = db.category.Find(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        //
        // POST: /Category/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            if (Session["swi"] == null)
                return RedirectToAction("Index", "Home");
            if (Session["swi"] != null && Session["isadmin"] == "no")
                return RedirectToAction("Index", "Customers");

            Categories categories = db.category.Find(id);
            db.category.Remove(categories);
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