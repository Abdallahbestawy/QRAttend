using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRAttend.Dto;
using QRAttend.Models;
using QRAttend.Repositories;
using QRAttend.Services;

namespace QRAttend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "AssistantTeacher")]
    public class AssistantTeacherController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICourseRepo _courseRepo;
        private readonly ISectionGroupRepo _sectionGroupRepo;

        public AssistantTeacherController(UserManager<ApplicationUser> userManager, ICourseRepo courseRepo, ISectionGroupRepo sectionGroupRepo)
        {
            _userManager = userManager;
            _courseRepo = courseRepo;
            _sectionGroupRepo = sectionGroupRepo;
        }

        [HttpGet("GetCourses")]
        public async Task<IActionResult> GetCourses()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();
            var courses = _courseRepo.GetByAssistantTeacherId(currentUser.Id);
            List<CourseDTO> result = new List<CourseDTO>();
            foreach (var course in courses)
            {
                result.Add(new CourseDTO { Name = course.Name, Id = course.Id });
            }
            return Ok(result);
        }

        [HttpGet("GetGroups/{courseId}")]
        public async Task<IActionResult> GetGroups(int courseId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();
            var groups = _sectionGroupRepo.GetAllSectionGroupByAssistantTeacherId(currentUser.Id,courseId);
            if(groups == null)
                return BadRequest();
            List<SectionGroupDTO> result = new List<SectionGroupDTO>();
            foreach (var group in groups)
            {
                result.Add(new SectionGroupDTO { Id = group.Id, Name = group.Name });
            }
            return Ok(result);
        }
    }
}
