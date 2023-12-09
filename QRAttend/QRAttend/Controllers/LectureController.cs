using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRAttend.Dto;
using QRAttend.Models;
using System.Collections.Immutable;

namespace QRAttend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LectureController : ControllerBase
    {
        private readonly QRContext context;

        public LectureController(QRContext _context)
        {
            context = _context;
        }
        [HttpPost("postlecture")]
        public IActionResult PostLecture(LectureDto lectureDto)
        {
            if (ModelState.IsValid)
            {
                var lecture = new Lecture()
                {
                    Title = lectureDto.Title,
                    Date = DateTime.Now,
                    DoctorId=lectureDto.DoctorId,
                };
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
    }
}
