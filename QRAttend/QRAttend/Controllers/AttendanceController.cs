using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRAttend.Dto;
using QRAttend.Models;
using QRAttend.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace QRAttend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly QRContext context;
        private readonly IStudentRepo studentRepo;

        public AttendanceController(QRContext _context, IStudentRepo _studentRepo)
        {
            context = _context;
            this.studentRepo = _studentRepo;
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
                }else if (attendanceDto.token != HashToken(std.Token))
                {
                    return NotFound("Invalid Token");
                }

                bool attendanceExists = context.Attendances
                    .Any(a => a.LectureId == attendanceDto.LectureId && a.StudentId == std.Id);

                if (attendanceExists)
                {
                    return BadRequest("Attendance for this MacAddressStudent or StudentId already exists for the specified Lecture.");
                }

                var attendance = new Attendance()
                {
                    MacAddressStudent = "00:00:00:00:00:00",
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
        private string HashToken(string token)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));

                // Convert the hashed bytes to a string representation
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in hashedBytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }
    }


}
