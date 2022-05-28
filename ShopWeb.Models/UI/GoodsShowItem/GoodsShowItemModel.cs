using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Models
{
    public class GoodsShowItemModel
    {
        public GoodsShowModel GoodsShowModel { get; set; }

        public int Count { get; set; }

        public GoodsShowItemModel()
        {

        }

        public GoodsShowItemModel(GoodsItem goodsItem)
        {
            this.Count = goodsItem.Count;
            this.GoodsShowModel = new GoodsShowModel(goodsItem.Goods);
        }
    }
}
