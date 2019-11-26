using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyAuthorManager.Controllers
{
    public abstract class ControllerBase : Controller
    {
        /// <summary>
        /// 控制器拦截，就是当我们直接通过在地址栏输入访问路由地址时，首先应该判断用户是否已经登录
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            byte[] result;
            string ss = HttpContext.Session.GetString("CurrentUserId");
            filterContext.HttpContext.Session.TryGetValue("CurrentUser", out result);
            if (result == null)
            {
                filterContext.Result = new RedirectResult("/Login/Index");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}