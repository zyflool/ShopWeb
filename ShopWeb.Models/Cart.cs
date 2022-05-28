using ShopWeb.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ShopWeb.Models
{
    public class Cart
    {

        private readonly List<GoodsItem> _goodsItems = new List<GoodsItem>();

        /// <summary>
        /// 获取购物车的所有项目
        /// </summary>
        public IList<GoodsItem> GetGoodsItems => _goodsItems;

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="goods"></param>
        /// <param name="count"></param>
        public void AddGoods(Goods goods, int count)
        {
            if (_goodsItems.Count == 0)
            {
                _goodsItems.Add(new GoodsItem() { Goods = goods, Count = count });
                return;
            }

            var model = _goodsItems.FirstOrDefault(x => x.Goods.Id == goods.Id);
            if (model == null)
            {
                _goodsItems.Add(new GoodsItem() { Goods = goods, Count = count });
                return;
            }
            model.Count += count;
        }

        /// <summary>
        /// 移除商品
        /// </summary>
        /// <param name="goods"></param>
        public void RemoveGoods(Goods goods)
        {
            var model = _goodsItems.FirstOrDefault(x => x.Goods.Id == goods.Id);
            if (model == null)
            {
                return;
            }

            _goodsItems.RemoveAll(x => x.Goods.Id == goods.Id);
        }

        /// <summary>
        /// 清空购物车
        /// </summary>
        public void Clear()
        {
            _goodsItems.Clear();
        }

        /// <summary>
        /// 统计总额
        /// </summary>
        /// <returns></returns>
        public double ComputeTotalValue()
        {
            return _goodsItems.Sum(x => DoubleOperationHelper.Multiply(x.Goods.Price, x.Count));
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        public List<GoodsItem> GoodsList { get; set; }
    }
}
