using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Recommended_System.Models;

namespace Recommended_System.Controllers
{
    public class SupportController : Controller
    {
        //
        // GET: /Support/

        public ActionResult Index()
        {
            Itemadder t = new Itemadder();
            CustomersDBContext c = new CustomersDBContext();
            t.pv = new List<string>();
            t.ind=new List<int>();
            t.name_admin = new List<string>();
            var tt = from a in c.customer where a.Admin == 1 select a;
            foreach (var item in tt)
            {
                t.pv.Add(item.E_mail);
                t.name_admin.Add(item.Cust_Nme);
                t.ind.Add(item.Phone);

            }
            


            return View(t);
        }

    }
}
