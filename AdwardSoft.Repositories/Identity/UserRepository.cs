using System.Collections.Generic;
using System.Threading.Tasks;
using AdwardSoft.DTO.Identity;
using AdwardSoft.Core.Identity;
using Microsoft.AspNetCore.Identity;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Utilities.Helper;
using System;

namespace AdwardSoft.Repositories.Identity
{
    public class UserRepository : IUserRepository
    {
        #region Constructor
        private IAdapterPattern _adapter;

        public UserRepository(IAdapterPattern adapter)
        {
            _adapter = adapter;
        }
        #endregion

        #region Create
        public async Task<IdentityResult> CreateAsync(User user)
        {
            user.LockoutEndDateUtc = DateTime.Now;
            user.LockoutEnabled = false;
            int result = await _adapter.ExecuteSingle<User>(user, DataHelper.ApiCRUD.Create);

            if (result > 0)
                return IdentityResult.Success;

            return IdentityResult.Failed(new IdentityError { Description = $"Could not insert this user {user.Email}." });
        }
        #endregion

        #region Update
        public async Task<IdentityResult> UpdatePasswordAsync(User user)
        {
            user.LockoutEndDateUtc = DateTime.Now;
            user.LockoutEnabled = false;

            int result = await _adapter.ExecuteSingle<User>(user, DataHelper.ApiCRUD.Update);

            if (result > 0)
                return IdentityResult.Success;

            return IdentityResult.Failed(new IdentityError { Description = $"Could not update password this user {user.Email}." });
        }

        #endregion

        #region Delete
        public async Task<IdentityResult> DeleteAsync(User user)
        {
            var param = DataHelper.GenParams("Id", user.Id);
            int result = await _adapter.ExecuteSingle<User, int>(param, DataHelper.ApiCRUD.Delete);
            if (result > 0)
                return IdentityResult.Success;
            return IdentityResult.Failed(new IdentityError { Description = $"Could not delete this user {user.Email}." });
        }

        public async Task<IdentityResult> DeleteAsync(long id)
        {
            var user = new User();
            user.Id = id;
            int result = await _adapter.ExecuteSingle<User>(user, DataHelper.ApiCRUD.Delete);

            if (result > 0)
                return IdentityResult.Success;
            return IdentityResult.Failed(new IdentityError { Description = $"Could not delete this user {user.Email}." });
        }
        #endregion

        #region Find
        public async Task<User> FindByIdAsync(long id)
        {
            var dict = new Dictionary<string, object> { { "Id", id } };
            var result = await _adapter.QuerySingle<User>(dict, DataHelper.ApiCRUD.ReadById);

            return result;
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            var dict = new Dictionary<string, object> { { "NormalizedUserName", userName } };
            var result = await _adapter.QuerySingle<User>(dict, DataHelper.ApiCRUD.Read, "ByNormalizedUserName");

            return result;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            var dict = new Dictionary<string, object> { { "NormalizedEmail", email } };
            var result = await _adapter.QuerySingle<User>(dict, DataHelper.ApiCRUD.Read, "ByNormalizedEmail");

            return result;
        }

        public async Task<IEnumerable<UserInfo>> FindUserIsNotReferenceToMerchant(Dictionary<string, dynamic> obj)
        {
            try
            {
                var result = await _adapter.Query<UserInfo>(obj, "usp_User_ReadUserIsNotReferenceToMerchant");
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Read
        //public async Task<IEnumerable<ApplicationUser>> Get(Dictionary<string, dynamic> obj)
        //{
        //    var result = await _adapter.ExecuteQuery<ApplicationUser>(obj, "usp_User_Read");
        //    return result;
        //}

        public async Task<IEnumerable<UserInfo>> GetAllAsync(Dictionary<string, dynamic> obj)
        {
            try
            {
                var result = await _adapter.Query<UserInfo>(obj, "usp_User_Read");
                return result;
            }
            catch (System.Exception ex)
            {

                throw;
            }

        }
        #endregion
    }
}
