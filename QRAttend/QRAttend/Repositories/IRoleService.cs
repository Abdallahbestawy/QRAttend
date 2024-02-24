using QRAttend.Dto;

namespace QRAttend.Repositories
{
    public interface IRoleService
    {
        Task<int> AddRole(RoleDTO model);
        Task<ICollection<RoleDTO>> GetAllRoles();
    }
}
