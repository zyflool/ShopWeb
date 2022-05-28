using ShopWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.DAL
{
    public class WishGoodsService
    {
        public static List<WishGoods> GetAllWishGoods()
        {
            return GetWishGoodsBySql("");
        }

        public static List<WishGoods> GetWishGoodsByName(string name)
        {

            string whereSql = string.Format("where wish_goods_name like \'%{0}%\'", name);
            return GetWishGoodsBySql(whereSql);
        }

        public static WishGoods GetWishGoodsById(int id)
        {
            string whereSql = string.Format("where wish_goods_id = {0}", id);
            List<WishGoods> list = GetWishGoodsBySql(whereSql);
            if (list == null || list.Count <= 0)
            {
                return null;
            } 
            else
            {
                return list[0];
            }
        }

        public static List<WishGoods> GetWishGoodsBySql(string whereSql)
        {
            string sql = "select * from [WishGoods] ";
            sql += whereSql;

            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<WishGoods> list = new List<WishGoods>();
            while (objReader.Read())
            {
                list.Add(new WishGoods()
                {
                    Id = Convert.ToInt32(objReader["wish_goods_id"]),
                    UserId = Convert.ToInt32(objReader["user_id"]),
                    Image = objReader["wish_goods_image"] is DBNull ? null : (byte[])(objReader["wish_goods_image"]),
                    Name = Convert.ToString(objReader["goods_name"])
                });
            }
            objReader.Close();
            return list;
        }


        public static int InsertWishGoodsByProcedure(WishGoods wishGoods)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                SQLHelper.CreateParameter("@UserId", SqlDbType.Int, wishGoods.UserId),
                SQLHelper.CreateParameter("@Name", SqlDbType.NVarChar, wishGoods.Name),
                SQLHelper.CreateParameter("@Image", SqlDbType.Image, wishGoods.Image)
            };
            object sql = SQLHelper.UpdateByProcedureForObject("Wish_Goods_Insert", sqlParams);
            return Convert.ToInt32(sql);
        }

    }
}
