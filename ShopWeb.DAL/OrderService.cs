using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopWeb.Models;

namespace ShopWeb.DAL
{
    public class OrderService
    {
        #region 查询

        public static List<Order> GetOrdersExceptInitial()
        {
            string sql =
                string.Format("where order_state = {0} or order_state = {1} or order_state = {2} order by order_state asc", Order.STATE_PAYED, Order.STATE_FINISHED, Order.STATE_CANCELED);
            return GetOrderBySql(sql);
        }
        public static List<Order> GetOrderById(string orderId)
        {
            string sql = string.Format("where order_id = \'{0}\'", orderId);
            return GetOrderBySql(sql);
        }

        public static List<Order> GetOrderByUserId(int userId)
        {
            string sql = string.Format("where user_id = {0} order by order_state asc", userId);
            return GetOrderBySql(sql);
        }

        public static List<Order> GetOrderByUserId(int userId,int filter)
        {
            if (filter == 4)
            {
                string sql = string.Format("where user_id = {0} order by order_state asc", userId);
                return GetOrderBySql(sql);

            } else
            {
                string sql = string.Format("where user_id = {0} and order_state = {1} order by order_state asc", userId, filter);
                return GetOrderBySql(sql);
            }
        }

        public static List<Order> GetOrderBySql(string querySql)
        {
            string sql = "select * from [Order] ";
            sql += querySql;

            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<Order> orders = new List<Order>();
            while(objReader.Read())
            {
                orders.Add(new Order()
                {
                    Id = Convert.ToString(objReader["order_id"]),
                    Cost = (double)Convert.ToDecimal(objReader["order_cost"]),
                    ConsigneeName = Convert.ToString(objReader["order_consignee_name"]),
                    ConsigneePhone = Convert.ToString(objReader["order_consignee_phone"]),
                    Address = objReader["order_address"] is DBNull ? null :Convert.ToString(objReader["order_address"]),
                    ScheduledTime = Convert.ToDateTime(objReader["order_scheduled_time"]),
                    CreatedTime = Convert.ToDateTime(objReader["order_created_time"]),
                    RealTime = objReader["order_real_time"] is DBNull ? DateTime.MaxValue: Convert.ToDateTime(objReader["order_real_time"]),
                    State = Convert.ToInt32(objReader["order_state"]),
                    Type = Convert.ToInt32(objReader["order_type"]),
                    UserId = Convert.ToInt32(objReader["user_id"]),
                    ManagerId = objReader["manager_id"] is DBNull ? 0 : Convert.ToInt32(objReader["manager_id"])
                });
            }
            return orders;
        }


        public static List<GoodsItem> GetGoodsItemsByOrderId(string orderId)
        {
            string sql = string.Format("where order_id = \'{0}\'", orderId);
            return GetGoodsItemsBySql(sql);
        }

        public static List<GoodsItem> GetGoodsItemsBySql(string querySql)
        {
            string sql = "select * from [Order_Goods] ";
            sql += querySql;
            List<GoodsItem> goodsIds = new List<GoodsItem>();
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            while (objReader.Read())
            {
                goodsIds.Add(new GoodsItem()
                {
                    Goods = new Goods()
                    {
                        Id = Convert.ToInt32(objReader["goods_id"])
                    },
                    Count = Convert.ToInt32(objReader["order_goods_count"])
                });
            }
            return goodsIds;
        }


        #endregion


        #region 更新
        public static bool UpdateOrderManagerId(string id, int managerId)
        {
            string updateSql =
                string.Format("set manager_id = {0} where order_id = \'{1}\'", managerId, id);
            return UpdateOrderBySql(updateSql);
        }

        public static bool UpdateOrderRealTime(string id, DateTime realTime)
        {
            string updateSql =
                string.Format("set order_real_time = \'{0}\' where order_id = \'{1}\'", realTime, id);
            return UpdateOrderBySql(updateSql);
        }
        public static bool UpdateOrderState(string id, int state)
        {
            string updateSql =
                string.Format("set order_state = {0} where order_id = \'{1}\'", state, id);
            return UpdateOrderBySql(updateSql);
        }

        public static bool UpdateOrderBySql(string updatesql)
        {
            string sql = "update [Order] ";
            sql += updatesql;
            return UpdateSql(sql);
        }

        #endregion

        #region 新增

        public static bool InsertOrder(Order order)
        {
            string insertSql = 
                "(order_id, user_id, order_type, order_consignee_name, order_consignee_phone, order_created_time, " +
                "order_scheduled_time, order_address, order_state, order_cost) values ";
            insertSql += string.Format("(\'{0}\', {1}, {2},\'{3}\',\'{4}\', \'{5}\', \'{6}\', \'{7}\', {8},{9})", 
                order.Id, order.UserId, order.Type,order.ConsigneeName, order.ConsigneePhone,
                order.CreatedTime, order.ScheduledTime, 
                order.Address, order.State, order.Cost);
            return InsertOrderBySql(insertSql);
        }

        public static bool InsertOrderBySql(string insertSql)
        {
            string sql = "insert into [Order] ";
            sql += insertSql;
            return UpdateSql(sql);
        }

        #endregion

        public static bool UpdateSql(string sql)
        {
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

        #region 商品列表相关信息

        public static bool InsertOrderGoods(string orderId, int goodsId, int count)
        {
            string insertSql =
                string.Format("(order_id, goods_id, order_goods_count) values " +
                "(\'{0}\', {1}, {2})", orderId, goodsId, count);
            return InsertOrderGoodsBySql(insertSql);
        }

        public static bool InsertOrderGoodsBySql(string insertSql)
        {
            string sql = "insert into Order_Goods ";
            sql += insertSql;
            try
            {
                int result = SQLHelper.Update(sql);
                return (result > 0);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
    }
}