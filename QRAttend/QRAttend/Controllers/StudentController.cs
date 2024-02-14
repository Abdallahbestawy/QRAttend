using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRAttend.Models;
using QRAttend.Repositories;
using QRAttend.Services;
using System.Text;
using System.Security.Cryptography;
using QRAttend.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace QRAttend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly QRContext context;
        private readonly IStudentRepo studentRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public StudentController(QRContext _context,IStudentRepo _studentRepo, UserManager<ApplicationUser> _userManager)
        {
            context = _context;
            studentRepo = _studentRepo;
            userManager = _userManager;
        }

        [HttpGet("isexist/{universityId}")]
        public IActionResult GetStudent(string universityId)
        {
            var student = context.Students.FirstOrDefault(s=>s.UniversityId == universityId);
            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpGet("AutoPrepare/{universityId}")]
        public IActionResult AutoPrepare(string universityId)
        {
            var student = context.Students.FirstOrDefault(s => s.UniversityId == universityId);

            if (student == null)
            {
                return NotFound();
            }

            if (student.Token != null)
            {
                return NotFound("Already Prepared");
            }

            string token = studentRepo.AddToken(student);

            // Hash the token using SHA-256

            // Optionally, you can store the hashed token in the database
            student.Token = token;
            context.SaveChanges();
            string hashedToken = HashToken(token);

            return Ok(hashedToken);
        }
        [Authorize]
        [HttpGet("ForcePrepare/{universityId}")]
        public IActionResult ForcePrepare(string universityId)
        {
            var student = context.Students.FirstOrDefault(s => s.UniversityId == universityId);

            if (student == null)
            {
                return NotFound();
            }

            string token = studentRepo.AddToken(student);

            // Hash the token using SHA-256

            // Optionally, you can store the hashed token in the database
            student.Token = token;
            context.SaveChanges();
            string hashedToken = HashToken(token);

            return Ok(hashedToken);
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

        [HttpPost("CheckToken")]
        //[Authorize]
        public async Task<IActionResult> CheckToken(AttendanceDto input)
        {
            //var currentUser = await userManager.GetUserAsync(User);
            //if (currentUser == null)
            //{
            //    return Unauthorized();
            //}
            var student = context.Students.FirstOrDefault(s => s.UniversityId == input.UniversityStudentId);

            if (student == null)
            {
                return NotFound();
            }
            if (input.token == HashToken(student.Token))
            {
                return Ok();
            }
            return BadRequest();
        }

    }
}
