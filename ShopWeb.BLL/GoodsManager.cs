using System;
using System.Collections.Generic;
using ShopWeb.DAL;
using ShopWeb.Models;

namespace ShopWeb.BLL
{
    public class GoodsManager
    {
        public static List<Goods> GetAllGoods()
        {
            List<Goods> goods = GoodsService.GetAllGoods();
            return goods;
        }

        public static List<Goods> SearchGoods(string search)
        {
            if (search == null || search == "")
            {
                return GetAllGoods();
            }
            List<Goods> goodsList = GoodsService.GetGoodsListByName(search);
            return goodsList;
        }

        public static Goods GetGoodsById(int id)
        {
            List<Goods> goods = GoodsService.GetGoodsById(id);
            if (goods == null || goods.Count == 0)
            {
                return null;
            }
            else
            {
                return goods[0];
            }
        }

        public static Goods GetGoodsByName(string name)
        {
            List<Goods> goods = GoodsService.GetGoodsByName(name);
            if (goods == null || goods.Count == 0)
            {
                return null;
            }
            else
            {
                return goods[0];
            }
        }

        /// <summary>
        /// 按销量排序的商品列表
        /// </summary>
        /// <returns></returns>
        public static List<Goods> GetOrderedGoods()
        {
            return GoodsService.GetOrderedGoodsListBySql();
        }

        public static int AddNewGoods(string name, double price,int inventory, byte[] image)
        {
            return GoodsService.InsertGoodsByProcedure(new Goods()
            {
                Name = name,
                Price = price,
                Inventory = inventory,
                Image = image
            });
        }

        public static int AddNewGoods(Goods goods)
        {
            return GoodsService.InsertGoodsByProcedure(goods);
        }

        public static bool DeleteGoods(int id)
        {
            return GoodsService.UpdateGoodsByParameters(new List<Parameter>()
            {
                new Parameter("goods_is_deleted", 1),
                new Parameter("goods_id", id)
            });
        }

        public static bool updateGoodsInventory(int goodsId,int inventory)
        {

            return GoodsService.UpdateGoodsByParameters(new List<Parameter>()
            {
                new Parameter("goods_inventory", inventory),
                new Parameter("goods_id", goodsId)
            });
        }

        public static bool UpdateGoods(Goods goods)
        {
            return GoodsService.UpdateGoodsByProcedure(goods);
        }
    }
}
