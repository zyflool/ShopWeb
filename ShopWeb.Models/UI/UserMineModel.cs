using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopWeb.Models
{
    public class UserMineModel
    {
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Display(Name = "联系电话")]
        public string Phone { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
    }
}