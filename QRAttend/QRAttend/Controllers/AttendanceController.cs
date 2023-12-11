using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRAttend.Dto;
using QRAttend.Models;

namespace QRAttend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly QRContext context;

        public AttendanceController(QRContext _context)
        {
            context = _context;
        }
        [HttpPost("postattendance")]
        public IActionResult PostAttendance(AttendanceDto attendanceDto)
        {
            if (ModelState.IsValid)
            {
                var std = context.Students.Where(s => s.UniversityId == attendanceDto.UniversityStudentId).FirstOrDefault();
                if (std == null)
                {
                    return NotFound("Student not found with the specified UniversityId.");
                }
            
                bool attendanceExists = context.Attendances
                 .Any(a => a.LectureId == attendanceDto.LectureId &&
                 a.MacAddressStudent == attendanceDto.MacAddressStudent ||
                a.StudentId == std.Id);

                if (attendanceExists)
                {
                    return BadRequest("Attendance for this MacAddressStudent or StudentId already exists for the specified Lecture.");
                }

                var attendance = new Attendance()
                {
                    MacAddressStudent = attendanceDto.MacAddressStudent,
                    CurrentDate = DateTime.Now,
                    LectureId = attendanceDto.LectureId,
                    StudentId = std.Id
                };
                context.Attendances.Add(attendance);
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
            return BadRequest("Attendance Object is not Valid !!!");
        }
    }


}
