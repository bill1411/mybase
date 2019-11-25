using MyDomain.Entities;
using MyDomain.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApplication.UserApp
{
    public class UserAppService:IUserAppService
    {
        //用户管理仓储接口
        private readonly IUserRepository _userReporitory;

        /// <summary>
        /// 构造函数 实现依赖注入
        /// </summary>
        /// <param name="userRepository">仓储对象</param>
        public UserAppService(IUserRepository userRepository)
        {
            _userReporitory = userRepository;
        }

        public User CheckUser(string userName, string password)
        {
            return _userReporitory.CheckUser(userName, password);
        }
    }
}
