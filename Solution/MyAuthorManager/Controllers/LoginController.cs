using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyApplications;

namespace MyAuthorManager.Controllers
{
    public class LoginController : Controller
    {
        //用户管理仓储接口
        private IUserAppService _userAppService;


        /// <summary>
        /// 构造函数 实现依赖注入
        /// </summary>
        /// <param name="userRepository">仓储对象</param>
        public LoginController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}