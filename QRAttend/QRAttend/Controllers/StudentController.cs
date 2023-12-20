using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRAttend.Models;

namespace QRAttend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly QRContext context;

        public StudentController(QRContext _context)
        {
            context = _context;
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
    }
}
