
using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;
using MyDomain.Entities;
using MyApplications.MenuApp.Dtos;

namespace MyApplications
{
    public class MyMapper:Profile
    {
        public MyMapper()
        {
            CreateMap<MenuDto,Menu>();
            CreateMap<Menu,MenuDto>();
        }
    }
}
