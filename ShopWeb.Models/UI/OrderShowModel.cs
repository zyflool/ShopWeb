using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Models
{
    public class OrderShowModel
    {
        public string Id { get; set; }
        public int UserId { get; set; }
        public int ManagerId { get; set; }
        public string ConsigneeName { get; set; }

        public string ConsigneePhone { get; set; }
        public DateTime CreatedTime { get; set; }

        public DateTime ScheduledTime { get; set; }

        public DateTime RealTime { get; set; }

        public string Address { get; set; }
        public double Cost { get; set; }

        public int Type { get; set; }

        public int State { get; set; }

        public List<OrderGoodsShowItemModel> GoodsList { get; set; }

        public OrderShowModel()
        {

        }

        public OrderShowModel(Order order)
        {
            this.Id = order.Id;
            this.UserId = order.UserId;
            this.ManagerId = order.ManagerId;
            this.ConsigneeName = order.ConsigneeName;
            this.ConsigneePhone = order.ConsigneePhone;
            this.CreatedTime = order.CreatedTime;
            this.ScheduledTime = order.ScheduledTime;
            this.RealTime = order.RealTime;
            this.Address = order.Address;
            this.Cost = order.Cost;
            this.Type = order.Type;
            this.State = order.State;
            List<OrderGoodsShowItemModel> goodsItems = new List<OrderGoodsShowItemModel>();
            order.GoodsList.ForEach(goodsItem => goodsItems.Add(new OrderGoodsShowItemModel(goodsItem)));
            this.GoodsList = goodsItems;
        }

    }
}
