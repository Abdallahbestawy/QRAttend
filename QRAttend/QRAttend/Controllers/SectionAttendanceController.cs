using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRAttend.Dto;
using QRAttend.Models;
using QRAttend.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace QRAttend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionAttendanceController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISectionAttendanceRepo _sectionAttendance;
        private readonly IStudentRepo _studentRepo;
        private readonly ISectionGroupRepo _sectionGroupRepo;

        public SectionAttendanceController(UserManager<ApplicationUser> userManager, ISectionAttendanceRepo sectionAttendance, IStudentRepo studentRepo, ISectionGroupRepo sectionGroupRepo)
        {
            _userManager = userManager;
            _sectionAttendance = sectionAttendance;
            _studentRepo = studentRepo;
            _sectionGroupRepo = sectionGroupRepo;
        }

        [HttpPost("Create")]
        public IActionResult Create(SectionAttendanceDto sectionAttendance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var student = _studentRepo.GetByUnverstyId(sectionAttendance.UniversityStudentId);
            if (student == null)
            {
                return BadRequest("Student Not Exsit");
            }
            else if (sectionAttendance.Token != HashToken(student.Token))
            {
                return NotFound("Invalid Token");
            }
            var exsitInGroup = _sectionGroupRepo.CheckStudentInGroup(student.Id, sectionAttendance.SectionId);
            if(!exsitInGroup)
            {
                return BadRequest("Student Doesn't Exsit in this Group");
            }
            var isExsit = _sectionAttendance.IsExsit(new SectionAttendance
            {
                StudentId = student.Id,
                SectionId = sectionAttendance.SectionId
            });
            if(isExsit)
                return BadRequest("Attent is already Exsit");
            var result = _sectionAttendance.Create(new SectionAttendance
            {
                SectionId = sectionAttendance.SectionId,
                StudentId = student.Id,
                CurrentDate = DateTime.Now
            });
            if (result == 0)
                return BadRequest("Server Error");
            return Ok(sectionAttendance);
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
