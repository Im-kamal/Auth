using Microsoft.AspNetCore.Identity;

namespace Auth.Services
{
    public interface ITokenServices
    {
         Task<string>  CreateTokenAsync(IdentityUser User,UserManager<IdentityUser> userManager);
    }
}
