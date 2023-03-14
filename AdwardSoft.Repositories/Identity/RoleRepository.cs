using System;
using System.Threading.Tasks;
using AdwardSoft.DTO.Identity;
using AdwardSoft.Core.Identity;
using Microsoft.AspNetCore.Identity;
using AdwardSoft.Utilities.Helper;
using AdwardSoft.ORM.Dapper;

namespace AdwardSoft.Repositories.Identity
{
    public class RoleRepository: IRoleRepository
    {
        #region Constructor
        private IAdapterPattern _adapter;

        public RoleRepository(IAdapterPattern adapter)
        {
            _adapter = adapter;
        }
        #endregion

        #region Create
        public async Task<IdentityResult> CreateAsync(Role role)
        {
            var result = await _adapter.ExecuteSingle<Role>(role, DataHelper.ApiCRUD.Create);
            if (result > 0)
                return IdentityResult.Success;
            else
                return IdentityResult.Failed(new IdentityError { Description = $"Could not create item " + role.Name });
        }
        #endregion

        #region Update
        public async Task<IdentityResult> UpdateAsync(Role role)
        {
            var result = await _adapter.ExecuteSingle<Role>(role, DataHelper.ApiCRUD.Update);
            if (result > 0)
                return IdentityResult.Success;
            return IdentityResult.Failed(new IdentityError { Description = $"Could not update item " + role.Name });
        }
        #endregion

        #region Delete
        public async Task<IdentityResult> DeleteAsync(Role role)
        {
            var result = await _adapter.ExecuteSingle<Role>(role, DataHelper.ApiCRUD.Delete);
            if (result > 0)
                return IdentityResult.Success;
            return IdentityResult.Failed(new IdentityError { Description = $"Could not delete item " + role.Name });
        }
        #endregion

        #region Find
        public async Task<Role> FindByIdAsync(int id)
        {
            var result = await _adapter.QuerySingle<Role>(DataHelper.GenParams("Id", id), DataHelper.ApiCRUD.ReadById);
            return result;
        }

        public async Task<Role> FindByNameAsync(string name)
        {
            var result = await _adapter.QuerySingle<Role>(DataHelper.GenParams("Name", name), DataHelper.ApiCRUD.Read, "ByName");
            return result;
        }
        #endregion

        #region Read
        /// <summary>
        /// Get Permission by UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> ReadByUser(long userId)
        {
            try
            {
                var result = await _adapter.QuerySingle<Role, string>(DataHelper.GenParams("UserId", userId), DataHelper.ApiCRUD.Read, "ByUser");
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        #endregion
    }
}
