using ShopWeb.BLL;
using ShopWeb.Models;
using System;
using System.Web.Mvc;
using ShopWeb.Utils;
using ShopWeb.Models;

namespace ShopWeb.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            int userId;
            if ((userId = UsualUtils.IsIdSession(Session["user"])) <= 0)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                return RedirectToAction("Mine");
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // 防伪标记验证
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "请重新输入");
                return View();
            }
            int userId = 0;
            if ((userId = UserManager.CheckNameAndPassword(model.UserName, model.Password)) != 0)
            {
                Session["user"] = userId;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "账号或密码有误，请重新输入");
                return View();
            }

        }

        /// <summary>
        /// 注册
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken] // 防伪标记验证
        public ActionResult Logup(LogupModel model)
        {
            int userId = UserManager.Register(model.UserName, model.Password, model.Phone);
            if (userId != 0)
            {
                Session["user"] = userId;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "注册失败，请重试");
                return View();
            }
        }

        [HttpGet]
        public ActionResult Logup()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Login");
        }

        /// <summary>
        /// 个人中心
        /// </summary>
        /// <returns></returns>
        public ActionResult Mine()
        {
            int userId;
            if ((userId = UsualUtils.IsIdSession(Session["user"])) <= 0)
            {
                return RedirectToAction("Login", "User");
            }

            Models.User user = BLL.UserManager.GetUserById(userId);
            ViewData["UserMineModel"] = new UserMineModel()
            {
                UserName = user.Name,
                Password = user.Password,
                Phone = user.Phone
            };
            return View();
        }

        /// <summary>
        /// 编辑密码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditPassWord()
        {
            int userId;
            if ((userId = UsualUtils.IsIdSession(Session["user"])) <= 0)
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }


        /// <summary>
        /// 编辑密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditPassWord(ChangePasswordModel model)
        {
            int userId;
            if ((userId = UsualUtils.IsIdSession(Session["user"])) <= 0)
            {
                return RedirectToAction("Login", "User");
            }

            if (UserManager.CheckIdAndPassword(userId, model.OldPassword) <= 0)
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
                if (UserManager.ChangePassword(userId, model.NewPassword))
                {
                    return RedirectToAction("Mine");
                } else
                {
                    ModelState.AddModelError("", "修改密码出现错误，请稍后重试");
                    return View();
                }
            }
        }


        /// <summary>
        /// 编辑电话
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditPhone()
        {
            int userId;
            if ((userId = UsualUtils.IsIdSession(Session["user"])) <= 0)
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }

        /// <summary>
        /// 编辑电话
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditPhone(ChangePhoneModel model)
        {
            int userId;
            if ((userId = UsualUtils.IsIdSession(Session["user"])) <= 0)
            {
                return RedirectToAction("Login", "User");
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("","输入有误，请重试");
                return View();
            }
            else
            {
                if (model.OldPhoneNumber.Equals(model.NewPhoneNumber))
                {
                    ModelState.AddModelError("", "新的联系电话不能与原联系电话相同，请重新输入");
                    return View();
                }

                Models.User user = UserManager.GetUserById(userId);
                user.Phone.Equals(model.OldPhoneNumber);

                if (!user.Phone.Equals(model.OldPhoneNumber))
                {
                    ModelState.AddModelError("", "原联系电话输入有误，请重新输入");
                    return View();
                }
                if (UserManager.UpdatePhoneNumber(userId, model.NewPhoneNumber))
                {
                    return RedirectToAction("Mine");
                } else
                {
                    ModelState.AddModelError("", "修改联系电话出现错误，请稍后重试");
                    return View();
                }
            }
        }

    }
}