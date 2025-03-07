using electro_shop_backend.Models.DTOs.User;
using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Models.Mappers
{
    public static class UserMapper
    {
        public static ViewUserForAdminDTO ToViewUserForAdminDTOFromUser (this User user)
        {
            return new ViewUserForAdminDTO ()
            {
                UserName = user.UserName,
                FullName = user.FullName,
                Address = user.Address,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                AvatarImg = user.AvatarImg,
                UserStatus = user.UserStatus,
                CreatedAt = user.CreatedAt
            };      
        }

        public static NewUserDTO ToNewUserDTOFromUser (this User user)
        {
            return new NewUserDTO()
            {
                UserName = user.UserName,
                Email = user.Email
            };
        }

        public static User ToUserFromAdminAddUserDTO (this AdminAddUserDTO adminAddUserDTO)
        {
            return new User()
            {
                UserName = adminAddUserDTO.UserName,
                Email = adminAddUserDTO.Email,
                PhoneNumber = adminAddUserDTO.PhoneNumber,
                FullName = adminAddUserDTO.FullName,
                Address = adminAddUserDTO.Address,
                AvatarImg = adminAddUserDTO.AvatarImg,
                UserStatus = adminAddUserDTO.UserStatus
            };
        }
    }
}
