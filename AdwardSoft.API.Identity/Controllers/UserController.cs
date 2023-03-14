using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Identity;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Identity;
using AdwardSoft.DTO.Presentation.Core;
using AdwardSoft.Utilities.Helper;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.Identity.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]

    public class UserController : ControllerBase
    {
        #region Contructor
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userStoreExtend;
        private readonly IRoleRepository _roleStoreExtend;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IGenericRepository _generic;
        private readonly IRedisRepositoty _redis;

        private const string OTP_REDIS_KEY = "ads:otpredis:id:{id}:phonenumber:{phonenumber}:otp:{otp}";

        public UserController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUserRepository userStoreExtend,
            IRoleRepository roleStoreExtend,
            IMapper mapper,
            IGenericRepository generic,
            IRedisRepositoty redis
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userStoreExtend = userStoreExtend;
            _roleStoreExtend = roleStoreExtend;
            _mapper = mapper;
            _generic = generic;
            _redis = redis;
        }
        #endregion

        #region Read
        [HttpGet("Read")]
        public async Task<IEnumerable<UserInfo>> Read(short type, short status)
        {
            var param = DataHelper.GenParams("Type", type, "Status", status);
            return await _userStoreExtend.GetAllAsync(param);
        }

        [HttpGet("ReadById")]
        public async Task<User> ReadById(long id)
        {
            var result = await _userStoreExtend.FindByIdAsync(id);
            return result;
        }

        [HttpGet("ReadByEmail")]
        public async Task<User> ReadByEmail(string email)
        {
            var result = await _userStoreExtend.FindByEmailAsync(email);
            return result;
        }

        [HttpGet("ReadByName")]
        public async Task<User> ReadByPhone(string name)
        {
            var result = await _userStoreExtend.FindByNameAsync(name);
            return result;
        }

        [HttpGet("ReadUserIsNotReferenceToMerchant")]
        public async Task<IEnumerable<UserInfo>> ReadUserIsNotReferenceToMerchant(int type, int status)
        {
            var param = DataHelper.GenParams("Type", type, "Status", status);
            var result = await _userStoreExtend.FindUserIsNotReferenceToMerchant(param);
            return result;
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadSelect()
        {
            //usp_Select_ReadEmployeeUser
            var result = await _generic.ReadCustomAsync<Select>(null, "EmployeeUser");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion

        #region Create
        // POST: api/User
        [HttpPost("Create")]
        public async Task<long> Create(User obj)
        {
            try
            {
                obj.UserName = obj.PhoneNumber;
                obj.LockoutEndDateUtc = DateTime.Now;
                var res1 = await _userManager.CreateAsync(obj, obj.PasswordHash);
                // Find by Name
                var user = await _userStoreExtend.FindByNameAsync(obj.PhoneNumber);
               
                if (res1.Succeeded)
                    return user.Id;
                return 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Update
        [HttpPut("Update")]
        public async Task<int> Update(User obj)
        {

            var result = await _userManager.UpdateAsync(obj);
            if (result.Succeeded) return 1;
            else return 0;
        }

        [HttpPut("UpdatePassword")]
        public async Task<int> UpdatePassword(UserChangePassword model)
        {

            var currentUser = await _userStoreExtend.FindByIdAsync(model.Id);

            if (currentUser != null)
            {
                var result = await _userManager.ChangePasswordAsync(
                    currentUser, 
                    model.OldPassword, 
                    model.NewPassword
                );
                
                if (result.Succeeded)
                {
                    return 1;
                };
            };

            return 0;
        }
        #endregion

        #region Delete
        [HttpDelete("Delete")]
        public async Task<int> Delete(long id)
        {
            var user = await _userStoreExtend.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded) return 1;
            else return 0;
        }
        #endregion

        #region Methods

        private static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        #endregion
    }
}
