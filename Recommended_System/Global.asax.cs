using Recommended_System.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;

namespace Recommended_System
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Session_Start(Object sender, EventArgs e)
        {
            Session["init"] = 0;
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            var display = DisplayModeProvider.Instance.Modes;
            Database.SetInitializer<ShopItemDBContext>(null);
            Database.SetInitializer<CatDBContext>(null);
            Database.SetInitializer<SubScription_KindtDBContext>(null);

            Database.SetInitializer<Cs_SuDBContext>(null);
            Database.SetInitializer<CustomersDBContext>(null);
            Database.SetInitializer<Av_Cust2DBContext>(null);
            Database.SetInitializer<Av_CustDBContext>(null);

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleMobileConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}