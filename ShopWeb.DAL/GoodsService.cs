using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopWeb.Models;
using static System.Net.Mime.MediaTypeNames;

namespace ShopWeb.DAL
{
    public class GoodsService
    {
        #region 查询语句
        public static List<Goods> GetAllGoods()
        {
            return GetGoodsBySql("where goods_is_deleted = 0");
        }

        public static List<Goods> GetGoodsById(int id)
        {
            string whereSql = string.Format("where goods_id = {0}", id);
            return GetGoodsBySql(whereSql);
        }

        public static List<Goods> GetGoodsByName(string name)
        {
            string whereSql = string.Format("where goods_name = '{0}'", name);
            return GetGoodsBySql(whereSql);
        }

        public static List<Goods> GetGoodsListByName(string name)
        {
            string whereSql = string.Format("where goods_name LIKE '%{0}%' and goods_is_deleted = 0", name);
            return GetGoodsBySql(whereSql);
        }

        public static List<Goods> GetGoodsBySql(string wheresql)
        {
            string sql = "select * from Goods ";
            sql += wheresql;

            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<Goods> list = new List<Goods>();
            while (objReader.Read())
            {
                list.Add(new Goods()
                {
                    Id = Convert.ToInt32(objReader["goods_id"]),
                    Image = objReader["goods_image"] is System.DBNull ? null : (byte[])(objReader["goods_image"]),
                    Price = Convert.ToDouble(objReader["goods_price"]),
                    Inventory = Convert.ToInt32(objReader["goods_inventory"]),
                    Name = Convert.ToString(objReader["goods_name"]),
                    IsDeleted = Convert.ToBoolean(objReader["goods_is_deleted"])
                });
            }
            objReader.Close();
            return list;
        }

        public static List<Goods> GetOrderedGoodsListBySql()
        {
            string sql = "select * from [Goods] LEFT Join [Sold_Goods] on [Goods].goods_id = [Sold_Goods].goods_id" +
                " order by [Sold_Goods].goods_count DESC";

            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<Goods> list = new List<Goods>();
            while (objReader.Read())
            {
                list.Add(new Goods()
                {
                    Id = Convert.ToInt32(objReader["goods_id"]),
                    Image = objReader["goods_image"] is System.DBNull ? null : (byte[])(objReader["goods_image"]),
                    Price = Convert.ToDouble(objReader["goods_price"]),
                    Inventory = Convert.ToInt32(objReader["goods_inventory"]),
                    Name = Convert.ToString(objReader["goods_name"]),
                    IsDeleted = Convert.ToBoolean(objReader["goods_is_deleted"])
                });
            }
            objReader.Close();
            return list;
        }

        #endregion

        #region 更新语句

        public static bool UpdateGoodsByParameters(List<Parameter> parameters)
        {
            string updateSql = "set ";
            for (int i = 0; i < parameters.Count; i++)
            {
                string kv;
                if (parameters[i].Value is string)
                {
                    kv = string.Format("{0} = '{1}'", parameters[i].Name, parameters[i].Value);
                }
                else
                {
                    kv = string.Format("{0} = {1}", parameters[i].Name, parameters[i].Value);
                }

                if (i < parameters.Count - 2)
                {
                    updateSql += kv + ", ";
                }
                else if (i == parameters.Count - 2)
                {
                    updateSql += kv + " ";

                }
                else if (i == parameters.Count - 1)
                {
                    updateSql += "where "+ kv;

                }
            }
            return UpdateGoodsBySql(updateSql);
        }

        public static bool UpdateGoodsBySql(string updatesql)
        {
            string sql = "update Goods ";
            sql += updatesql;
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

        public static bool UpdateGoodsByProcedure(Goods goods)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                SQLHelper.CreateParameter("@Id", SqlDbType.Int, goods.Id),
                SQLHelper.CreateParameter("@Name", SqlDbType.NVarChar,  goods.Name),
                SQLHelper.CreateParameter("@Price", SqlDbType.NVarChar,  goods.Price),
                SQLHelper.CreateParameter("@Inventory", SqlDbType.Int, goods.Inventory),
                SQLHelper.CreateParameter("@Image", SqlDbType.Image,  goods.Image)
            };
            int result = SQLHelper.UpdateByProcedure("Goods_Update", sqlParams);
            return result > 0;
        }

        #endregion

        #region 新增语句

        public static int InsertGoodsByProcedure(Goods goods)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                SQLHelper.CreateParameter("@Name", SqlDbType.NVarChar, goods.Name),
                SQLHelper.CreateParameter("@Price", SqlDbType.NVarChar, goods.Price),
                SQLHelper.CreateParameter("@Inventory", SqlDbType.Int, goods.Inventory),
                SQLHelper.CreateParameter("@Image", SqlDbType.Image, goods.Image)
            };
            object sql = SQLHelper.UpdateByProcedureForObject("Goods_Insert", sqlParams);
            return Convert.ToInt32(sql);
        }

        #endregion
    }
}
