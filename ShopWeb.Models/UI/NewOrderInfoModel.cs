using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models
{
    public enum OrderType
    {
        [Display(Name = "上门配送")]
        Deliverty = 0,
        [Display(Name = "到店自取")]
        SelfTake = 1
    }

    public class NewOrderInfoModel
    {
        [Required(ErrorMessage = "请输入收货人姓名")]
        [Display(Name = "收货人姓名")]
        public string ConsigneeName { get; set; }

        [Required(ErrorMessage = "请输入收货人联系电话")]
        [Display(Name = "联系电话")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "输入电话格式有误")]
        [DataType(DataType.PhoneNumber)]
        public string ConsigneePhone { get; set; }

        [Required(ErrorMessage = "请选择收货时间")]
        [Display(Name = "收货时间")]
        [DataType(DataType.DateTime)]
        public DateTime ScheduledTime { get; set; }

        [Required(ErrorMessage = "请选择收货方式")]
        [Display(Name = "收货方式")]
        [EnumDataType(typeof(OrderType))]
        public OrderType OrdersType { get; set; }


        [Display(Name = "送货地址")]
        [Required(ErrorMessage = "请填写送货地址")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        public List<OrderGoodsShowItemModel> goodsItemList {get; set;}

    }
}
