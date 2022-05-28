using System;
using System.IO;

namespace ShopWeb.Models
{
    public class GoodsShowModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int Inventory { get; set; }

        public string ImagePath { get; set; }

        public GoodsShowModel(Goods goods)
        {
            if (goods != null)
            {
                this.Name = goods.Name;
                this.Price = goods.Price;
                this.Inventory = goods.Inventory;
                this.Id = goods.Id;
                if (goods.Image != null)
                {
                    MemoryStream MStream = new MemoryStream(goods.Image);
                    string base64 = Convert.ToBase64String(MStream.ToArray());
                    this.ImagePath = base64;
                }
            }
        }

        public GoodsShowModel()
        {

        }
    }
}
