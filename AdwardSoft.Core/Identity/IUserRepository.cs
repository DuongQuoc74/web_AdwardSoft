using AdwardSoft.DTO.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdwardSoft.Core.Identity
{
    public interface IUserRepository
    {
        //Task<IEnumerable<ApplicationUser>> Get(Dictionary<string, dynamic> obj);
        Task<IdentityResult> CreateAsync(User user);

        Task<IdentityResult> DeleteAsync(User user);
        Task<IdentityResult> DeleteAsync(long id);

        Task<User> FindByIdAsync(long id);
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByNameAsync(string userName);
        Task<IEnumerable<UserInfo>> FindUserIsNotReferenceToMerchant(Dictionary<string, dynamic> obj);

        Task<IEnumerable<UserInfo>> GetAllAsync(Dictionary<string, dynamic> obj);

        Task<IdentityResult> UpdatePasswordAsync(User user);
        
    }
}
