using Microsoft.AspNetCore.Identity;

namespace Auth.Data.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<IdentityUser> userManager)
        {

            if (!userManager.Users.Any()) {
                var User = new IdentityUser
                {
                    UserName = "Kamal",
                    Email = "Kamal@gmail.com",
                    PhoneNumber = "01156053262"
                };

                await userManager.CreateAsync(User, "Pa$$w0rd");
            }
          
        }
    }
}
