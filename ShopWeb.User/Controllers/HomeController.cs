using ShopWeb.BLL;
using ShopWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Goods> goodsList = GoodsManager.GetOrderedGoods();

            List<GoodsShowModel> goodsShowModels = new List<GoodsShowModel>();
            goodsList.ForEach(goods => goodsShowModels.Add(new GoodsShowModel(goods)));

            return View(goodsShowModels);
        }

    }
}