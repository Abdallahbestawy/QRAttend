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
    [Authorize(Roles = "Admin,AssistantTeacher")]
    public class SectionController : ControllerBase
    {
        private readonly ISectionRepo _sectionRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISectionAttendanceRepo _sectionAttendanceRepo;

        public SectionController(ISectionRepo sectionRepo, UserManager<ApplicationUser> userManager, ISectionAttendanceRepo sectionAttendanceRepo)
        {
            _sectionRepo = sectionRepo;
            _userManager = userManager;
            _sectionAttendanceRepo = sectionAttendanceRepo;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(SectionDto section)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Unauthorized();
            }
            var result = _sectionRepo.CreateSection(new Section
            {
                AssistantTeacherId = currentUser.Id,
                SectionGroupId = section.SectionGroupId,
                Title = section.Title,
                Date = DateTime.UtcNow
            });
            if (result == 0)
                return BadRequest();

            return Ok(section);
        }

        //[HttpGet("GetAll/{courseId?}")]
        //public async Task<IActionResult> GetAll(int? courseId)
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    if(currentUser == null)
        //        return Unauthorized();
        //    var UserRoles = await _userManager.GetRolesAsync(currentUser);
        //    if(UserRoles.Contains("Admin"))
        //        return Ok(_sectionRepo.GetAllSections());
        //    else
        //    {
        //        if(courseId == null)
        //            return BadRequest("CourseId Is Required");
        //        return Ok(_sectionRepo.GetSectionsByAssistantTeacherId(currentUser.Id,courseId));
        //    }
        //}

        [HttpGet("GetAll/{groupId}")]
        [Authorize(Roles = "AssistantTeacher")]
        public async Task<IActionResult> GetAll(int groupId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();
            
            return Ok(_sectionRepo.GetSectionsByGroupId(groupId));
        }

        [HttpGet("GetStudents/{sectionId}")]
        public async Task<IActionResult> GetStudents(int sectionId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();

            return Ok(_sectionAttendanceRepo.GetSudentsBySectionId(sectionId));
        }

        [HttpGet("CheckStudentInSection")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckStudentInSection([FromQuery] StudentSectionDTO studentSectionDTO)
        {
            var result = _sectionAttendanceRepo.CheckStudentInSection(studentSectionDTO);
            if(result)
                return Ok(result);
            return BadRequest();
        }
    }
}
