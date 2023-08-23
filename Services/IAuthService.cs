using Microsoft.AspNetCore.Identity;
using Sq016FirstApi.Data.Entities;
using System.Security.Claims;

namespace Sq016FirstApi.Services
{
    public interface IAuthService
    {
        string GenerateJWT(AppUser user, IList<string> roles, IList<Claim> userclaims);
        Task<SignInResult> Login(AppUser user, string password);
    }
}
