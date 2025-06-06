﻿using electro_shop_backend.Models.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<IActionResult> RegisterAsync(RegisterDTO registerDTO);
        Task<IActionResult> AddUserAsync(AdminAddUserDTO adminAddUserDTO);
        Task<IActionResult> LoginAsync(LoginDTO loginDTO);
        Task<IActionResult> GetAllUsersAsync();
        Task<IActionResult> GetUserAsync(string userName);
        Task<IActionResult> UpdateUserAsync(string userName, UserChangeUser userChangeUser);
        Task<IActionResult> AdminUpdateUserAsync(UserForAdminDTO userForAdminDTO);
        Task<IActionResult> DeleteUserAsync(string userName);
        Task<IActionResult> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO);
        Task<IActionResult> SendForgotPasswordEmail(string email);
        Task<IActionResult> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
        Task<IActionResult> ConfirmEmailAsync(string email, string token);
        Task<IActionResult> SendEmailConfirmedAsync(string email);
        Task<IActionResult> UpdateAvatarAsync(string userName, string url);

    }
}
