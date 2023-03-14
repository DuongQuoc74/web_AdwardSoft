﻿using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class PermissionController : Controller
    {
        #region Structure
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;
        public PermissionController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }
        #endregion

        #region view
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Read
        [HttpGet]
        public async Task<List<PermissionViewModel>> Read()
        {
            var response = await _apiFactory.GetAsync<List<PermissionViewModel>>("Permission/Read", HostConstants.ApiIdentity, _userSession.BearerToken);
            return response;
        }
        #endregion

        #region Create, Update
        [HttpPost]
        public async Task<ResponseContainer> Create(PermissionViewModel permission)
        {
            var response = new ResponseContainer();
            var result = await _apiFactory.PostAsync<PermissionViewModel, int>(permission, this.ApiResources("Create"), HostConstants.ApiIdentity, _userSession.BearerToken);
            if (result < 0)
            {
                throw new Exception($"Thất bại khi tạo mới");
            }
            response.Activity = $"Tạo mới permission ";
            response.Action = "create";
            return response;
        }

        [HttpPost]
        public async Task<ResponseContainer> Update(PermissionViewModel permission)
        {
            var response = new ResponseContainer();
            var result = await _apiFactory.PutAsync<PermissionViewModel, int>(permission, this.ApiResources("Update"), HostConstants.ApiIdentity, _userSession.BearerToken);
            if (result < 0)
            {
                throw new Exception($"Thất bại khi Điều chỉnh");
            }
            response.Activity = $"Điều chỉnh permission ";
            response.Action = "update";
            return response;
        }

        [HttpGet]
        public async Task<JsonResult> IsActionControllerExist(int Id, string ControllerName, string ActionName)
        {
            if (ControllerName != null && ActionName != null)
            {
                var resultCheck = await _apiFactory.GetAsync<PermissionViewModel>(this.ApiResources("CheckExsit?con=") + ControllerName + "&ac=" + ActionName + "&id=" + Id, HostConstants.ApiAuth, _userSession.BearerToken);
                return Json(resultCheck.Id > 0 ? false : true);
            }
            return Json(true);
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(string id)
        {
            var response = new ResponseContainer();
            var resultCheck = await _apiFactory.GetAsync<PermissionViewModel>(this.ApiResources("CheckUse/") + id, HostConstants.ApiIdentity, _userSession.BearerToken);
            if (resultCheck.Id > 0)
            {
                throw new Exception("Permission đã được sử dụng");
            }
            var result = await _apiFactory.DeleteAsync<int>(this.ApiResources("Delete?id=" + id), HostConstants.ApiIdentity, _userSession.BearerToken);
            if (result < 0)
                throw new Exception("Thất bại khi xóa permission");
            response.Action = "delete";
            response.Activity = $"Xóa permission ";
            return response;
        }
        #endregion

        #region Form
        [HttpGet]
        public async Task<IActionResult> _Form(int id)
        {
            try
            {
                var model = new PermissionViewModel();
                ViewBag.IsCreate = true;
                if (id != 0)
                {
                    model = await _apiFactory.GetAsync<PermissionViewModel>(this.ApiResources("ReadById?id=" + id), HostConstants.ApiIdentity, _userSession.BearerToken);
                    ViewBag.IsCreate = false;
                }
                return PartialView(model);
            }
            catch (Exception)
            {
                throw new Exception("Không thể tạo mẫu!");
            }

        }
        #endregion
    }
}