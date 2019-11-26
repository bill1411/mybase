using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApplications;
using MyAuthorManager.Models;
using MyUtility;

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

        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                //检查用户信息
                var user = _userAppService.CheckUser(model.UserName, model.Password);
                if (user != null)//说明有用户信息
                {
                    //记录Session
                    HttpContext.Session.SetString("CurrentUserId", user.Id.ToString());
                    HttpContext.Session.Set("CurrentUser", ByteConvertHelper.Object2Bytes(user));
                    //跳转到系统首页
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.ErrorInfo = "用户名或密码错误。";
                return View();
            }
            ViewBag.ErrorInfo = ModelState.Values.First().Errors[0].ErrorMessage;
            return View(model);
        }
    }
}