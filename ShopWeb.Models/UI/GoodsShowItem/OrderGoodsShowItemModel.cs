using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Models
{
    public class OrderGoodsShowItemModel : GoodsShowItemModel
    {
        public OrderGoodsShowItemModel(GoodsItem item): base(item)
        {
            this.Count = item.Count;
            this.GoodsShowModel = new GoodsShowModel(item.Goods);
        }

        public OrderGoodsShowItemModel() : base()
        {

        }
    }
}
