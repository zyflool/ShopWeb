using ShopWeb.DAL;
using ShopWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.BLL
{
    public class WishGoodsManager
    {
        public static int GenerateWishGoods(int userId, string name, byte[] image)
        {
            int id = WishGoodsService.InsertWishGoodsByProcedure(new WishGoods()
            {
                UserId = userId,
                Name = name,
                Image = image
            });
            return id;
        }

        public static List<WishGoods> GetAllWishGoods()
        {
            return WishGoodsService.GetAllWishGoods();
        }

        public static List<WishGoods> SearchWishGoodsByName(string name)
        {
            return WishGoodsService.GetWishGoodsByName(name);
        }

        public static WishGoods GetWishGoodsById(int id)
        {
            return WishGoodsService.GetWishGoodsById(id);
        }


    }
}
