using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Utilities.Helper;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class EmployeeController : Controller
    {
        #region Constructor

        private readonly IUserSession _userSession;
        private readonly IAPIFactory _apiFactory;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public EmployeeController(IUserSession userSession, IAPIFactory apiFactory, IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        #endregion

        #region View

        public async Task<IActionResult> Index()
        {
            ViewBag.Positions = (await _ReadPositionSelect())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });

            return View();
        }

        #endregion

        #region Form

        public async Task<IActionResult> _Form(int id)
        {
            var model = new EmployeeViewModel { Id = id };

            if (model.Id > 0)
                model = await _ReadByIdAsync(id);


            // Viewbag
            ViewBag.Positions = (await _ReadPositionSelect())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });
            ViewBag.Genders = (await _ReadGenderSelect())
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });

            return PartialView(model);
        }

        #endregion

        #region Read

        [HttpPost]
        public async Task<IActionResult> Read(DataTableAjaxPostModel model, int departmentId = 0, int positionId = 0)
        {
            if (String.IsNullOrEmpty(model.Search.Value))
                model.Search.Value = "NULL";

            var response = await _apiFactory.GetAsync<List<EmployeeDataTableViewModel>>(
                apiUrl: this.ApiResources($"Read?pageSize={model.Length}&pageSkip={model.Start}&searchBy={model.Search.Value}" +
                    $"&departmentId={departmentId}&positionId={positionId}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            int recordsTotal = response.Any() ? response.FirstOrDefault().Count : 0;

            return Json(new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = response });
        }

        [HttpGet]
        public async Task<EmployeeViewModel> ReadById(int id) => await _ReadByIdAsync(id);

        #endregion

        #region Create

        [HttpPost]
        public async Task<ResponseContainer> Create(EmployeeViewModel vm)
        {
            string[] DoBStr = vm.DoBStr.Split("/");
            vm.DoB = DateTime.Parse(DoBStr[2] + "-" + DoBStr[1] + "-" + DoBStr[0]);

            string[] StartingDateStr = vm.StartingDateStr.Split("/");
            vm.StartingDate = DateTime.Parse(StartingDateStr[2] + "-" + StartingDateStr[1] + "-" + StartingDateStr[0]);

            if (vm.AvatarFile != null)
            {

                vm.Avatar = (await DeleteOrUploadFile(
                    file: vm.AvatarFile,
                    deleteURL: null,
                    namefile: null)).ToString();

            }

            vm.Code = Helper.StringHelper.RandomString(10);

            var result = await _apiFactory.PostAsync<EmployeeViewModel, int>(
                data: vm,
                apiUrl: this.ApiResources("Create"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Activity = "Create",
                Action = "Create",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }

        #endregion

        #region Update

        [HttpPut]
        public async Task<ResponseContainer> Update(EmployeeViewModel vm)
        {
            string[] StartingDateStr = vm.StartingDateStr.Split("/");
            vm.StartingDate = DateTime.Parse(StartingDateStr[2] + "-" + StartingDateStr[1] + "-" + StartingDateStr[0]);

            string[] DoBStr = vm.DoBStr.Split("/");
            vm.DoB = DateTime.Parse(DoBStr[2] + "-" + DoBStr[1] + "-" + DoBStr[0]);

            if (vm.AvatarFile != null)
            {

                vm.Avatar = (await DeleteOrUploadFile(
                    file: vm.AvatarFile,
                    deleteURL: null,
                    namefile: null)).ToString();

            }

            var result = await _apiFactory.PutAsync<EmployeeViewModel, int>(
                data: vm,
                apiUrl: this.ApiResources("Update"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Activity = "Update",
                Action = "Update",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }

        #endregion

        #region Delete

        [HttpPost]
        public async Task<ResponseContainer> Delete(int id, string avatar)
        {
            var result = await _apiFactory.DeleteAsync<int>(
                apiUrl: this.ApiResources($"Delete?Id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            if (result > 0 && !string.IsNullOrEmpty(avatar))
            {
                var delete = (bool)await DeleteOrUploadFile(
                    file: null,
                    deleteURL: avatar,
                    namefile: null);
            }

            var response = new ResponseContainer
            {
                Action = "Delete",
                Activity = "Delete",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }

        #endregion

        #region Methods
        private async Task<EmployeeViewModel> _ReadByIdAsync(int id)
        {
            var response = await _apiFactory.GetAsync<EmployeeViewModel>(
                apiUrl: this.ApiResources($"ReadById?id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        private async Task<IEnumerable<Select>> _ReadPositionSelect()
        {
            var PositionSelect = await _apiFactory.GetAsync<IEnumerable<Select>>
            (
                apiUrl: $"Position/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return PositionSelect;
        }

        private async Task<IEnumerable<Select>> _ReadGenderSelect()
        {
            var PositionSelect = await _apiFactory.GetAsync<IEnumerable<Select>>
            (
                apiUrl: $"Gender/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return PositionSelect;
        }

        #endregion

        #region Search
        [HttpGet]
        public async Task<JsonResult> Search(string searchTerm, int pageSize, int pageNumber)
        {
            if (String.IsNullOrEmpty(searchTerm))
            {
                return Json(new EmployeeSearch());
            }
            string strRequest = "Employee/Search?pageSize=" + pageSize + "&pagenumber=" + (pageNumber - 1)
                + "&keyword=" + searchTerm;
            var result = await _apiFactory.GetAsync<EmployeeSearch>(strRequest, Endpoints.ApiPOS, _userSession.BearerToken);
            return Json(result);
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
