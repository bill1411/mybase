using MyDomain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApplication.UserApp
{
    public interface IUserAppService
    {
        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        User CheckUser(string userName, string password);
    }
}
