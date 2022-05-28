using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopWeb.Models;

namespace ShopWeb.DAL
{
    public class CartService
    {

        #region 购物车信息
        public static bool AddNewCart(int userId)
        {
            string sql = "insert into Cart (user_id) values";
            sql += string.Format("({0})", userId);
            try
            {
                int result = SQLHelper.Update(sql);
                return result > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static List<int> GetCartId(int userId)
        {
            string sql = string.Format(
                "select cart_id from [User] where user_id = {0}", 
                userId);
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<int> list = new List<int>();
            while (objReader.Read())
            {
                list.Add(Convert.ToInt32(objReader["cart_id"]));
            }
            return list;
        }

        public static int GetCartIdByUid(int userId)
        {
            string sql = string.Format("select cart_id from Cart where user_id = {0}", userId);
            List<int> list = new List<int>();
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            while (objReader.Read())
            {
                list.Add(Convert.ToInt32(objReader["cart_id"]));
            }
            if (list == null || list.Count == 0)
            {
                return 0;
            }
            else
            {
                return list[0];
            }
        }
        #endregion


        #region 购物车商品处理

        public static List<GoodsItem> GetCartAllGoodsItem(int cartId)
        {
            string sql = string.Format(
                "select * from Cart_Goods where cart_id = {0}",
                cartId);
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<GoodsItem> list = new List<GoodsItem>();
            while (objReader.Read())
            {
                list.Add(new GoodsItem()
                {
                    Goods = new Goods()
                    {
                        Id = Convert.ToInt32(objReader["goods_id"])
                    },
                    Count = Convert.ToInt32(objReader["cart_goods_count"])
                });
            }
            return list;
        }
        public static bool AddGoodsItem(int cartId, int goodsId, int goodsCount)
        {
            string sql = "insert into Cart_Goods (cart_id, goods_id, cart_goods_count) values";
            sql += string.Format("({0}, {1}, {2})", cartId, goodsId, goodsCount);
            try
            {
                int result = SQLHelper.Update(sql);
                return result > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool DeleteGoodsItem(int cartId, int goodsId)
        {
            string sql = string.Format("delete from Cart_Goods where cart_id = {0} and goods_id = {1}", cartId, goodsId);
            try
            {
                int result = SQLHelper.Update(sql);
                return result > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static bool UpdateGoodsCount(int cartId, int goodsId, int goodsCount)
        {
            string sql = string.Format("update Cart_Goods set cart_goods_count = {0} " +
                "where cart_id = {1} and goods_id = {2}", goodsCount, cartId, goodsId);
            try
            {
                int result = SQLHelper.Update(sql);
                return result > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
