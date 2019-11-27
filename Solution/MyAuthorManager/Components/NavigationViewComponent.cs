using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApplications;
using MyApplications.MenuApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAuthorManager.Components
{
    [ViewComponent(Name = "Navigation")]
    public class NavigationViewComponent : ViewComponent
    {
        private readonly IUserAppService _userAppService;
        private readonly IMenuAppService _menuAppService;
        
        public NavigationViewComponent(IUserAppService userAppService,IMenuAppService menuAppService)
        {
            _userAppService = userAppService;
            _menuAppService = menuAppService;
        }

        public IViewComponentResult Invoke()
        {
            var userId = "c2c57210-f247-4b3f-a1d2-ad988af7c73f"; //HttpContext.Session.GetString("CurrentUserId");
            var menus = _menuAppService.GetMenusByUser(Guid.Parse(userId));
            return View(menus);
        }
    }
}
