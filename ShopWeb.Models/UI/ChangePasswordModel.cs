using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopWeb.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "请填写旧密码")]
        [DataType(DataType.Password)]
        [Display(Name = "旧密码")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "请填写新密码")]
        [StringLength(100, ErrorMessage = "密码至少需要 {2} 位", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "请再次填写新密码以确认")]
        [Compare(nameof(NewPassword), ErrorMessage = "两次输入密码不一致")]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        public string ConfirmPassword { get; set; }
    }
}