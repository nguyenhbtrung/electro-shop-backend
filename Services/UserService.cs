using electro_shop_backend.Data;
using electro_shop_backend.Models.DTOs.User;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;
        public UserService(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _context = context;
        }
        public async Task<IActionResult> RegisterAsync(RegisterDTO registerDTO)
        {
            try
            {
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
                        return new OkObjectResult(
                            new NewUserDTO
                            {
                                UserName = user.UserName,
                                Email = user.Email,
                                Token = token,
                                Roles = "User"
                            });
                    }
                    else
                    {
                        return new ObjectResult(userRole.Errors) { StatusCode = 500 };
                    }
                }
                else
                {
                    return new ObjectResult(createUser.Errors) { StatusCode = 500 };
                }
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex) { StatusCode = 500 };
            }
        }
        public async Task<IActionResult> LoginAsync (LoginDTO loginDTO)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginDTO.UserName.ToLower());
            if (user == null)
            {
                return new UnauthorizedObjectResult("User not found");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (!result.Succeeded)
            {
                return new UnauthorizedObjectResult("Password incorrect!");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            return new OkObjectResult(
                new NewUserDTO
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = await _tokenService.createToken(user),
                    Roles = role
                }
            );
        }

        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return new OkObjectResult(users);
        }

        public async Task<IActionResult> GetUserAsync(string userName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                return new UnauthorizedObjectResult("User not found");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            return new OkObjectResult(
                new NewUserDTO
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = role
                }
            );
        }
    }
}