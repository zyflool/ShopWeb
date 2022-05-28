using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.Models
{
    public class ChangePhoneModel
    {
        [Required(ErrorMessage = "请填写原联系电话")]
        [StringLength(11, ErrorMessage = "电话格式有误", MinimumLength = 11)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "原联系电话")]
        public string OldPhoneNumber { get; set; }


        [Required(ErrorMessage = "请填写新联系电话")]
        [StringLength(11, ErrorMessage = "电话格式有误", MinimumLength = 11)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "新联系电话")]
        public string NewPhoneNumber { get; set; }

    }
}
