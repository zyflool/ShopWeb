using ShopWeb.BLL;
using ShopWeb.Models;
using ShopWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopWeb.Admin.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            int managerId;
            if ((managerId = UsualUtils.IsIdSession(Session["manager"])) <= 0)
            {
                return RedirectToAction("Login", "Home");
            }

            List<User> users = UserManager.GetUsers();
            return View(users);
        }

        [HttpGet]
        public ActionResult Index(string content, string search)
        {
            int managerId;
            if ((managerId = UsualUtils.IsIdSession(Session["manager"])) <= 0)
            {
                return RedirectToAction("Login", "Home");
            }

            List<User> users = new List<User>();
            
            if ((content == null && search == null) || (content.Length == 0 && search.Length == 0))
            {
                users = UserManager.GetUsers();
            } else
            {
                users = UserManager.SearchUsers(content, search);
            }

            return View(users);
        }
    }
}
