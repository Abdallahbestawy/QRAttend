using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QRAttend.Dto;
using QRAttend.Models;
using QRAttend.Repositories;

namespace QRAttend.Services
{
    public class UsersRepo : IUsersRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersRepo(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> ChangeUserRoles(UserRolesDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
                return false;

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                if (userRoles.Any(r => r == role.Name) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.Name);

                if (!userRoles.Any(r => r == role.Name) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.Name);
            }
            return true;
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            List<ApplicationUser> users = await _userManager.Users.ToListAsync();
            List<UserDTO> result = new List<UserDTO>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new UserDTO
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Roles = roles.ToList()
                });
            }
            return result;
        }

        public async Task<UserRolesDTO>? GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            var roles = await _roleManager.Roles.ToListAsync();

            var result = new UserRolesDTO
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new RoleDTO
                {
                    Id = role.Id,
                    Name = role.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList()
            };
            return result;
        }
    }
}
