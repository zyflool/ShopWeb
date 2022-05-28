using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopWeb.Models;
using ShopWeb.DAL;

namespace ShopWeb.BLL
{
    public class CartManager
    {
        public static int GenerateNewCartObject(int userId)
        {
            bool result = CartService.AddNewCart(userId);
            if (result)
            {
                int cartId = CartService.GetCartIdByUid(userId);
                UserManager.UpdateCartId(userId, cartId); // 存入user表
                return cartId;
            } 
            else
            {
                return 0;
            }
        }

        public static Cart GetCartByUid(int userId)
        {
            int cartId = CartService.GetCartIdByUid(userId);
            if (cartId < 0)
            {
                return null;
            }

            List<GoodsItem> items = CartService.GetCartAllGoodsItem(cartId);
            return new Cart()
            {
                Id = cartId,
                UserId = userId,
                GoodsList = items
            };
        }

        public static List<GoodsItem> GetCartGoodsItem(int userId)
        {
            List<int> ids = CartService.GetCartId(userId);
            if (ids == null || ids.Count == 0)
            {
                return new List<GoodsItem>();
            }
            int cartId = ids[0];
            List<GoodsItem> items = CartService.GetCartAllGoodsItem(cartId);
            if (items == null || items.Count == 0)
            {
                return new List<GoodsItem>();
            }
            for (int i = 0; i < items.Count; i++)
            {
                List<Goods> goods = GoodsService.GetGoodsById(items[i].Goods.Id);
                if (goods == null || goods.Count == 0)
                    items[i].Goods = null;
                else 
                    items[i].Goods = goods[0];
            }
            return items;
        }
        #region 增删商品
        public static bool AddCartGoods(int user_id, int goods_id, int goods_count)
        {
            int cartId = CartService.GetCartIdByUid(user_id);
            return CartService.AddGoodsItem(cartId, goods_id, goods_count);
        }

        public static bool DeletCartGoods(int user_id, int goods_id)
        {
            int cartId = CartService.GetCartIdByUid(user_id);
            return CartService.DeleteGoodsItem(cartId, goods_id);
        }
        #endregion

        #region 调整商品数量
        public static bool UpdateCartGoodsCount(int user_id, int goods_id, int goods_count)
        {
            int cartId = CartService.GetCartIdByUid(user_id);
            return CartService.UpdateGoodsCount(cartId, goods_id, goods_count);
        }
        #endregion

    }
}
