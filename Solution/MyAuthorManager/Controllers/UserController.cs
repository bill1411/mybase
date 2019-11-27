using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyApplications;
using MyApplications.RoleApp;
using MyApplications.UserApp.Dtos;

namespace MyAuthorManager.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserAppService _service;
        private readonly IRoleAppService _roleService;
        public UserController(IUserAppService service, IRoleAppService roleService)
        {
            _service = service;
            _roleService = roleService;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetUserByDepartment(Guid departmentId, int startPage, int pageSize)
        {
            int rowCount = 0;
            var result = _service.GetUserByDepartment(departmentId, startPage, pageSize, out rowCount);
            var roles = _roleService.GetAllList();
            return Json(new
            {
                rowCount = rowCount,
                pageCount = Math.Ceiling(Convert.ToDecimal(rowCount) / pageSize),
                rows = result,
                roles = roles
            });
        }

        public IActionResult Edit(UserDto dto, string roles)
        {
            try
            {
                if (dto.Id == Guid.Empty)
                {
                    dto.Id = Guid.NewGuid();
                }
                var userRoles = new List<UserRoleDto>();
                foreach (var role in roles.Split(','))
                {
                    userRoles.Add(new UserRoleDto() { UserId = dto.Id, RoleId = Guid.Parse(role) });
                }
                dto.UserRoles = userRoles;
                var user = _service.InsertOrUpdate(dto);
                return Json(new { Result = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Faild", Message = ex.Message });

            }
        }

        public IActionResult DeleteMuti(string ids)
        {
            try
            {
                string[] idArray = ids.Split(',');
                List<Guid> delIds = new List<Guid>();
                foreach (string id in idArray)
                {
                    delIds.Add(Guid.Parse(id));
                }
                _service.DeleteBatch(delIds);
                return Json(new
                {
                    Result = "Success"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Faild",
                    Message = ex.Message
                });
            }
        }
        public IActionResult Delete(Guid id)
        {
            try
            {
                _service.Delete(id);
                return Json(new
                {
                    Result = "Success"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Faild",
                    Message = ex.Message
                });
            }
        }
        public IActionResult Get(Guid id)
        {
            var dto = _service.Get(id);
            return Json(dto);
        }
    }
}