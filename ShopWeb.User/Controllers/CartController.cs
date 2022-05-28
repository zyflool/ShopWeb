using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopWeb.BLL;
using ShopWeb.Models;
using ShopWeb.Utils;

namespace ShopWeb.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            int userId;
            if ((userId = UsualUtils.IsIdSession(Session["user"])) <= 0)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                List<GoodsItem> goodsItemList = CartManager.GetCartGoodsItem(Convert.ToInt32(Session["user"]));
                List<CartGoodsShowItemModel> goodsShowItemList = new List<CartGoodsShowItemModel>();
                for (int i = 0; i < goodsItemList.Count; i++)
                {
                    goodsShowItemList.Add(new CartGoodsShowItemModel()
                    {
                        GoodsShowModel = new GoodsShowModel(goodsItemList[i].Goods),
                        Count = goodsItemList[i].Count
                    });
                }
                return View(goodsShowItemList);
            }
        }

        public ActionResult Add(int goods_id, int goods_count)
        {
            int userId;
            if ((userId = UsualUtils.IsIdSession(Session["user"])) <= 0)
            {
                return RedirectToAction("Login", "User");
            }
            if (goods_id > 0 && goods_count > 0)
            {
                CartManager.AddCartGoods(userId, goods_id, goods_count);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int goods_id)
        {
            int userId;
            if ((userId = UsualUtils.IsIdSession(Session["user"])) <= 0)
            {
                return RedirectToAction("Login", "User");
            }
            CartManager.DeletCartGoods(userId, goods_id);

            return RedirectToAction("Index");
        }


    }
}