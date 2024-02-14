using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRAttend.Dto;
using QRAttend.Models;
using QRAttend.Repositories;
using QRAttend.Settings;

namespace QRAttend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepo _courseRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseController(ICourseRepo courseRepo, UserManager<ApplicationUser> userManager)
        {
            _courseRepo = courseRepo;
            _userManager = userManager;
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Create(CourseDTO course)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }
            int done = await _courseRepo.Create(new Course { AcademicYearId =1,Name=course.Name,TeacherId = currentUser.Id});
            if (done == 0)
            {
                return BadRequest();
            }
            else
            {
                return Ok(course);
            }
        }

        [HttpGet("GetCourses")]
        public async Task<IActionResult> GetByTeacherId()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }
            List<Course> courses = await _courseRepo.GetByTeacherId(currentUser.Id);
            List<CourseDTO> result = new List<CourseDTO>();
            foreach (var course in courses)
            {
                result.Add(new CourseDTO { Name = course.Name,Id = course.Id });
            }
            if (result != null)
            {
                //return Ok(_response.CreateResponse(200, true, "Data Fetched Successfully", null, result));
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
