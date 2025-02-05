using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Services
{
    public class TokenServices : ITokenServices
    {
        private readonly IConfiguration configuration;

        public TokenServices(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(IdentityUser User , UserManager<IdentityUser> userManager)
        {
            var AuthClaims = new List<Claim>()
            { 
                new Claim (ClaimTypes.GivenName , User.UserName),
                new Claim (ClaimTypes.Email , User.Email),
            };
            var UserRoles = await userManager.GetRolesAsync (User);
            foreach (var Role in UserRoles)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role, Role));
            }

            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));

            var Token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(double.Parse( configuration["JWT:DurationInDays"])) ,
                claims : AuthClaims,
                signingCredentials: new SigningCredentials(AuthKey , SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(Token);
            
        }
    }
}
