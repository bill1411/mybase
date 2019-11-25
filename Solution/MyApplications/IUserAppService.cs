using MyDomain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApplications
{
    public interface IUserAppService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        User CheckUser(string userName, string password);
    }
}
