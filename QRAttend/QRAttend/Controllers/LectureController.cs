using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRAttend.Dto;
using QRAttend.Models;
using System.Collections.Immutable;

namespace QRAttend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class LectureController : ControllerBase
    {
        private readonly QRContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public LectureController(QRContext _context, UserManager<ApplicationUser> _userManager)
        {
            context = _context;
            userManager = _userManager;
        }
        [HttpPost("postlecture")]
        public async Task<IActionResult> PostLecture(LectureDto lectureDto)
        {
            if (ModelState.IsValid)
            {
                var currentUser=await userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return Unauthorized();
                }
                var lecture = new Lecture()
                {
                    Title = lectureDto.Title,
                    Date = DateTime.Now,
                };
                lecture.DoctorId = currentUser.Id;  
                context.Lectures.Add(lecture);
                try
                {
                    context.SaveChanges();
                    ///string url = Url.Link(nameof(PostLecture), new { Id = lecture.Id });
                    //return Created(url, lecture);
                    return Ok();
                   
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
                }
            }
            return BadRequest("Lecture Object is not Valid !!!");
        }
        [HttpGet]
        public async Task<IActionResult> GetLecture()
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            List<Lecture> lectures = context.Lectures
                .Where(d => d.DoctorId == currentUser.Id)
                .ToList();

            if (lectures.Count == 0)
            {
                return NotFound("No lectures found for the current user.");
            }

            List<LectureDto> lectureDtos = lectures
                .Select(lec => new LectureDto { Title = lec.Title,LectureId=lec.Id})
                .ToList();

            return Ok(lectureDtos);
        }

        [HttpGet("{Id:int}")]
        public IActionResult LectureDetails(int Id)
        {
            var lecture = context.Lectures
                .Include(l => l.Attendances)
                .ThenInclude(a => a.Student)
                .FirstOrDefault(l => l.Id == Id);

            if (lecture == null)
            {
                return NotFound("No lecture found with the given ID.");
            }

            var lectureDetails = new LectureDetailsDto
            {
                LectureTitle = lecture.Title,
                StudentName = lecture.Attendances.Select(a => a.Student.Name).ToList(),
                StudentUniversityId = lecture.Attendances.Select(a => a.Student.UniversityId).ToList()
            };

            return Ok(lectureDetails);
        }

    }
}
