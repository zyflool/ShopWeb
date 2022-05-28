using ShopWeb.BLL;
using ShopWeb.Models;
using ShopWeb.Utils;
using System;
using System.Web.Mvc;

namespace ShopWeb.Admin.Controllers
{
    public class HomeController : Controller
    { 

        /// <summary>
        /// 管理员首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            int managerId;
            if ((managerId = UsualUtils.IsIdSession(Session["manager"])) <= 0)
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("Index","Goods");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "请重新输入");
                return View();
            }

            if (ManagerManager.CheckIdAndPassword(model.UserName, model.Password))
            {
                Session["manager"] = ManagerManager.GetManagerByName(model.UserName).Id;
                return RedirectToAction("Index", "Goods");
            }
            else
            {
                ModelState.AddModelError("", "账号或密码有误，请重新输入");
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session["manager"] = null;
            return RedirectToAction("Login");
        }
        
        public ActionResult Edit()
        {
            int managerId;
            if ((managerId = UsualUtils.IsIdSession(Session["manager"])) <= 0)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ChangePasswordModel model)
        {
            int managerId;
            if ((managerId = UsualUtils.IsIdSession(Session["manager"])) <= 0)
            {
                return RedirectToAction("Login");
            }

            if (!ManagerManager.CheckIdAndPassword(managerId, model.OldPassword))
            {
                ModelState.AddModelError("", "旧密码输入错误，请重新输入");
                return View();
            }
            if (model.NewPassword.Equals(model.OldPassword))
            {
                ModelState.AddModelError("", "两次密码不能相同，请重新输入");
                return View();
            }
            else
            {
                ManagerManager.ChangePassword(managerId, model.NewPassword);
                TempData.Add("result", true);
                return View();
            }
        }

    }
}