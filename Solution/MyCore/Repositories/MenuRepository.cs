using MyDomain.Entities;
using MyDomain.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCore.Repositories
{
    public class MenuRepository: RepositoryBase<Menu>, IMenuRepository
    {
        public MenuRepository(MyDbContext dbcontext) : base(dbcontext)
        {

        }
    }
}
