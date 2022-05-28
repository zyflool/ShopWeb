using ShopWeb.BLL;
using ShopWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopWeb.Utils;

namespace ShopWeb.Controllers
{
    public class OrderController : Controller
    {

        private static List<OrderGoodsShowItemModel> OrderGoodsItemList = new List<OrderGoodsShowItemModel>();
        
        // GET: Order
        public ActionResult Index()
        {
            int userId;
            if ((userId = UsualUtils.IsIdSession(Session["user"])) <= 0)
            {
                return RedirectToAction("Login","User");
            }
            
            List<Order> orders = OrderManager.GetOrdersByUserId(Convert.ToInt32(Session["user"]), 4);

            return View(orders);
            
        }

        [HttpGet]
        public ActionResult Index(Int32? filter)
        {
            int userId;
            if ((userId = UsualUtils.IsIdSession(Session["user"])) <= 0)
            {
                return RedirectToAction("Login", "User");
            }
            if (filter == null)
            {
                filter = 4;
            }

            List<Order> orders = OrderManager.GetOrdersByUserId(Convert.ToInt32(Session["user"]), filter.Value);

            ViewBag.Filter = filter;

            return View(orders);

        }

        public ActionResult Detail(string order_id)
        {
            int userId;
            if ((userId = UsualUtils.IsIdSession(Session["user"])) <= 0)
            {
                return RedirectToAction("Login", "User");
            }

            Order order = OrderManager.GetOrderById(order_id);
            return View(new OrderShowModel(order));
        }

        [HttpPost]
        public ActionResult PreNew(List<CartGoodsShowItemModel> models)
        {
            int userId;
            if ((userId = UsualUtils.IsIdSession(Session["user"])) <= 0)
            {
                return RedirectToAction("Login", "User");
            }

            List<OrderGoodsShowItemModel> orderGoodsItems = new List<OrderGoodsShowItemModel>();
            foreach (var model in models)
            {
                if (model.IsSelected)
                {
                    OrderGoodsShowItemModel orderGoodsItemModel = new OrderGoodsShowItemModel()
                    {
                        GoodsShowModel = model.GoodsShowModel,
                        Count = model.Count
                    };
                    orderGoodsItems.Add(orderGoodsItemModel);
                }
            }

            OrderGoodsItemList = orderGoodsItems;
            return RedirectToAction("New");
        }

        [HttpGet]
        public ActionResult New()
        {
            int userId;
            if ((userId = UsualUtils.IsIdSession(Session["user"])) <= 0)
            {
                return RedirectToAction("Login", "User");
            }

            return View(new NewOrderInfoModel()
            {
                ScheduledTime = DateTime.Now.AddMinutes(50),
                goodsItemList = OrderGoodsItemList
            }) ;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(NewOrderInfoModel models)
        {
            int userId;
            if ((userId = UsualUtils.IsIdSession(Session["user"])) <= 0)
            {
                return RedirectToAction("Login", "User");
            }

            if (ModelState.IsValid)
            {
                string orderId = OrderManager.GenerateOrder(Convert.ToInt32(Session["user"]), models);
                return RedirectToAction("NewOrder", "Order", new { order_id = orderId});
            }

            return View(new NewOrderInfoModel()
            {
                ScheduledTime = DateTime.Now.AddMinutes(50),
                goodsItemList = OrderGoodsItemList
            });
        }

        public PartialViewResult GoodsList(List<CartGoodsShowItemModel> models)
        {
            return PartialView(models);
        }

        /// <summary>
        /// 新订单创建成功页面
        /// </summary>
        /// <returns></returns>
        public ActionResult NewOrder(string order_id)
        {
            int userId;
            if ((userId = UsualUtils.IsIdSession(Session["user"])) <= 0)
            {
                return RedirectToAction("Login", "User");
            }
            ViewData["order"] = order_id;
            return View();
        }

        /// <summary>
        /// 付款
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Pay(string order_id)
        {
            int userId;
            if ((userId = UsualUtils.IsIdSession(Session["user"])) <= 0)
            {
                return RedirectToAction("Login", "User");
            }

            Order order = OrderManager.GetOrderById(order_id);

            return View(order);
        }


        [HttpPost]
        public ActionResult Pay(Order order)
        {
            int userId;
            if ((userId = UsualUtils.IsIdSession(Session["user"])) <= 0)
            {
                return RedirectToAction("Login", "User");
            }
            string orderId = order.Id;
            Order realOrder = OrderManager.GetOrderById(orderId);
            string updateResult = OrderManager.UpdateOrderState(order.Id, realOrder.State, Order.STATE_PAYED, true);
            if (updateResult.Length <= 0) 
            {
                return RedirectToAction("Detail", "Order", new { order_id = order.Id });
            } 
            else
            {
                return View(order);
            }
        }

        public ActionResult Cancel(string order_id)
        {
            Order order = OrderManager.GetOrderById(order_id);
            string updateResult = OrderManager.UpdateOrderState(order.Id, order.State, Order.STATE_CANCELED, true);
            if (updateResult.Length <= 0)
            {
                return RedirectToAction("Detail", "Order", new { order_id = order.Id });
            }
            else
            {
                ViewBag["ErrorMessage"] = updateResult;
                return View();
            }
        }
    }
}