using electro_shop_backend.Data;
using electro_shop_backend.Models.DTOs.User;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
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
                    Email = registerDTO.Email,
                    CreatedAt = DateTime.UtcNow,
                    UserStatus = "Active"
                };
                var createUser = await _userManager.CreateAsync(user, registerDTO.Password);

                if (createUser.Succeeded)
                {
                    var userRole = await _userManager.AddToRoleAsync(user, "User");
                    if (userRole.Succeeded)
                    {
                        var token = await _tokenService.createToken(user);
                        var roles = await _userManager.GetRolesAsync(user);
                        var role = roles.FirstOrDefault();

                        var newUserDTO = UserMapper.ToNewUserDTOFromUser(user);
                        newUserDTO.Roles = role;
                        newUserDTO.Token = await _tokenService.createToken(user),

                        return new OkObjectResult(newUserDTO);
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
            
            if(user.UserStatus == "Banned")
            {
                return new UnauthorizedObjectResult("User is banned");
            }
            var newUserDTO = UserMapper.ToNewUserDTOFromUser(user);
            newUserDTO.Roles = role;
            newUserDTO.Token = await _tokenService.createToken(user),

            return new OkObjectResult(newUserDTO);
        }
        
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = new List<ViewUserForAdminDTO>();
            foreach (var user in users) {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault();
                var userForAdminDTO = UserMapper.ToViewUserForAdminDTOFromUser(user);
                userForAdminDTO.Roles = role;
                userDtos.Add(userForAdminDTO);
            }
            return new OkObjectResult(userDtos);
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
            var userForAdminDTO = UserMapper.ToViewUserForAdminDTOFromUser(user);
            userForAdminDTO.Roles = role;
            return new OkObjectResult(userForAdminDTO);
        }

        public async Task<IActionResult> UpdateUserAsync(UserForAdminDTO userForAdminDTO)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userForAdminDTO.UserName);
            if (user == null)
            {
                return new UnauthorizedObjectResult("User not found");
            }
            user.FullName = userForAdminDTO.FullName;
            user.Address = userForAdminDTO.Address;
            user.PhoneNumber = userForAdminDTO.PhoneNumber;
            user.AvatarImg = userForAdminDTO.AvatarImg;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new OkObjectResult("User updated successfully");
            }
            return new ObjectResult(result.Errors) { StatusCode = 500 };
        }

        public async Task<IActionResult> AdminUpdateUserAsync(UserForAdminDTO userForAdminDTO)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userForAdminDTO.UserName);
            if (user == null)
            {
                return new UnauthorizedObjectResult("User not found");
            }
            user.FullName = userForAdminDTO.FullName;
            user.Address = userForAdminDTO.Address;
            user.PhoneNumber = userForAdminDTO.PhoneNumber;
            user.AvatarImg = userForAdminDTO.AvatarImg;
            user.UserStatus = userForAdminDTO.UserStatus;
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            if (role != userForAdminDTO.Roles)
            {
                var removeRole = await _userManager.RemoveFromRoleAsync(user, role);
                if (!removeRole.Succeeded)
                {
                    return new ObjectResult(removeRole.Errors) { StatusCode = 500 };
                }
                var addRole = await _userManager.AddToRoleAsync(user, userForAdminDTO.Roles);
                if (!addRole.Succeeded)
                {
                    return new ObjectResult(addRole.Errors) { StatusCode = 500 };
                }
            }
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new OkObjectResult("User updated successfully");
            }
            return new ObjectResult(result.Errors) { StatusCode = 500 };
        }


        public async Task<IActionResult> DeleteUserAsync(string userName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                return new UnauthorizedObjectResult("User not found");
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return new OkObjectResult("User deleted successfully");
            }
            return new ObjectResult(result.Errors) { StatusCode = 500 };
        }
    }
}