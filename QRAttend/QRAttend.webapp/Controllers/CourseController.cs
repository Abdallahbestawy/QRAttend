using Microsoft.AspNetCore.Authorization;
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

        public CourseController(ICourseRepo courseRepo, UserManager<ApplicationUser> userManager, IAcademicYearRepo academicYearRepo, ISectionGroupRepo sectionGroupRepo)
        {
            _courseRepo = courseRepo;
            _userManager = userManager;
            _academicYearRepo = academicYearRepo;
            _sectionGroupRepo = sectionGroupRepo;
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
    }
}
