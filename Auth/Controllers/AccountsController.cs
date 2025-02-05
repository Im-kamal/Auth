using Auth.DTOs;
using Auth.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITokenServices _tokenServices;

        public AccountsController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
            ,ITokenServices tokenServices)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._tokenServices = tokenServices;
        }

        //Register 
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var User = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
            };
            var Result = await _userManager.CreateAsync(User, model.Password);

            if (!Result.Succeeded)
                return BadRequest();
            var ReturnedUser = new UserDto
            {
                UserName = User.UserName,
                Email = User.Email,
                Token =await _tokenServices.CreateTokenAsync(User , _userManager)
            };

            return Ok(ReturnedUser);
        }
        //Login

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User is null)
                return Unauthorized();
            var Result = await _signInManager.CheckPasswordSignInAsync(User, model.Password, false);

            if (!Result.Succeeded)
                return Unauthorized();
            return Ok(new UserDto()
            {
                UserName = User.UserName,
                Email = User.Email,
                Token =await _tokenServices.CreateTokenAsync(User , _userManager)

            });





        }

    }
}
