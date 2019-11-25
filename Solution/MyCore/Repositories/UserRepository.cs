using MyDomain.Entities;
using MyDomain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCore.Repositories
{
    public class UserRepository:RepositoryBase<User>, IUserRepository
    {
        public UserRepository(MyDbContext dbcontext) : base(dbcontext)
        {

        }

        /// <summary>
        /// 根据用户名及密码获取用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public User CheckUser(string userName, string password)
        {
            return _dbContext.Set<User>().FirstOrDefault(it=>it.Password == password && it.UserName == userName);
        }

        /// <summary>
        /// 根据Id获取实体及角色信息
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public User GetWithRoles(Guid id)
        {
            var user = _dbContext.Set<User>().FirstOrDefault(it => it.Id == id);
            if (user != null)
            {
                user.UserRoles = _dbContext.Set<UserRole>().Where(it => it.UserId == id).ToList();
            }
            return user;
        }
    }
}
