using AdwardSoft.DTO.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace AdwardSoft.Core.Identity
{
    public interface IRoleRepository
    {
        Task<IdentityResult> CreateAsync(Role role);

        Task<IdentityResult> UpdateAsync(Role role);

        Task<IdentityResult> DeleteAsync(Role role);

        Task<Role> FindByIdAsync(int roleId);

        Task<Role> FindByNameAsync(string roleName);

        Task<string> ReadByUser(long userId);
    }
}
