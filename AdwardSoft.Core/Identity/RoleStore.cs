using Microsoft.AspNetCore.Identity;
using AdwardSoft.DTO.Identity;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace AdwardSoft.Core.Identity
{
    public class RoleStore : IRoleStore<Role>
    {
        #region Constructor
        private readonly IRoleRepository _roleRepository;

        public RoleStore(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        #endregion

        #region Create
        public async Task<IdentityResult> CreateAsync(Role role,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            return await _roleRepository.CreateAsync(role);
        }
        #endregion

        #region Read
        #endregion

        #region Update
        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            return await _roleRepository.UpdateAsync(role);
        }
        #endregion

        #region Delete
        public async Task<IdentityResult> DeleteAsync(Role role,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            return await _roleRepository.DeleteAsync(role);
        }
        #endregion

        #region Find
        public async Task<Role> FindByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (id == null) throw new ArgumentNullException(nameof(id));

            return await _roleRepository.FindByIdAsync(Convert.ToInt32(id));
        }

        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (normalizedRoleName == null) throw new ArgumentNullException(nameof(normalizedRoleName));

            return await _roleRepository.FindByNameAsync(normalizedRoleName);
        }
        #endregion

        #region Get
        public async Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            var result = await _roleRepository.FindByIdAsync(role.Id);
            return result.NormalizedName;
        }

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            var result = await _roleRepository.FindByIdAsync(role.Id);
            return result.Name;
        }
        #endregion

        #region Set
        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
        #endregion

        public void Dispose()
        {
        }
    }
}
