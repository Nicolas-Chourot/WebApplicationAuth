using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Controllers.AccessControl;

namespace WebApplication.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [UserAccess(Models.Access.View)]
        public ActionResult GetData(bool forceRefresh = false)
        {
            return PartialView();
        }


        [UserAccess(Models.Access.View)]
        public ActionResult ProtectedView()
        {
            return View();
        }
    }
}