using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class UserRoleController : Controller
    {
        #region Structure
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;
        public UserRoleController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }
        #endregion

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public async Task<List<UserRolesViewModel>> Read()
        {
            var response = await _apiFactory.GetAsync<List<UserRolesViewModel>>("UserRole/ReadRoles", HostConstants.ApiIdentity, _userSession.BearerToken);
            return response;
        }

        [HttpGet]
        public async Task<IActionResult> _Form(long userId)
        {
            try
            {
                var model = new UserRoleViewModel();
                var users = await _apiFactory.GetAsync<List<UserInfoViewModel>>("User/Read?status=" + 0, HostConstants.ApiIdentity, _userSession.BearerToken);

                
                if (userId > 0)
                {
                    var roleChecked = await _apiFactory.GetAsync<List<UserRoleViewModel>>("UserRole/Read?id=" + userId, HostConstants.ApiIdentity, _userSession.BearerToken);
                    List<int> lst = new List<int>();
                    foreach (var item in roleChecked)
                        lst.Add(item.Id);
                    model.RolesId = lst;

                    var tmpRoles = await _apiFactory.GetAsync<List<RoleViewModel>>("Role/ReadByUserType?userType=" + users.Where(x => x.Id == userId).FirstOrDefault().Type, HostConstants.ApiIdentity, _userSession.BearerToken);
                    ViewBag.Roles = tmpRoles;

                }
                else
                {
                    model.RolesId = new List<int>();

                    if (users.Count > 0)
                    {
                        var tmpRoles = await _apiFactory.GetAsync<List<RoleViewModel>>("Role/ReadByUserType?userType=" + users.FirstOrDefault().Type, HostConstants.ApiIdentity, _userSession.BearerToken);
                        ViewBag.Roles = tmpRoles;
                    }
                    else
                    {
                        ViewBag.Roles = new List<RoleViewModel>();
                    }
                }
                 
                model.Users = users;
               
                return PartialView(model);
            }
            catch (Exception ex)
            {
                throw new Exception("Không thể lấy biểu mẫu quyền!!!");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ResponseContainer> CreateOrUpdateRole(UserRoleViewModel vm)
        {
            ResponseContainer response = new ResponseContainer();

            var lst = new List<UserRoleViewModel>();
            if (vm.RolesId != null)
            {
                foreach (var item in vm.RolesId)
                {
                    var model = new UserRoleViewModel();
                    model.Id = item;
                    model.UserId = vm.UserId;
                    lst.Add(model);
                }

                var resul = await _apiFactory.PostAsync<List<UserRoleViewModel>, bool>(lst, "UserRole/Create", HostConstants.ApiIdentity, _userSession.BearerToken);
                response.Activity = "Chỉnh sửa quyền";
                response.Action = "create";
            }
            else
            {
                var model = new UserRoleViewModel()
                {
                    UserId = vm.UserId
                };
                lst.Add(model);
                var resul = await _apiFactory.PostAsync<List<UserRoleViewModel>, bool>(lst, "UserRole/Create", HostConstants.ApiIdentity, _userSession.BearerToken);
                response.Activity = "Chỉnh sửa quyền";
                response.Action = "create";
            }
            return response;
        }

        [HttpPost]
        public async Task<ResponseContainer> Delete(long id)
        {
            var response = new ResponseContainer();
            var result = await _apiFactory.DeleteAsync<bool>("UserRole/Delete?id=" + id, HostConstants.ApiIdentity, _userSession.BearerToken);
            if (result)
            {
                response.Activity = "Xóa";
            }
            else
            {
                throw new Exception("Không thể xóa");
            }
            response.Action = "delete";
            return response;
        }
    }
}