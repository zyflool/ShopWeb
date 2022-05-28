using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopWeb.Models
{
        public class LogupModel
        {

            [Required(ErrorMessage = "请填写用户名")]
            [Display(Name = "用户名")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "请输入联系电话")]
            [StringLength(11, MinimumLength = 11, ErrorMessage = "电话号码格式错误")]
            [DataType(DataType.PhoneNumber, ErrorMessage = "电话号码格式错误")]
            [Display(Name = "联系电话")]
            public string Phone { get; set; }

            [Required(ErrorMessage = "请填写密码")]
            [StringLength(100, ErrorMessage = "密码至少需要 {2} 位", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "密码")]
            public string Password { get; set; }

            [Required(ErrorMessage = "请再次填写密码以确认")]
            [Compare(nameof(Password), ErrorMessage = "两次输入密码不一致")]
            [DataType(DataType.Password)]
            [Display(Name = "确认密码")]
            public string ConfirmPassword { get; set; }
        }
}