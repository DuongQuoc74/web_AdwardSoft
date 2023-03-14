using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AdwardSoft.Core.Identity;
using AdwardSoft.DTO.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AdwardSoft.API.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        #region Constructor
        private readonly IUserRepository _userStoreExtend;
        private readonly IRoleRepository _roleStoreExtend;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthController(
            SignInManager<User> signInManager,
            IUserRepository userStoreExtend,
            IRoleRepository roleStoreExtend,
            IConfiguration config,
            IMapper mapper
        )
        {
            _signInManager = signInManager;
            _userStoreExtend = userStoreExtend;
            _roleStoreExtend = roleStoreExtend;
            _mapper = mapper;
            _config = config;
        }
        #endregion

        #region Login
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLogin model)
        {
            try
            {
                IActionResult response = BadRequest("Username or password not correct");

                //Check user authentication
                var signin = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.IsRememberMe, lockoutOnFailure: false);

                if (signin.Succeeded)
                {
                    //Get user info
                    var user = await _userStoreExtend.FindByNameAsync(model.UserName);

                    //Get Permissions
                    var permissions = await _roleStoreExtend.ReadByUser(user.Id);

                    //Mapping
                    var userInfo = _mapper.Map<UserInfo>(user);
                    userInfo.Permissions = permissions;

                    response = Ok(userInfo);
                }

                if (signin.RequiresTwoFactor)
                {
                    response = BadRequest();
                }
                if (signin.IsLockedOut)
                {
                    response = NotFound();
                }
                return response;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Methods
        #endregion
    }
}
