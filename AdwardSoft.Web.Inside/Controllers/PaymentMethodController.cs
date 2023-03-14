using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class PaymentMethodController : Controller
    {
        #region Constructor

        private readonly IUserSession _userSession;
        private readonly IAPIFactory _apiFactory;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public PaymentMethodController(IUserSession userSession, IAPIFactory apiFactory, IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        #endregion

        #region View

        public IActionResult Index()
        {
            return View();
        }

        #endregion

        #region Form

        public async Task<IActionResult> _Form(int id)
        {
            var model = new PaymentMethodViewModel { Id = id };

            if (model.Id > 0)
            {
                model = await _ReadByIdAsync(id);
            }

            return PartialView(model);
        }

        #endregion

        #region Read

        [HttpGet]
        public async Task<IEnumerable<PaymentMethodViewModel>> Read()
        {
            var response = await _apiFactory.GetAsync<IEnumerable<PaymentMethodViewModel>>(
                apiUrl: this.ApiResources($"Read"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        [HttpGet]
        public async Task<PaymentMethodViewModel> ReadById(int id) => await _ReadByIdAsync(id);
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(PaymentMethodViewModel vm)
        {
            if (vm.IconFile != null)
            {

                vm.Icon = (await DeleteOrUploadFile(
                    file: vm.IconFile,
                    deleteURL: null,
                    namefile: null)).ToString();

            }

            var result = await _apiFactory.PostAsync<PaymentMethodViewModel, int>(
                data: vm,
                apiUrl: this.ApiResources("Create"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Activity = "Thêm mới",
                Action = "Create",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }
        #endregion

        #region Update
        [HttpPut]
        public async Task<ResponseContainer> Update(PaymentMethodViewModel vm)
        {
            if (vm.IconFile != null)
            {
              
                vm.Icon = (await DeleteOrUploadFile(
                    file: vm.IconFile,
                    deleteURL: vm.Icon,
                    namefile: null)).ToString();

            }

            var result = await _apiFactory.PutAsync<PaymentMethodViewModel, int>(
                data: vm,
                apiUrl: this.ApiResources("Update"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Activity = "Điều chỉnh",
                Action = "Update",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(int id, string icon)
        {
            var result = await _apiFactory.DeleteAsync<int>(
                apiUrl: this.ApiResources($"Delete?Id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            if (result > 0 && !string.IsNullOrEmpty(icon))
            {
                var delete = (bool)await DeleteOrUploadFile(
                    file: null,
                    deleteURL: icon,
                    namefile: null);

            }

            var response = new ResponseContainer
            {
                Action = "Delete",
                Activity = "Xóa",
                Succeeded = result > 0 ? true : false
            };
            return response;
        }

        #endregion

        #region Methods
        private async Task<PaymentMethodViewModel> _ReadByIdAsync(int id)
        {
            var response = await _apiFactory.GetAsync<PaymentMethodViewModel>(
                apiUrl: this.ApiResources($"ReadById?id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }
        #endregion

        #region File
        public async Task<object> DeleteOrUploadFile(IFormFile file = null, string deleteURL = null, string namefile = null)
        {
            var controller = this.RouteData.Values["Controller"].ToString();
            if (!String.IsNullOrEmpty(deleteURL))
            {
                DeleteFileLocal(deleteURL);
            }

            if (file != null)
            {      
                var fileName = "";
                var webRoot = _hostingEnvironment.WebRootPath;
                var type = Path.GetExtension(file.FileName);
                var name = namefile ?? Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
                var PathWithFolderName = Path.Combine(webRoot, "file" + $@"\" + controller);
                if (!Directory.Exists(PathWithFolderName))
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(PathWithFolderName);
                }
                var fullPath = Path.Combine(PathWithFolderName, name);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                        fileName = _configuration["DomainFile"] + "file" + $@"/" + controller + $@"/" + name;
                }
                return fileName;
            }
            else
            {
                return true;
            }

        }
        public void DeleteFileLocal(string oldImageFullPath)
        {
            var controller = this.RouteData.Values["Controller"].ToString();
            if (!String.IsNullOrEmpty(oldImageFullPath))
            {
                oldImageFullPath = oldImageFullPath.Replace(_configuration["DomainFile"] + "file" + $@"/" + controller + $@"/", "");
                var webRoot = _hostingEnvironment.WebRootPath;
                var PathWithFolderName = Path.Combine(webRoot, "file" + $@"\" + controller);
                var oldPath = Path.Combine(PathWithFolderName, oldImageFullPath);
                if (!String.IsNullOrEmpty(oldPath) && System.IO.File.Exists(oldPath) && !String.IsNullOrEmpty(oldImageFullPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }

        }
        #endregion
    }
}
