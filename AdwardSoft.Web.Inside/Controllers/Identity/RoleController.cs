using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class RoleController : Controller
    {
        #region Structure
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;
        public RoleController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }
        #endregion

        #region View
        public async Task<IActionResult> Index()
        {
            RoleViewModel roleModel = new RoleViewModel();
            var lstR = await _apiFactory.GetAsync<List<RoleViewModel>>("Role/Read", HostConstants.ApiIdentity, _userSession.BearerToken);
            ViewBag.IsDefault = lstR.FindAll(x => x.IsDefault == true).Count > 0 ? true : false;
            var tmpPermissions = await _apiFactory.GetAsync<List<PermissionViewModel>>("Permission/Read", HostConstants.ApiIdentity, _userSession.BearerToken);
            ViewBag.Select = tmpPermissions.Select(x => new SelectListItem() { Text = x.Controller + "." + x.Action, Value = x.Id.ToString() }).ToList();
            return View(roleModel);
        }
        #endregion

        #region Form
        public async Task<IActionResult> _Form(int id, int type)
        {
            

            var tmpPermissions = await _apiFactory.GetAsync<List<PermissionViewModel>>("Permission/Read", HostConstants.ApiIdentity, _userSession.BearerToken);
            ViewBag.Select = tmpPermissions.Select(x => new SelectListItem() { Text = x.Controller + "." + x.Action, Value = x.Id.ToString() });

            RoleViewModel model = new RoleViewModel();
            if (id > 0)
            {
                model = await _apiFactory.GetAsync<RoleViewModel>("Role/ReadById?Id=" + id, HostConstants.ApiIdentity, _userSession.BearerToken);
                var lst = await _apiFactory.GetAsync<List<RolePermissionViewModel>>("RolePermission/ReadById?Id=" + id, HostConstants.ApiIdentity, _userSession.BearerToken);
                List<int> permissions = new List<int>();
                foreach (var item in lst)
                    permissions.Add(item.PermissionId);
                model.Permissions = permissions;
                type = model.UserType;
            }

            var lstR = await _apiFactory.GetAsync<List<RoleViewModel>>("Role/Read", HostConstants.ApiIdentity, _userSession.BearerToken);
            ViewBag.IsDefault = lstR.FindAll(x => x.IsDefault == true && x.UserType == type).Count > 0 ? true : false;

            return PartialView(model);
        }

        public async Task<IActionResult> _ConfigForm(int id)
        {
            RoleConfigViewModel model = new RoleConfigViewModel();
            if (id > 0)
            {
                model = await _apiFactory.GetAsync<RoleConfigViewModel>("RoleConfig/ReadById?Id=" + id, HostConstants.ApiIdentity, _userSession.BearerToken);
            }
            return PartialView(model);
        }
        #endregion

        #region Read
        [HttpGet]
        public async Task<List<RoleViewModel>> Read()
        {
            var response = await _apiFactory.GetAsync<List<RoleViewModel>>("Role/Read", HostConstants.ApiIdentity, _userSession.BearerToken);
            return response;
        }
        [HttpGet]
        public async Task<List<RoleViewModel>> ReadByUserType(int userType)
        {
            var response = await _apiFactory.GetAsync<List<RoleViewModel>>("Role/ReadByUserType?userType=" + userType, HostConstants.ApiIdentity, _userSession.BearerToken);
            return response;
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(RoleViewModel vm)
        {
            ResponseContainer response = new ResponseContainer();
            var modelR = await _apiFactory.PostAsync<RoleViewModel, int>(vm, "Role/Create", HostConstants.ApiIdentity, _userSession.BearerToken);
            bool modelRP = true;
            List<RolePermissionViewModel> lst = new List<RolePermissionViewModel>();

            response.Action = "create";
            if (vm.Permissions != null)
            {
                foreach (int item in vm.Permissions)
                {
                    lst.Add(new RolePermissionViewModel()
                    {
                        PermissionId = item,
                        RoleId = modelR
                    });
                }

                var result = await _apiFactory.PostAsync<List<RolePermissionViewModel>, bool>(lst, "RolePermission/Create", HostConstants.ApiIdentity, _userSession.BearerToken);
                modelRP = result;
            }

            if (modelR > 0 && modelRP)
            {
                response.Activity = "Thêm mới";
            }
            else
            {
                response.Activity = "Thêm mới không";
            }
            return response;
        }

        [HttpPost]
        public async Task<ResponseContainer> CreateConfig(RoleConfigViewModel vm)
        {
            var result = await _apiFactory.PostAsync<RoleConfigViewModel, int>(vm, "RoleConfig/Create", HostConstants.ApiIdentity, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Activity = "Thêm mới";
            response.Succeeded = result > 0 ? true : false;
            return response;
        }
        #endregion

        #region Update
        [HttpPost]
        public async Task<ResponseContainer> Update(RoleViewModel vm)
        {
            ResponseContainer response = new ResponseContainer();
            var modelR = await _apiFactory.PutAsync<RoleViewModel, int>(vm, "Role/Update", HostConstants.ApiIdentity, _userSession.BearerToken);
            bool modelRP = true;
            List<RolePermissionViewModel> lst = new List<RolePermissionViewModel>();
            response.Action = "update";
            await _apiFactory.DeleteAsync<bool>("RolePermission/Delete?Id=" + vm.Id, HostConstants.ApiIdentity, _userSession.BearerToken);
            if (vm.Permissions != null)
            {
                foreach (int item in vm.Permissions)
                {
                    lst.Add(new RolePermissionViewModel()
                    {
                        PermissionId = item,
                        RoleId = vm.Id
                    });
                }                
                var result = await _apiFactory.PostAsync<List<RolePermissionViewModel>, bool>(lst, "RolePermission/Create", HostConstants.ApiIdentity, _userSession.BearerToken);
                modelRP = result;
            }

            if (modelRP)
            {
                response.Activity = "Điều chỉnh ";
            }
            else
            {
                response.Activity = "Điều chỉnh không ";
            }
            return response;
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(int id)
        {
            ResponseContainer response = new ResponseContainer();
            var result = await _apiFactory.DeleteAsync<bool>("Role/Delete?Id=" + id, HostConstants.ApiIdentity, _userSession.BearerToken);
            if (result)
            {
                response.Activity = "Xóa ";
            }
            else
            {
                response.Activity = "Xóa không ";
            }
            response.Action = "delete";
            return response;
        }
        #endregion
    }
}