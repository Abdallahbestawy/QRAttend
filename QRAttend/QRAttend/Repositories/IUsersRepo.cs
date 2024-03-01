using QRAttend.Dto;
using QRAttend.Models;

namespace QRAttend.Repositories
{
    public interface IUsersRepo
    {
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<UserRolesDTO>? GetUserRolesAsync(string userId);
        Task<bool> ChangeUserRoles(UserRolesDTO model);
    }
}
