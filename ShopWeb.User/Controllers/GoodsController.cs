using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopWeb.Models;
using ShopWeb.BLL;
using ShopWeb.Utils;

namespace ShopWeb.Controllers
{
    public class GoodsController : Controller
    {
        public ActionResult Index()
        {
            List<Goods> goodsList = new List<Goods>(0);
            goodsList.AddRange(GoodsManager.GetAllGoods());
            List<GoodsShowModel> goodsShowModels = new List<GoodsShowModel>();
            for (int i = 0; i < goodsList.Count; i++)
            {
                goodsShowModels.Add(new GoodsShowModel(goodsList[i]));
            }
            return View(goodsShowModels);
        }

        [HttpGet]
        public ActionResult Index(string search)
        {
            List<Goods> goodsList = new List<Goods>(0);
            goodsList.AddRange(GoodsManager.SearchGoods(search));
            List<GoodsShowModel> goodsShowModels = new List<GoodsShowModel>();
            for (int i = 0; i < goodsList.Count; i++)
            {
                goodsShowModels.Add(new GoodsShowModel(goodsList[i]));
            }
            TempData["search"] = search;
            return View(goodsShowModels);
        }


        public ActionResult Search(string search)
        {
            List<Goods> goodsList = new List<Goods>(0);
            goodsList.AddRange(GoodsManager.SearchGoods(search));
            return View(goodsList);
        }

        public ActionResult Detail(int goods_id)
        {
            Goods goods = GoodsManager.GetGoodsById(goods_id);
            return View(new GoodsShowModel(goods));
        }

        [HttpGet]
        public ActionResult Wish()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Wish(WishGoodsModel wishGoodsModel)
        {
            int userId;
            if ((userId = UsualUtils.IsIdSession(Session["user"])) <= 0)
            {
                return RedirectToAction("Login", "User");
            }

            if (ModelState.IsValid)
            {
                byte[] image = Convert.FromBase64String(wishGoodsModel.ImageUrl);
                int wishGoodsId = WishGoodsManager.GenerateWishGoods(userId,wishGoodsModel.Name, image);
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}