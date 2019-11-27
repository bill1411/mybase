using MyDomain.Entities;
using MyDomain.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCore.Repositories
{
    public class DepartmentRepository:RepositoryBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(MyDbContext dbcontext) : base(dbcontext)
        {

        }
    }
}
