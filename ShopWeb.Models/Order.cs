using System;
using System.Collections.Generic;

namespace ShopWeb.Models
{
    public class Order
    {
        public const int STATE_INITIAL = 0; // 待付款
        public const int STATE_PAYED = 1; // 待收货
        public const int STATE_CANCELED = 2; // 已取消
        public const int STATE_FINISHED = 3; // 已完成

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

        public List<GoodsItem> GoodsList {get; set;}
    }
}
