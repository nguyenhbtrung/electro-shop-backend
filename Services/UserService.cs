﻿using electro_shop_backend.Data;
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
        private readonly IEmailService _emailService;
        public UserService(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager, ApplicationDbContext context, IEmailService emailService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _context = context;
            _emailService = emailService;
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
                        newUserDTO.Token = await _tokenService.createToken(user);
                        return new OkObjectResult(newUserDTO);
                    }
                    else
                    {
                        var result = await _userManager.DeleteAsync(user);
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

        public async Task<IActionResult> AddUserAsync(AdminAddUserDTO adminAddUserDTO)
        {
            try
            {
                var user = adminAddUserDTO.ToUserFromAdminAddUserDTO();
                user.CreatedAt = DateTime.UtcNow;
                var createUser = await _userManager.CreateAsync(user, adminAddUserDTO.Password);
                if (createUser.Succeeded)
                {
                    if (adminAddUserDTO.Role == "Admin")
                    {
                        var userRole = await _userManager.AddToRoleAsync(user, adminAddUserDTO.Role);
                        if (userRole.Succeeded)
                        {
                            return new OkObjectResult("User added successfully");
                        }
                        else
                        {
                            return new ObjectResult("User created but add role error: " + userRole.Errors) { StatusCode = 500 };
                        }
                    }
                    else
                    {
                        var userRole = await _userManager.AddToRoleAsync(user, "User");
                        if (userRole.Succeeded)
                        {
                            return new OkObjectResult("User added successfully");
                        }
                        else
                        {
                            var result = await _userManager.DeleteAsync(user);
                            return new ObjectResult(userRole.Errors) { StatusCode = 500 };
                        }
                    }
                }
                else
                {
                    return new ObjectResult("User create not success: " + createUser.Errors) { StatusCode = 500 };
                }
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex) { StatusCode = 500 };
            }
        }
        public async Task<IActionResult> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginDTO.UserName.ToLower());
            if (user == null)
            {
                return new UnauthorizedObjectResult("UserName or Password is incorrect!");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (!result.Succeeded)
            {
                return new UnauthorizedObjectResult("UserName or Password is incorrect!");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            if (user.UserStatus == "Banned")
            {
                return new UnauthorizedObjectResult("User is banned");
            }
            var newUserDTO = UserMapper.ToNewUserDTOFromUser(user);
            newUserDTO.Roles = role;
            newUserDTO.Token = await _tokenService.createToken(user);

            return new OkObjectResult(newUserDTO);
        }

        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = new List<ViewUserForAdminDTO>();
            foreach (var user in users)
            {
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
            user.Email = userForAdminDTO.Email;
            user.EmailConfirmed = userForAdminDTO.EmailConfirmed;
            user.AvatarImg = userForAdminDTO.AvatarImg;
            user.UserStatus = userForAdminDTO.UserStatus;
            user.AvatarImg = userForAdminDTO.AvatarImg;
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
            try
            {
                var removePassword = await _userManager.RemovePasswordAsync(user);
                var addPassword = await _userManager.AddPasswordAsync(user, userForAdminDTO.Password);
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex) { StatusCode = 500 };
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
            var rating = await _context.Ratings.Where(r => r.UserId == user.Id).ToListAsync();
            foreach (var r in rating)
            {
                _context.Ratings.Remove(r);
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return new OkObjectResult("User deleted successfully");
            }
            return new ObjectResult(result.Errors) { StatusCode = 500 };
        }

        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == changePasswordDTO.UserName);
            if (user == null)
            {
                return new UnauthorizedObjectResult("User not found");
            }
            var result = await _userManager.ChangePasswordAsync(user, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword);
            if (result.Succeeded)
            {
                await _emailService.SendEmailAsync(user.Email, "Mật khẩu của bạn đã được cập nhật – GTG Shop",
                    $"<div style='font-family: Time New Roman; font-size: 18px; color: black;'>Xin chào {user.UserName},  " +
                    "<br><br>Mật khẩu của bạn đã được thay đổi thành công. Nếu bạn đã thực hiện thay đổi này, không cần làm gì thêm.  " +
                    "<br><br>Nếu bạn không thực hiện, vui lòng liên hệ ngay với chúng tôi tại dutshop66@gmail.com để bảo vệ tài khoản của bạn.  " +
                    "<br><br>Trân trọng,  <br>GTG Shop." +
                    "</div>");
                return new OkObjectResult("Password changed successfully");
            }
            return new ObjectResult(result.Errors) { StatusCode = 500 };
        }

        public async Task<IActionResult> SendForgotPasswordEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new UnauthorizedObjectResult("Email not found");
            }
            if (user.EmailConfirmed == false)
            {
                return new UnauthorizedObjectResult("Email not confirmed");
            }
            try
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = "http://localhost:4200/reset-password?email=" + user.Email + "&token=" + token;
                await _emailService.SendEmailAsync(user.Email, "Reset Password",
                     $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return new OkObjectResult("Email sent successfully!" + token);
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex) { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (user == null)
            {
                return new UnauthorizedObjectResult("Email not found");
            }
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.NewPassword);
            if (result.Succeeded)
            {
                return new OkObjectResult("Password reset successfully");
            }
            return new ObjectResult(result.Errors) { StatusCode = 500 };
        }

        public async Task<IActionResult> SendEmailConfirmedAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new BadRequestObjectResult("User not found!");
            }
            try
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = "http://localhost:7169/reset-password?email=" + user.Email + "&token=" + token;
                await _emailService.SendEmailAsync(user.Email, "Xác nhận email của bạn – GTG Shop",
                    $"<div style='font-family: Time New Roman; font-size: 18px; color: black;'>Xin chào {user.UserName},  " +
                    "<br><br>Cảm ơn bạn đã đăng ký tài khoản tại GTG Shop. Vui lòng xác nhận email của bạn bằng cách nhấp vào nút dưới đây:" +
                    "<br><br>Nếu bạn không thực hiện, vui lòng liên hệ ngay với chúng tôi tại dutshop66@gmail.com để bảo vệ tài khoản của bạn.  " +
                    "<br><br>Trân trọng,  <br>GTG Shop." +
                    "</div>");
                return new OkObjectResult("Confirmed email sent successfully!" + token);
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex) { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new BadRequestObjectResult("Email not found!");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return new OkObjectResult("Email confirmed successfully");
            }
            return new ObjectResult(result.Errors) { StatusCode = 500 };
        }
    }
}