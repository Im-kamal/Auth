using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> signInManager;

    public DashboardController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        this.signInManager = signInManager;
    }

    // Add User
    [HttpPost("AddUser")]
    public async Task<IActionResult> AddUser(CreateUserDto model)
    {
        var user = new IdentityUser
        {
            UserName = model.UserName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new { Errors = errors });
        }

        return Ok(new { Message = "User created successfully" });
    }

    // Edit User
    [HttpPut("EditUser/{id}")]
    public async Task<IActionResult> EditUser(string id, UpdateUserDto model)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound(new { Error = "User not found" });

        user.UserName = model.UserName;
        user.Email = model.Email;
        user.PhoneNumber = model.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new { Errors = errors });
        }

        return Ok(new { Message = "User updated successfully" });
    }

    // Delete User
    [HttpDelete("DeleteUser/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound(new { Error = "User not found" });

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new { Errors = errors });
        }

        return Ok(new { Message = "User deleted successfully" });
    }

    // View User
    [HttpGet("ViewUser/{id}")]
    public async Task<IActionResult> ViewUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound(new { Error = "User not found" });

        var userDto = new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };

        return Ok(userDto);
    }

    // View All Users
    [HttpGet("ViewAllUsers")]
    public async Task<IActionResult> ViewAllUsers()
    {
        var users = _userManager.Users.ToList();
        var userDtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber
        }).ToList();

        return Ok(userDtos);
    }

    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto model, [FromServices] IEmailService emailService)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return NotFound(new { Error = "User not found" });

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var resetLink = $"https://yourwebsite.com/reset-password?email={model.Email}&token={token}";
        var message = $"Please reset your password by clicking here: {resetLink}";

        await emailService.SendEmailAsync(model.Email, "Reset Password", message);

        return Ok(new { Message = "Password reset link has been sent to your email" });
    }



    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class CreateUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class UpdateUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
    public class ForgotPasswordDto
    {
        public string Email { get; set; }
    }
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

    public class SendGridEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public SendGridEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var apiKey = _configuration["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(_configuration["SendGrid:FromEmail"], _configuration["SendGrid:FromName"]);
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            await client.SendEmailAsync(msg);
        }


    }
}