using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QRAttend.Dto;
using QRAttend.Repositories;

namespace QRAttend.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<int> AddRole(RoleDTO model)
        {
            IdentityRole role = new IdentityRole();
            role.Name = model.Name;
            IdentityResult result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public async Task<ICollection<RoleDTO>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles.Select(role => new RoleDTO
            {
                Id = role.Id,
                Name = role.Name
            }).ToList();
        }
    }
}
