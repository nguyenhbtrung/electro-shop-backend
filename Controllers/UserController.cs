using electro_shop_backend.Models.DTOs.User;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        public UserController(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = new User
                {
                    UserName = registerDTO.UserName,
                    Email = registerDTO.Email
                };
                var createUser = await _userManager.CreateAsync(user, registerDTO.Password);

                if (createUser.Succeeded)
                {
                    var userRole = await _userManager.AddToRoleAsync(user, "User");
                    if (userRole.Succeeded)
                    {
                        var token = await _tokenService.createToken(user);
                        var newUserDTO = new NewUserDTO
                        {
                            UserName = user.UserName,
                            Email = user.Email,
                            Token = token
                        };
                        return Ok(new { message = "Register success", user = newUserDTO });
                    }
                    else
                    {
                        return StatusCode(500, userRole.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createUser.Errors);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
