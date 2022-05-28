using ShopWeb.BLL;
using ShopWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using ShopWeb.Utils;

namespace ShopWeb.Admin.Controllers
{
    public class GoodsController : Controller
    {
        // GET: Goods
        public ActionResult Index()
        {
            int managerId;
            if ((managerId = UsualUtils.IsIdSession(Session["manager"])) <= 0)
            {
                return RedirectToAction("Login","Home");
            }
            List<Goods> goods = GoodsManager.GetAllGoods();
            List<GoodsShowModel> goodsShow = new List<GoodsShowModel>();
            for(int i = 0; i < goods.Count; i++)
            {
                goodsShow.Add(new GoodsShowModel(goods[i]));
            }
            return View(goodsShow);
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(string search)
        {
            int managerId;
            if ((managerId = UsualUtils.IsIdSession(Session["manager"])) <= 0)
            {
                return RedirectToAction("Login", "Home");
            }
            List<Goods> goods = GoodsManager.SearchGoods(search);
            List<GoodsShowModel> goodsShow = new List<GoodsShowModel>();
            for (int i = 0; i < goods.Count; i++)
            {
                goodsShow.Add(new GoodsShowModel(goods[i]));
            }
            return View(goodsShow);
        }

        /// <summary>
        /// 详情页
        /// </summary>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        public ActionResult Detail(int goods_id)
        {
            int managerId;
            if ((managerId = UsualUtils.IsIdSession(Session["manager"])) <= 0)
            {
                return RedirectToAction("Login", "Home");
            }

            Goods goods = GoodsManager.GetGoodsById(goods_id);
            GoodsShowModel showModel = new GoodsShowModel(goods);
            return View(showModel);
        }

        /// <summary>
        /// 编辑页提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GoodsEditModel model)
        {
            int managerId;
            if ((managerId = UsualUtils.IsIdSession(Session["manager"])) <= 0)
            {
                return RedirectToAction("Login", "Home");
            }
            if (model.Id > 0)
            {
                byte[] image = Convert.FromBase64String(model.ImageUrl);
                GoodsManager.UpdateGoods(new Goods()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Price = model.Price,
                    Inventory = model.Inventory,
                    Image = image
                });
                return RedirectToAction("Detail", "Goods", new { goods_id = model.Id });
            }
            else
            {
                byte[] image = Convert.FromBase64String(model.ImageUrl);
                int id = GoodsManager.AddNewGoods(model.Name, model.Price, model.Inventory, image);
                return RedirectToAction("Detail", "Goods", new { goods_id = id });
            }
        }

        /// <summary>
        /// 编辑页显示
        /// </summary>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int goods_id)
        {
            int managerId;
            if ((managerId = UsualUtils.IsIdSession(Session["manager"])) <= 0)
            {
                return RedirectToAction("Login", "Home");
            }

            // 新增逻辑
            if (goods_id == 0)
            {
                return View();
            }
            // 编辑旧有的
            Goods goods = GoodsManager.GetGoodsById(goods_id);
            if (goods.IsDeleted)
            {
                TempData["error"] = "该商品已被删除，不可编辑";
                return View();
            }
            else
            {
                return View(new GoodsEditModel(goods));
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        public ActionResult Delete(int goods_id)
        {
            int managerId;
            if ((managerId = UsualUtils.IsIdSession(Session["manager"])) <= 0)
            {
                return RedirectToAction("Login", "Home");
            }
            return View(GoodsManager.DeleteGoods(goods_id));
        }

    }
}