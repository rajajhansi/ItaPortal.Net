using ItaPortal.Net.Models.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;

namespace ItaPortal.Net.Data
{
    public interface IAuthRepository
    {
        Task<IdentityResult> RegisterUser(UserModel userModel);
        Task<ApplicationUser> FindUser(string userName);
    }
}