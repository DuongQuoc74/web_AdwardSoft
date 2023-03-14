using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class HomeController : Controller
    {
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;
        private IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public HomeController(
            IUserSession userSession,
            IAPIFactory apiFactory,
            IWebHostEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }
        #region AJAX
        public async Task<JsonResult> DashboardSumary()
        {
            var res = await _apiFactory.GetAsync<DashboardSumaryViewModel>("Dashboard/Sumary", Endpoints.ApiPOS, _userSession.BearerToken);
            return Json(res); ;
        }
        public async Task<JsonResult> TopProduct()
        {
            var res = await _apiFactory.GetAsync< List<DashboardPieViewModel>>("Dashboard/PieChart?type=1", Endpoints.ApiPOS, _userSession.BearerToken);
            return Json(new { name = res.Select(x => x.Name).ToArray(), value = res.ToArray() });
        }
        public async Task<JsonResult> TopCustomer()
        {
            var res = await _apiFactory.GetAsync< List<DashboardPieViewModel>>("Dashboard/PieChart?type=2", Endpoints.ApiPOS, _userSession.BearerToken);
            return Json(new { name = res.Select(x => x.Name).ToArray(), value = res.ToArray() });
        }
        public async Task<JsonResult> TopCategory()
        {
            var res = await _apiFactory.GetAsync<List<DashboardPieViewModel>>("Dashboard/PieChart?type=3", Endpoints.ApiPOS, _userSession.BearerToken);
            return Json(new { name = res.Select(x => x.Name).ToArray(), value = res.ToArray() });
        }
        [AllowAnonymous]
        public async Task<JsonResult> TotalSale()
        {
            var res = await _apiFactory.GetAsync<List<DashboardBarViewModel>>("Dashboard/BarChart?type=1", Endpoints.ApiPOS, _userSession.BearerToken);
            decimal[] barData = new decimal[12];
            for (int i = 1; i <= 12; i++)
            {
                var tmp = res.Any(x => x.Month == i) ? res.Find(x => x.Month == i).Amount : (decimal)0;
                barData[i - 1] = tmp;
            }
            return Json(barData);
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }

        public async void UpdateClaims(string image, string name)
        {
            ClaimsIdentity claimIdentity = HttpContext.User.Identities.First() as ClaimsIdentity;
            if (image != null && image != _userSession.Avatar)
            {
                Claim avatar = HttpContext.User.FindFirst(ClaimTypesConstants.Avatar);
                claimIdentity.RemoveClaim(avatar);
                claimIdentity.AddClaim(new Claim(ClaimTypesConstants.Avatar, image, ClaimValueTypes.String));
            }
            if (name != _userSession.FullName)
            {
                Claim fullname = HttpContext.User.FindFirst(ClaimTypes.Surname);
                claimIdentity.RemoveClaim(fullname);
                claimIdentity.AddClaim(new Claim(ClaimTypes.Surname, name, ClaimValueTypes.String));
            }
            ClaimsPrincipal principal = new ClaimsPrincipal(claimIdentity);
            await HttpContext.SignInAsync(principal);
        }
        
        public async Task<IActionResult> PasswordChange()
        {      
            try
            {
                UserViewModel model = new UserViewModel();

                var user = await _apiFactory.GetAsync<UserInsertViewModel>($"User/ReadById?id={_userSession.UserId}", HostConstants.ApiIdentity, _userSession.BearerToken);

                model.Id = long.Parse(_userSession.UserId);
                model.FullName = user.FullName;
                model.Email = user.Email;
                model.Username = user.UserName;
                model.Password = user.PasswordHash;
                model.Phone = user.PhoneNumber;
                model.Gender = user.Gender;

                return View(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ResponseContainer> UpdatePassword(UserViewModel model)
        {
            ResponseContainer response = new ResponseContainer();
            try
            {            
                var res = await _apiFactory.PutAsync<UserViewModel, int>(
                    model, 
                    "User/UpdatePassword", 
                    HostConstants.ApiIdentity, 
                    _userSession.BearerToken
                );
                
                // Success
                if (res == 1)
                {
                    response.Activity = "Đổi mật khẩu";
                    response.CustomMessage = "Đổi mật khẩu thành công";
                    return response;
                }
                             
                response.CustomMessage = "Sai mật khẩu, không thể đổi mật khẩu";
                response.Succeeded = false;
                return response;             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                response.CustomMessage = "Xảy ra lỗi trong quá trình đổi mật khẩu.";
                response.Succeeded = false;
                return response;
            }

        }

        #region Form
        public async Task<IActionResult> Profile()
        {
            var vm = new UserInsertAndInfoViewModel();

            vm.UserInfo = await _apiFactory.GetAsync<UserInsertViewModel>($"User/ReadById?id={_userSession.UserId}", HostConstants.ApiIdentity, _userSession.BearerToken);


            return View(vm);

        }

        #endregion Form


        public  IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        [HttpPost]
        public async Task<ResponseContainer> Update(UserInsertAndInfoViewModel model)
        {
            if (String.IsNullOrEmpty(model.UserInfo.Avatar))
            {
                model.UserInfo.Avatar = "";
            }

            if (model.UserInfo.FileImage != null)
            {
                //Delete old
                DeleteFileLocal(model.UserInfo.Avatar);

                //create file local
                model.UserInfo.Avatar = await CreateFileLocal(model.UserInfo.FileImage);
            }

            var result_user = await _apiFactory.PutAsync<UserInsertViewModel, int>(model.UserInfo, "User/Update", Endpoints.ApiIdentity, _userSession.BearerToken);

            ResponseContainer response = new ResponseContainer();
            if (_userSession.FullName != model.UserInfo.FullName)
            {
                if (model.UserInfo.Id == long.Parse(_userSession.UserId) && result_user > 0)
                {
                    UpdateClaims(model.UserInfo.Avatar, model.UserInfo.FullName);
                    response.Response = new
                    {
                        avatar = model.UserInfo.Avatar,
                        fullname = model.UserInfo.FullName
                    };
                }
            }
            response.Action = "update";
            response.Activity = "Cập nhật thông tin ";
            response.Succeeded = true;

            return response;
        }

        [HttpPost]
        public async Task<ResponseContainer> UpdateUser(UserInsertAndInfoViewModel model)
        {
            if (String.IsNullOrEmpty(model.UserInfo.Avatar))
            {
                model.UserInfo.Avatar = "";
            }

            if (model.UserInfo.FileImage != null)
            {
                //Delete old
                DeleteFileLocal(model.UserInfo.Avatar);

                //create file local
                model.UserInfo.Avatar = await CreateFileLocal(model.UserInfo.FileImage);
            }

            var result_user = await _apiFactory.PutAsync<UserInsertViewModel, int>(model.UserInfo, "User/Update", Endpoints.ApiIdentity, _userSession.BearerToken);

            ResponseContainer response = new ResponseContainer();
            if (_userSession.Avatar != model.UserInfo.Avatar)
            {
                if (model.UserInfo.Id == long.Parse(_userSession.UserId) && result_user > 0)
                {
                    UpdateClaims(model.UserInfo.Avatar, model.UserInfo.FullName);
                    response.Response = new
                    {
                        avatar = model.UserInfo.Avatar,
                        fullname = model.UserInfo.FullName
                    };

                }

                
            }
            response.Action = "update";
            response.Activity = "Cập nhật Avatar";
            response.Succeeded = true;
            return response;
        }

        public async Task<string> CreateFileLocal(IFormFile file)
        {
            var fileName = "";
            var webRoot = _hostingEnvironment.WebRootPath;
            var type = Path.GetExtension(file.FileName);
            var name = Guid.NewGuid().ToString("N") + type;
            var PathWithFolderName = Path.Combine(webRoot, "upload" + $@"\Images" + $@"\Avatar");
            if (!Directory.Exists(PathWithFolderName))
            {
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(PathWithFolderName);
            }
            var fullPath = Path.Combine(PathWithFolderName, name);
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
                fileName = name;
            }
            ViewBag.Avar = fileName;
            return fileName;

        }
        
        public void DeleteServicePointImgageLocal(string oldImageFullPath)
        {
            oldImageFullPath = oldImageFullPath.Replace(_configuration["DomainFile"] + "upload" + $@"/Images" + $@"/ServicePointInfo" + $@"/", "");
            var webRoot = _hostingEnvironment.WebRootPath;
            var PathWithFolderName = Path.Combine(webRoot, "upload" + $@"\Images" + $@"\ServicePointInfo");
            var oldPath = Path.Combine(PathWithFolderName, oldImageFullPath);
            if (!String.IsNullOrEmpty(oldPath) && System.IO.File.Exists(oldPath) && !String.IsNullOrEmpty(oldImageFullPath))
            {
                System.IO.File.Delete(oldPath);
            }

        }
        
        public async Task<string> CreateServicePointImgageLocal(IFormFile image)
        {
            var fileName = "";
            var webRoot = _hostingEnvironment.WebRootPath;
            var type = Path.GetExtension(image.FileName);
            var name = Guid.NewGuid().ToString("N") + type;
            var PathWithFolderName = Path.Combine(webRoot, "upload" + $@"\Images" + $@"\ServicePointInfo");
            if (!Directory.Exists(PathWithFolderName))
            {
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(PathWithFolderName);
            }
            var fullPath = Path.Combine(PathWithFolderName, name);
            using (var file = new FileStream(fullPath, FileMode.Create))
            {
                await image.CopyToAsync(file);
                fileName = name;
            }
            return fileName;
        }
        
        public void DeleteFileLocal(string oldImageFullPath)
        {
            if (oldImageFullPath != null)
            {
                oldImageFullPath = oldImageFullPath.Replace(_configuration["DomainFile"] + "upload/Images" + $@"/Avatar" + $@"/", "");
                var webRoot = _hostingEnvironment.WebRootPath;
                var PathWithFolderName = Path.Combine(webRoot, "upload" + $@"\Images" + $@"\Avatar");
                var oldPath = Path.Combine(PathWithFolderName, oldImageFullPath);
                if (!String.IsNullOrEmpty(oldPath) && System.IO.File.Exists(oldPath) && !String.IsNullOrEmpty(oldImageFullPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }

        }
        
        public async Task<string> CreateLogoLocal(IFormFile image)
        {
            var fileName = "";
            var webRoot = _hostingEnvironment.WebRootPath;
            var type = Path.GetExtension(image.FileName);
            var name = Guid.NewGuid().ToString("N") + type;
            var PathWithFolderName = Path.Combine(webRoot, "images" + $@"\ProviderInfo");
            if (!Directory.Exists(PathWithFolderName))
            {
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(PathWithFolderName);
            }
            var fullPath = Path.Combine(PathWithFolderName, name);
            using (var file = new FileStream(fullPath, FileMode.Create))
            {
                await image.CopyToAsync(file);
                fileName = name;
            }
            return fileName;
        }
        
        public void DeleteLogoLocal(string oldImageFullPath)
        {
            oldImageFullPath = oldImageFullPath.Replace(_configuration["DomainFile"] + "images" + $@"/ProviderInfo" + $@"/", "");
            var webRoot = _hostingEnvironment.WebRootPath;
            var PathWithFolderName = Path.Combine(webRoot, "images" + $@"\ProviderInfo");
            var oldPath = Path.Combine(PathWithFolderName, oldImageFullPath);
            if (!String.IsNullOrEmpty(oldPath) && System.IO.File.Exists(oldPath) && !String.IsNullOrEmpty(oldImageFullPath))
            {
                System.IO.File.Delete(oldPath);
            }

        }


        }
    }
