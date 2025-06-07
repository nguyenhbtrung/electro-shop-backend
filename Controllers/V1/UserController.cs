using electro_shop_backend.Models.DTOs.User;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) => _userService = userService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _userService.RegisterAsync(registerDTO);
        }

        [HttpPost("addUser")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AddUser([FromBody] AdminAddUserDTO adminAddUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _userService.AddUserAsync(adminAddUserDTO);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _userService.LoginAsync(loginDTO);
        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllUsers()
        {
            return await _userService.GetAllUsersAsync();
        }
        [HttpGet("admin/{userName}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AdminGetUser(string userName)
        {
            return await _userService.GetUserAsync(userName);
        }
        [HttpGet("user/{userName}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetUser(string userName)
        {
            if (userName == null)
            {
                return BadRequest("User name is required.");
            }
            var authenticatedUserName = User.Identity.Name;
            if (authenticatedUserName != userName)
            {
                return Unauthorized("You can only view your own account.");
            }
            return await _userService.GetUserAsync(userName);
        }
        [HttpGet("user/me")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userName = User.Identity?.Name;

            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("User not authenticated.");
            }

            return await _userService.GetUserAsync(userName);
        }
        [HttpPut("user")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> UpdateUser(UserChangeUser userChangeUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var authenticatedUserName = User.Identity.Name;
            return await _userService.UpdateUserAsync(authenticatedUserName, userChangeUser);
        }
        [HttpPut("admin")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AdminUpdateUserAsync(UserForAdminDTO userForAdminDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _userService.AdminUpdateUserAsync(userForAdminDTO);
        }
        [HttpDelete("user")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            var authenticatedUserName = User.Identity.Name;
            if (authenticatedUserName != userName)
            {
                return Unauthorized("You can only delete your own account.");
            }
            return await _userService.DeleteUserAsync(userName);
        }
        [HttpDelete("admin")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AdminDeleteUser(string userName)
        {
            return await _userService.DeleteUserAsync(userName);
        }

        [HttpPut("changePassword")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (User.Identity.Name != changePasswordDTO.UserName)
            {
                return Unauthorized("Invalid username!");
            }
            return await _userService.ChangePasswordAsync(changePasswordDTO);
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _userService.SendForgotPasswordEmail(email);
        }

        [HttpPut("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _userService.ResetPasswordAsync(resetPasswordDTO);
        }

        [HttpPost("confirmedEmail")]
        public async Task<IActionResult> SendEmailConfirmed(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _userService.SendEmailConfirmedAsync(email);
        }

        [HttpPut("confirmedEmail")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _userService.ConfirmEmailAsync(email, token);
        }

        [HttpPut("user/avatar")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> UpdateAvatar(string url)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var authenticatedUserName = User?.Identity?.Name;
            if (authenticatedUserName == null)
            {
                return Unauthorized("Invalid username!");
            }
            return await _userService.UpdateAvatarAsync(authenticatedUserName, url);
        }
    }
}