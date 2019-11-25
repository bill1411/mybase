using MyDomain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyDomain.IRepositories
{
    public interface IUserRepository :IRepository<User>
    {
        /// <summary>
        /// 检查用户是存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>存在返回用户实体，否则返回NULL</returns>
        User CheckUser(string userName, string password);

        /// <summary>
        /// 根据用户ID，获取用户角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User GetWithRoles(Guid id);
    }
}
