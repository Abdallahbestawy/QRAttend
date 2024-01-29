using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRAttend.Models;
using QRAttend.Repositories;
using QRAttend.Services;

namespace QRAttend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly QRContext context;
        private readonly IStudentRepo studentRepo;

        public StudentController(QRContext _context,IStudentRepo _studentRepo)
        {
            context = _context;
            studentRepo = _studentRepo;
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
            if(student.Token != null)
            {
                return NotFound("Alredy Prepared");
            }

            string token = studentRepo.AddToken(student);

            return Ok(token);
        }
    }
}
