using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRAttend.Dto;
using QRAttend.Models;
using QRAttend.Repositories;

namespace QRAttend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SectionGroupController : ControllerBase
    {
        private readonly ISectionGroupRepo _SectionGroupRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public SectionGroupController(ISectionGroupRepo sectionRepo, UserManager<ApplicationUser> userManager)
        {
            _SectionGroupRepo = sectionRepo;
            _userManager = userManager;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(SectionGroupDTO sectionGroup)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Unauthorized();
            }
            var result = await _SectionGroupRepo.CreateGroup(new SectionGroup
            {
                Name = sectionGroup.Name,
                CourseId = sectionGroup.CourseId
            });
            if (result == 0)
                return BadRequest();

            return Ok(sectionGroup);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();
            return Ok(await _SectionGroupRepo.GetAllSectionGroup());
        }
    }
}
