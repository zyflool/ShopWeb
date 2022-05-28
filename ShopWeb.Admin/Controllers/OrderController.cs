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
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            int managerId;
            if ((managerId = UsualUtils.IsIdSession(Session["manager"])) <= 0)
            {
                return RedirectToAction("Login","Home");
            }
            
            return View(OrderManager.GetOrderThroughManager());
        }

        [HttpGet]
        public ActionResult Index(string order_id)
        {
            int managerId;
            if ((managerId = UsualUtils.IsIdSession(Session["manager"])) <= 0)
            {
                return RedirectToAction("Login", "Home");
            }
            List<Order> orderList = new List<Order>();

            if (order_id == null || order_id.Length == 0)
            {
                orderList = OrderManager.GetOrderThroughManager();
            }
            else
            {
                Order order = OrderManager.GetOrderById(order_id);
                if (order != null)
                    orderList.Add(order);
            }
            return View(orderList);
        }


        public PartialViewResult GoodsList(List<CartGoodsShowItemModel> models)
        {
            return PartialView(models);
        }


        public ActionResult Detail(string order_id)
        {
            int managerId;
            if ((managerId = UsualUtils.IsIdSession(Session["manager"])) <= 0)
            {
                return RedirectToAction("Login", "Home");
            }
            Order order = OrderManager.GetOrderById(order_id);
            return View(new OrderShowModel(order));
        }

        public ActionResult Finish(string order_id)
        {
            int managerId;
            if ((managerId = UsualUtils.IsIdSession(Session["manager"])) <= 0)
            {
                return RedirectToAction("Login", "Home");
            }
            Order order = OrderManager.GetOrderById(order_id);
            string result = OrderManager.ManagerFinishOrder(managerId, order);
            if (result.Length <= 0 )
            {
                return RedirectToAction("Detail", "Order", new { order_id = order_id });
            } else
            {
                return Content(result);
            } 
        }
    }
}