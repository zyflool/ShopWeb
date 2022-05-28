using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace ShopWeb.Models
{
    public class GoodsEditModel
    {
        [Required(ErrorMessage = "请填写商品Id")]
        [Display(Name = "商品Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "请填写商品名称")]
        [Display(Name = "商品名称")]
        public string Name { get; set; }

        [Required(ErrorMessage = "请填写商品单价")]
        [Display(Name = "商品单价")]
        public double Price { get; set; }

        [Required(ErrorMessage = "请填写商品库存量")]
        [Display(Name = "商品库存量")]
        public int Inventory { get; set; }

        [Display(Name = "商品图片")]
        [Required(ErrorMessage = "请上传商品图片")]
        public string ImageUrl { get; set; }

        public GoodsEditModel(Goods goods)
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
                    this.ImageUrl = base64;
                }
            }
        }
        public GoodsEditModel()
        {

        }
    }
}