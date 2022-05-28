using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models
{
    public class WishGoodsModel
    {
        [Required(ErrorMessage = "请填写商品名称")]
        [Display(Name = "商品名称")]
        public string Name { get; set; }

        [Display(Name = "商品图片")]
        public string ImageUrl { get; set; }
    }
}
