using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Provider.Models;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Security.Claims;
using AdwardSoft.Utilities.Mailer;
using AdwardSoft.Utilities.Mailer.DTO;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Controllers
{
    //[AdsAuthorization]
    public class UserController : Controller
    {
        #region contructor
        //private readonly UserManager<ApplicationUser> _userManager;
        private IConfiguration Configuration { get; }
        private IAPIFactory _apiFactory;
        private IUserSession _userSession;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<ClientAuthSetting> _clientSetting;
        private int _BranchId;


        public UserController
        (
            // UserManager<ApplicationUser> userManager,
            IAPIFactory apiFactory,
            IConfiguration configuration,
            IUserSession userSession,
            IOptions<ClientAuthSetting> clientSetting,
            IHttpContextAccessor httpContextAccessor
        )
        {
            //_userManager = userManager;
            _apiFactory = apiFactory;
            Configuration = configuration;
            _userSession = userSession;
            _clientSetting = clientSetting;
            _httpContextAccessor = httpContextAccessor;

            var branchId = ClaimsPrincipalIdentity.GetClaim("branch_id", _httpContextAccessor.HttpContext);

            Int32.TryParse(branchId, out _BranchId);
        }
        #endregion

        #region View
        public IActionResult Index() => View();

        public IActionResult OTP() => View();

        #endregion

        #region Form
        [HttpGet]
        public async Task<IActionResult> _UserForm(long id, short type)
        {
            try
            {
                var model = new UserInsertViewModel();
                model.Type = type;
                var RoleConfig = await _apiFactory.GetAsync<RoleConfigViewModel>("RoleConfig/ReadByDefaultType?userType=" + type, HostConstants.ApiIdentity, _userSession.BearerToken);
                ViewBag.RoleConfig = String.IsNullOrEmpty(RoleConfig.PwdRegex) ? "" : RoleConfig.PwdRegex;
                if (id > 0)
                {
                    model = await _apiFactory.GetAsync<UserInsertViewModel>("User/ReadById?id=" + id, HostConstants.ApiIdentity, _userSession.BearerToken);
                }

                return PartialView(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> _ChangePasswordForm(long id)
        {
            try
            {
                var model = new UserViewModel();
                var user = await _apiFactory.GetAsync<UserInsertViewModel>("User/ReadById?Id=" + id, HostConstants.ApiIdentity, _userSession.BearerToken);
                model.Id = id;
                model.FullName = user.FullName;
                model.Email = user.Email;
                model.Username = user.UserName;
                model.Password = user.PasswordHash;
                model.Phone = user.PhoneNumber;
                model.OldPassword = "";
                model.NewPassword = "";
                model.RepeatPassword = "";
                model.Gender = user.Gender;
                var RoleConfig = await _apiFactory.GetAsync<RoleConfigViewModel>("RoleConfig/ReadByDefaultType?userType=" + user.Type, HostConstants.ApiIdentity, _userSession.BearerToken);
                ViewBag.RoleConfig = String.IsNullOrEmpty(RoleConfig.PwdRegex) ? "" : RoleConfig.PwdRegex;
                return PartialView(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult _UserBranchForm()
        {
            try
            {
                var model = new UserBranchViewModel();
                model.BranchId = _BranchId;
              

                return PartialView(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Read
        [HttpGet]
        public async Task<List<UserInfoViewModel>> Read(int type, int status)
        {
            var res = await _apiFactory.GetAsync<List<UserInfoViewModel>>("User/Read?type=" + type + "&status=" + status, HostConstants.ApiIdentity, _userSession.BearerToken);
            return res;
        }
        #endregion

        #region Create and Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ResponseContainer> Create(UserInsertViewModel model)
        {
            try
            {
                ResponseContainer response = new ResponseContainer();
                response.Activity = "Thêm mới";
                response.Action = "Thêm mới";
                if (model.FileImage != null)
                {
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload", "Images", "Avatar", model.FileImage.FileName);
                    using (var file = new FileStream(fullPath, FileMode.Create))
                    {
                        await model.FileImage.CopyToAsync(file);
                        model.Avatar = model.FileImage.FileName;
                    }
                }
                else
                {
                    model.Avatar = "user.png";
                }    
               
                //string randomPassword = "@" + RandomString(8);
                //model.PasswordHash = randomPassword;
                var result = await _apiFactory.PostAsync<UserInsertViewModel, int>(model, "User/Create", HostConstants.ApiIdentity, _userSession.BearerToken);
                if (result == 0)
                {
                    response.Succeeded = false;
                }
                else
                {
                    var lst = new List<UserBranchViewModel>();
                    if (model.Branchs != null)
                    {
                        for (int i = 0; i < model.Branchs.Length; i++)
                        {
                            var item = new UserBranchViewModel()
                            {
                                BranchId = int.Parse(model.Branchs[i]),
                                UserId = result
                            };
                            lst.Add(item);
                        };
                        response.Succeeded = await _apiFactory.PostAsync<List<UserBranchViewModel>, bool>(lst, "UserBranch/Create", Endpoints.ApiPOS, _userSession.BearerToken);
                    }

                    // Send Email
                    //await SendMail(model.Email, randomPassword, model.FullName);
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ResponseContainer> Update(UserInsertViewModel model)
        {
            try
            {
                ResponseContainer response = new ResponseContainer();
                response.Activity = "Update";
                response.Action = "update";

                if (model.FileImage != null)
                {
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload", "Images", "Avatar", model.FileImage.FileName);
                    using (var file = new FileStream(fullPath, FileMode.Create))
                    {
                        await model.FileImage.CopyToAsync(file);
                        model.Avatar = model.FileImage.FileName;
                    }
                }
                var res = await _apiFactory.PutAsync<UserInsertViewModel, int>(model, "User/Update", HostConstants.ApiIdentity, _userSession.BearerToken);
                //Update Claims 
                if (model.Id == long.Parse(_userSession.UserId) && res > 0)
                {
                    UpdateClaims(model.Avatar, model.FullName);
                    response.Response = new
                    {
                        avatar = model.Avatar,
                        fullname = model.FullName
                    };
                }

                var lst = new List<UserBranchViewModel>();
                if (model.Branchs != null)
                {
                    for (int i = 0; i < model.Branchs.Length; i++)
                    {
                        var item = new UserBranchViewModel()
                        {
                            BranchId = int.Parse(model.Branchs[i]),
                            UserId = model.Id
                        };
                        lst.Add(item);
                    };
                    response.Succeeded = await _apiFactory.PostAsync<List<UserBranchViewModel>, bool>(lst, "UserBranch/Create", Endpoints.ApiPOS, _userSession.BearerToken);
                }
                else
                {
                    response.Succeeded = await _apiFactory.DeleteAsync<bool>("UserBranch/Delete?id=" + model.Id, Endpoints.ApiPOS, _userSession.BearerToken);
                }

                if (res < 1) response.Succeeded = false;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ResponseContainer> UpdatePassword(UserViewModel model)
        {
            try
            {
                ResponseContainer response = new ResponseContainer();
                var res = await _apiFactory.PutAsync<UserViewModel, int>(model, "User/UpdatePassword", HostConstants.ApiIdentity, _userSession.BearerToken);
                if (res > 0)
                {
                    response.Activity = "Change password";
                    return response;
                }
                else
                {
                    response.Activity = "Wrong password, Change password";
                    response.Succeeded = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]

        public async Task<ResponseContainer> Delete(int id)
        {
            ResponseContainer response = new ResponseContainer();
            response.Action = "delete";
            var result = await _apiFactory.DeleteAsync<int>("User/Delete?Id=" + id, HostConstants.ApiIdentity, _userSession.BearerToken);
            if (result > 0)
            {
                response.Activity = "Delete";
            }
            else
            {
                response.Activity = "Delete";
                response.Succeeded = false;
            }
            return response;

        }
        #endregion

        #region Function
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

        public async Task<IActionResult> IsAlreadySigned(string Email)
        {
            var RegisterUsers = await _apiFactory.GetAsync<UserInsertViewModel>("User/ReadByEmail?email=" + Email, HostConstants.ApiIdentity, _userSession.BearerToken);
            if (RegisterUsers.Email != null)
                return Json(false);
            else
                return Json(true);
        }

        public async Task<IActionResult> IsAlreadySignedName(string PhoneNumber)
        {
            var RegisterUsers = await _apiFactory.GetAsync<UserInsertViewModel>("User/ReadByName?name=" + PhoneNumber, HostConstants.ApiIdentity, _userSession.BearerToken);
            if (RegisterUsers.Email != null)
                return Json(false);
            else
                return Json(true);
        }
        [HttpGet]
        public async Task<IActionResult> IsOTPVerification(string OTP)
        {
            var result = await _apiFactory.GetAsync<bool>("User/OTPVerification?phoneNumber=" + _userSession.UserName + "&OTP=" + OTP, HostConstants.ApiIdentity, _userSession.BearerToken);
            if (result)
            {
                ClaimsPrincipalIdentity.UpdateClaims(new Claim(ClaimTypesConstants.VerificationOTP, "1", ClaimValueTypes.String), HttpContext);
            }         
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> SendNewOTP(string phoneNumber)
        {
            var result = await _apiFactory.GetAsync<bool>("SMSGateway/SendOTP?phoneNumber=" + phoneNumber + "&isWeb=true", Endpoints.ApiPOS);
            return Json(result);
        }


        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private async Task SendMail(string email, string pass, string name)
        {
            var mailServer = await _apiFactory.GetAsync<MailServerDetailViewModel>("MailServer/ReadByAction?action=1&type=0", Endpoints.ApiCore, _userSession.BearerToken);

            var mailSetting = new MailSettingDTO
            {
                Id = 1,
                Email = mailServer.Email,
                Host = mailServer.SMTP,
                IsSSL = mailServer.IsSSL,
                Port = mailServer.Port,
                Pwd = mailServer.Pwd,
                Name = mailServer.Name
            };

            // Change Content
            mailServer.Content = mailServer.Content.Replace("{email}", email);
            mailServer.Content = mailServer.Content.Replace("{password}", pass);

            var mailContent = new MailDTO
            {
                Email = email,
                Body = mailServer.Content,
                Name = name,
                Subject = mailServer.Title
            };

            new Mailer().Send(mailSetting, mailContent);
        }

        public async Task<JsonResult> GetListSelectBrand(int userId)
        {
            var model = new MultiSelectLevels();
            var listSelect = await _apiFactory.GetAsync<List<SelectLevels>>("Branch/ReadSelect", Endpoints.ApiPOS, _userSession.BearerToken);

            if (listSelect != null && listSelect.Count > 0)
            {
                model.ListSelect = listSelect;
            }
            else
            {
                model.ListSelect = new List<SelectLevels>();
            }

            var listValue = await _apiFactory.GetAsync<List<UserBranchViewModel>>("UserBranch/ReadByUserId?id=" + userId, Endpoints.ApiPOS, _userSession.BearerToken);

            if (listValue != null && listValue.Count > 0)
            {
                model.ListValue = listValue.Select(c => new SelectValue
                {
                    Value = c.BranchId.ToString()
                }).ToList();
            }
            else
            {
                model.ListValue = new List<SelectValue>();
            }

            return Json(model);
        }
        #endregion

        #region Login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["FlashMessage"] = "Vui lòng nhập tên tài khoản và mật khẩu";
                return View(model);
            }
            try
            {
                var client = new HttpClient();
                var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
                {
                    Address = _clientSetting.Value.AuthServer,
                    Policy = { RequireHttps = false }
                });
                if (disco.IsError)
                {
                    Console.WriteLine(disco.Error);
                    Log.Error("Get discovery error: " + disco.Error);
                    return View(model);
                }
                var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = _clientSetting.Value.ClientName,
                    ClientSecret = _clientSetting.Value.ClientSecret,
                    Scope = _clientSetting.Value.Scope
                });

                if (tokenResponse.IsError)
                {
                    Console.WriteLine(tokenResponse.Error);
                    Log.Error("Request token error: " + tokenResponse.Error);
                    return View(model);
                }
                var loginInfo = await _apiFactory.PostAsync<UserLoginViewModel, UserInfoViewModel>(model, "Auth/Login", HostConstants.ApiAuth);
                //Update Claims
                var roleChecked = await _apiFactory.GetAsync<List<RoleViewModel>>("Role/ReadByUserId?id=" + loginInfo.Id, HostConstants.ApiIdentity);

                var role = roleChecked.Where(x => x.IsDefault == true).FirstOrDefault();

                if (role == null || role.Id == 0)
                {
                    role = roleChecked.FirstOrDefault();
                }

                string OTP = "-1";

                //if (role != null)
                //{
                //    if (role.IsOTPVerification == true)
                //    {
                //         //OTP = "0";
                //         //await SendNewOTP(loginInfo.UserName);
                //    }
                //}

                // => Get current Branch Id
                var listBranch = await _apiFactory.GetAsync<List<SelectLevels>>("Branch/ReadSelectUser?userId=" + loginInfo.Id, Endpoints.ApiPOS);

                string branch = "0";
                if (listBranch != null && listBranch.Count > 0)
                {
                    branch = listBranch.FirstOrDefault().Id.ToString();
                }

                //Get distributor id
                var custId = 0;
                var cust = await _apiFactory.GetAsync<CustomerViewModel>("Customer/ReadByUserId?userId=" + loginInfo.Id, Endpoints.ApiPOS);
                if (cust != null) custId = cust.Id;


                Log.Information(tokenResponse.AccessToken);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, loginInfo.Id.ToString(), ClaimValueTypes.Integer64),
                    new Claim(ClaimTypes.Name, loginInfo.UserName, ClaimValueTypes.String),
                    new Claim(ClaimTypes.Email, loginInfo.Email, ClaimValueTypes.String),
                    new Claim(ClaimTypes.Surname, loginInfo.FullName, ClaimValueTypes.String),
                    new Claim(ClaimTypesConstants.Access_Token, tokenResponse.AccessToken, ClaimValueTypes.String),
                    new Claim(ClaimTypesConstants.Permissions, loginInfo.Permissions, ClaimValueTypes.String),
                    new Claim(ClaimTypesConstants.Avatar, (loginInfo.Avatar != null ? loginInfo.Avatar : string.Empty), ClaimValueTypes.String),
                    new Claim(ClaimTypesConstants.VerificationOTP, OTP, ClaimValueTypes.String),
                    new Claim("customerId", custId.ToString(), ClaimValueTypes.Integer32),
                    new Claim("branch_id", branch, ClaimValueTypes.Integer32)
                };

                //Update ClaimsPrincipal
                ClaimsPrincipalIdentity.AddClaims(claims, HttpContext);                           

                //return RedirectToAction("Profile", "Home",new { loginInfo });
                return RedirectToAction("index", "Home");
            }
            catch (Exception ex)
            {
                TempData["FlashMessage"] = ex.Message;
                Log.Error(ex.Message);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            return RedirectToAction("Login", "User");
        }

        [HttpPut]
        public ResponseContainer UpdateBranch(UserBranchViewModel model)
        {
            var claim = new Claim("branch_id", model.BranchId.ToString(), ClaimValueTypes.Integer32);
            ClaimsPrincipalIdentity.UpdateClaims(claim, HttpContext);

            var response = new ResponseContainer
            {
                Activity = "Update",
                Action = "Change branch",
                Succeeded = true
            };

            return response;
        }
        #endregion
    }
}