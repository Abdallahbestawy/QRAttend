using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRAttend.Dto;
using QRAttend.Models;
using QRAttend.Repositories;
using QRAttend.Services;

namespace QRAttend.webapp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CourseController : Controller
    {
        private readonly ICourseRepo _courseRepo;
        private readonly IAcademicYearRepo _academicYearRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISectionGroupRepo _sectionGroupRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CourseController(ICourseRepo courseRepo, UserManager<ApplicationUser> userManager, IAcademicYearRepo academicYearRepo, ISectionGroupRepo sectionGroupRepo, IWebHostEnvironment webHostEnvironment)
        {
            _courseRepo = courseRepo;
            _userManager = userManager;
            _academicYearRepo = academicYearRepo;
            _sectionGroupRepo = sectionGroupRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _courseRepo.GetAllCourses();
            return View(result);
        }

        public IActionResult Add ()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddCourseDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }
            var currentAcademicYear = await _academicYearRepo.GetCurrentOne();
            int done = await _courseRepo.Create(new Course { AcademicYearId = currentAcademicYear.Id, Name = model.Name, TeacherId = currentUser.Id });
            if (done == 0)
                return View(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ManageGroups(int id)
        {
            if(await _courseRepo.GetById(id) == null)
                return NotFound();
            var result = await _sectionGroupRepo.GetAllSectionGroupByCourseId(id);
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGroup(SectionGroupDTO sectionGroup)
        {
            var isexist = _courseRepo.GetById(sectionGroup.CourseId);
            if (isexist == null)
                return NotFound();
            var result = await _sectionGroupRepo.CreateGroup(new SectionGroup
            {
                Name = sectionGroup.Name,
                CourseId = sectionGroup.CourseId
            });
            if (result == 0)
                return BadRequest();

            return RedirectToAction(nameof(ManageGroups), new { id = sectionGroup.CourseId});
        }

        public async Task<IActionResult> ManageGroupStudents(int Id)
        {
            var result = await _sectionGroupRepo.GetStudetsBySectionGroupId(Id);
            if (result == null)
                result = new SectionGroupStudentsDTO();
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageGroupStudents(AddSectiongroupStudentsFromExcelDTO model)
        {
            if (ModelState.IsValid)
            {
                // Check if a file is uploaded
                if (model.File != null && model.File.Length > 0)
                {
                    var filePath = SaveFile(model.File);

                    var studentsList = ExcelHelper.Import<StudentExcelDTO>(filePath);

                    var result = await _sectionGroupRepo.AddListOfStudentsInSectionGroup(model.GroupId, studentsList);
                    if (result)
                    {
                        TempData["SuccessMessage"] = "Students added successfully.";
                        return RedirectToAction(nameof(ManageGroupStudents), new { id = model.GroupId });
                    }
                }
                else
                {
                    // Handle case when no file is uploaded
                    ModelState.AddModelError("File", "Please select a file.");
                }
            }
            return RedirectToAction(nameof(ManageGroupStudents), new { id = model.GroupId });
        }


        // save the uploaded file into wwwroot/uploads folder
        private string SaveFile(IFormFile file)
        {
            if (file.Length == 0)
            {
                throw new BadHttpRequestException("File is empty.");
            }

            var extension = Path.GetExtension(file.FileName);

            var webRootPath = _webHostEnvironment.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var folderPath = Path.Combine(webRootPath, "uploads");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = $"{Guid.NewGuid()}.{extension}";
            var filePath = Path.Combine(folderPath, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);

            return filePath;
        }
    }
}
