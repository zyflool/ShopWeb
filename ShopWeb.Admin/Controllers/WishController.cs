using ShopWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopWeb.Admin.Controllers
{
    public class WishController : Controller
    {
        // GET: Wish
        public ActionResult Index()
        {
            int managerId;
            if ((managerId = UsualUtils.IsIdSession(Session["manager"])) <= 0)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
    }
}