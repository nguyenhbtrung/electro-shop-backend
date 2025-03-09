using electro_shop_backend.Models.DTOs.User;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;

        }

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
            return await _userService.AddUser(adminAddUserDTO);
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
        [HttpGet("{userName}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetUser(string userName)
        {
            return await _userService.GetUserAsync(userName);
        }

        [HttpPut("user")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> UpdateUser(UserForAdminDTO userForAdminDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _userService.UpdateUserAsync(userForAdminDTO);
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
    }
}
