﻿using AutoMapper;
using MyApplications.UserApp.Dtos;
using MyDomain.Entities;
using MyDomain.IRepositories;
using System;
using System.Collections.Generic;

namespace MyApplications
{
    public class UserAppService : IUserAppService
    {
        //用户管理仓储接口
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        /// <summary>
        /// 构造函数 实现依赖注入
        /// </summary>
        /// <param name="userRepository">仓储对象</param>
        public UserAppService(IUserRepository userRepository,IMapper mapper)
        {
            _repository = userRepository;
            _mapper = mapper;
        }

        public User CheckUser(string userName, string password)
        {
            return _repository.CheckUser(userName, password);
        }
        public List<UserDto> GetUserByDepartment(Guid departmentId, int startPage, int pageSize, out int rowCount)
        {
            return _mapper.Map<List<UserDto>>(_repository.LoadPageList(startPage, pageSize, out rowCount, it => it.DepartmentId == departmentId, it => it.CreateTime));
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        /// <param name="dto">实体</param>
        /// <returns></returns>
        public UserDto InsertOrUpdate(UserDto dto)
        {
            if (Get(dto.Id) != null)
                _repository.Delete(dto.Id);
            var user = _repository.InsertOrUpdate(_mapper.Map<User>(dto));
            return _mapper.Map<UserDto>(user);
        }
        /// <summary>
        /// 根据Id集合批量删除
        /// </summary>
        /// <param name="ids">Id集合</param>
        public void DeleteBatch(List<Guid> ids)
        {
            _repository.Delete(it => ids.Contains(it.Id));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">Id</param>
        public void Delete(Guid id)
        {
            _repository.Delete(id);
        }

        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public UserDto Get(Guid id)
        {
            return _mapper.Map<UserDto>(_repository.GetWithRoles(id));
        }
    }
}
