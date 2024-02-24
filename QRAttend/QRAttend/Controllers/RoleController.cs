using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRAttend.Dto;
using QRAttend.Models;
using QRAttend.Repositories;

namespace QRAttend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpGet]
        public async Task<IActionResult> GetRole()
        {
            var roles = await _roleService.GetAllRoles();
            return Ok(roles);
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleDTO model)
        {
            if (ModelState.IsValid)
            {
                int raw = await _roleService.AddRole(model);
                if (raw > 0)
                {
                    return Ok("Role Added");
                }
                else
                {
                    return BadRequest("Role didn't Added");
                }
            }
            else { return BadRequest("please enter Valid Model"); }
        }
    }
}
