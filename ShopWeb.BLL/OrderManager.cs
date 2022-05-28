using System;
using System.Collections.Generic;
using ShopWeb.Models;
using ShopWeb.DAL;
using ShopWeb.Utils;

namespace ShopWeb.BLL
{
    public class OrderManager
    {
        #region 创建订单

        /// <summary>
        /// 生成订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns>返回订单号</returns>
        public static string GenerateOrder(int userId, NewOrderInfoModel model)
        {
            double cost = 0.0;

            model.goodsItemList.ForEach(item =>
            cost = DoubleOperationHelper.Add(
                cost, 
                DoubleOperationHelper.Multiply(item.Count, item.GoodsShowModel.Price)
                )
            );
            List<GoodsItem> goodsItems = new List<GoodsItem>();
            model.goodsItemList.ForEach(item => goodsItems.Add(new GoodsItem()
            {
                Goods = new Goods()
                {
                    Id = item.GoodsShowModel.Id,
                },
                Count = item.Count
            }));

            if (model.OrdersType == OrderType.SelfTake)
            {
                model.Address = null;
            }


            Order order =  new Order()
            {
                Id = GenerateOrderId(userId),
                Type = ((int)model.OrdersType),
                UserId = userId,
                ConsigneeName = model.ConsigneeName,
                ConsigneePhone = model.ConsigneePhone,
                Address = model.Address,
                Cost = cost,
                GoodsList = goodsItems,
                CreatedTime = DateTime.Now,
                ScheduledTime = model.ScheduledTime,
                State = Order.STATE_INITIAL
            };

            // 存入数据库
            SaveOrder(order);
            // 将购物车对应的商品删除
            DeleteOrderGoodsInCart(userId, order.GoodsList);

            return order.Id;
        }

        public static string GenerateOrderId(int user_id)
        {
            return string.Format("{0}_{1}", user_id, UsualUtils.GetTimeStamp());

        } 

        public static bool SaveOrder(Order order)
        {
            bool insertOrder = OrderService.InsertOrder(order);

            if (!insertOrder) return false;
            for (int i = 0; i < order.GoodsList.Count; i++)
            {
                bool insertGoods = 
                    OrderService.InsertOrderGoods(order.Id, 
                    order.GoodsList[i].Goods.Id, order.GoodsList[i].Count);
                if (!insertGoods) return false;
            }
            return true;
        }

        public static void DeleteOrderGoodsInCart(int userId, List<GoodsItem> goodsItems)
        {
            goodsItems.ForEach(item => CartManager.DeletCartGoods(userId, item.Goods.Id));
        }

        #endregion


        #region 查询订单信息
        //!!!
        //在查询到订单信息后、返回给视图前，如果有待付款状态的订单，【必须】要检查订单状态，
        //以保证订单创建5分钟内未进行支付的订单自动转换成已取消状态
        //!!!

        public static List<Order> GetOrderThroughManager()
        {
            List<Order> orders = OrderService.GetOrdersExceptInitial();

            if (orders.Count <= 0)
            {
                return null;
            }

            orders.ForEach(order =>
            {
                order.GoodsList = OrderService.GetGoodsItemsByOrderId(order.Id);

                order.GoodsList.ForEach(item =>
                {
                    List<Goods> goods = GoodsService.GetGoodsById(item.Goods.Id);

                    if (goods != null || goods.Count <= 0)
                    {
                        item.Goods = goods[0];
                    }
                });
            });
            return orders;
        }



        public static List<Order> GetOrdersByUserId(int userId, int filter)
        {
            List<Order> orders = OrderService.GetOrderByUserId(userId, filter);
            if (orders.Count <= 0)
            {
                return null;
            }

            orders.ForEach(order =>
            {
                order.GoodsList = OrderService.GetGoodsItemsByOrderId(order.Id);

                order.GoodsList.ForEach(item =>
                {
                    List<Goods> goods = GoodsService.GetGoodsById(item.Goods.Id);

                    if (goods != null || goods.Count <= 0)
                    {
                        item.Goods = goods[0];
                    }
                });
            });

            // 检查状态，如果有变，在这里手动置为cancel
            orders.ForEach(order =>
            {
                if (CheckOrderPayState(order))
                {
                    order.State = Order.STATE_CANCELED;
                }
            });

            return orders;
        }


        public static Order GetOrderById(string orderId)
        {
            List<Order> result = OrderService.GetOrderById(orderId);
            if (result.Count <= 0)
            {
                return null;
            }

            Order order = result[0];
            order.GoodsList = OrderService.GetGoodsItemsByOrderId(orderId);

            order.GoodsList.ForEach(item =>
            {
                List<Goods> goods = GoodsService.GetGoodsById(item.Goods.Id);

                if (goods != null || goods.Count <= 0)
                {
                    item.Goods = goods[0];
                }
            });

            // 检查状态，如果有变，在这里手动置为cancel
            if (CheckOrderPayState(order))
            {
                order.State = Order.STATE_CANCELED;
            }
            return order;
        }

        #endregion

        /// <summary>
        /// 检查订单创建5分钟内是否支付
        /// 5分钟未支付将状态变为已取消
        /// </summary>
        /// <param name="order"></param>
        /// <returns>是否进行了状态转换</returns>
        public static bool CheckOrderPayState(Order order)
        {
            if (order.State == Order.STATE_INITIAL && (DateTime.Now - order.CreatedTime).TotalSeconds > 5 * 60)
            {
                string result = UpdateOrderState(order.Id, order.State, Order.STATE_CANCELED, false);
                return result.Length <= 0;
            } 
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 管理员完成订单
        /// </summary>
        /// <param name="managerId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string ManagerFinishOrder(int managerId, Order order)
        {
            if (!ManagerManager.CheckIdExist(managerId))
            {
                return "管理员id不存在";
            }

            for (int i = 0; i < order.GoodsList.Count; i++)
            {
                GoodsItem goods = order.GoodsList[i];
                if (!GoodsManager.updateGoodsInventory(goods.Goods.Id, goods.Goods.Inventory - goods.Count))
                {
                    return "更新商品库存出现错误，请重试。";
                }
            }

            string upateStateResult = UpdateOrderState(order.Id, order.State, Order.STATE_FINISHED, false);
            if (upateStateResult.Length > 0)
            {
                return upateStateResult;
            }

            UpdateOrderManagerId(order.Id, managerId);

            return "";
        }

        public static bool UpdateOrderManagerId(string orderId, int managerId)
        {
            return OrderService.UpdateOrderManagerId(orderId, managerId);
        }


        /// <summary>
        /// 更新订单状态
        /// 0 --> 1
        /// 0 --> 2
        /// 1 --> 2
        /// 1 --> 3
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="preState"></param>
        /// <param name="newState"></param>
        /// <param name="isUser">是否是用户(反之为管理员)</param>
        /// <returns>执行结果语句，如果更新成功返回空字符串</returns>
        public static string UpdateOrderState(string orderId, int preState, int newState, bool isUser)
        {
            if (orderId == null || orderId.Length <= 0)
            {
                return "订单号有误";
            }

            // 订单状态转换逻辑判断
            if ((preState == newState) || (preState == 0 && newState == 3) ||
                (preState == 1 && newState == 0) || preState == 2 || preState == 3)
            {
                return "订单状态转换逻辑有误";
            }

            // 订单已经支付后 用户进行取消
            if (preState == 1 && newState == 2 && !isUser)
            {
                Order order = GetOrderById(orderId);
                // 距离预估时间小于三十分钟
                if ((order.ScheduledTime - DateTime.Now).TotalSeconds < 30 * 60)
                {
                    return "当前据订单预估完成时间小于30分钟，不能进行取消订单操作";
                }
            }

            if (OrderService.UpdateOrderState(orderId, newState))
            {
                if(newState == Order.STATE_FINISHED)
                {
                    OrderService.UpdateOrderRealTime(orderId, DateTime.Now);
                }
                return "";
            } 
            else
            {
                return "订单状态更新时出现错误";
            }
        }
    }
}
