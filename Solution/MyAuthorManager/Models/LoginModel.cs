using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAuthorManager.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "用户名不能为空")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        /// <summary>
        /// 客户是否勾选了记住密码选项
        /// </summary>
        public bool RememberMe { get; set; }
    }
}
