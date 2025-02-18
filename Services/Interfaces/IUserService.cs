using electro_shop_backend.Models.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<IActionResult> RegisterAsync(RegisterDTO registerDTO);
        Task<IActionResult> LoginAsync(LoginDTO loginDTO);
        Task<IActionResult> GetAllUsersAsync();
        Task<IActionResult> GetUserAsync(string userName);
        //Task<IActionResult> UpdateUserAsync(UpdateUserDTO updateUserDTO);
        //Task<IActionResult> DeleteUserAsync(string userId);
        //Task<IActionResult> GetUserByIdAsync(string userId);
    }
}
