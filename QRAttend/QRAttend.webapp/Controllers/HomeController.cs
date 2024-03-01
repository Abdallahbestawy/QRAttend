using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRAttend.Dto;
using QRAttend.Models;
using QRAttend.webapp.Models;
using QRAttend.webapp.ViewModels;
using System.Diagnostics;

namespace QRAttend.webapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly QRContext _context;

        public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager, QRContext context)
        {
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var summaryData = await GetSummaryData(); // Replace with your method to fetch summary data
            return View(summaryData);
        }

        public async Task<DashboardSummaryViewModel> GetSummaryData()
        {
            // Assuming you have DbContext or any other data access mechanism (_context in this example)
            var totalCourses = await _context.Courses.CountAsync(c=>c.AcademicYear.IsCurrent == true);
            var currentAcademicYear = await _context.AcademicYears.FirstOrDefaultAsync(ay => ay.IsCurrent);
            var totalSections = await _context.Sections.CountAsync();
            var totalUsers = await _context.Users.CountAsync();

            return new DashboardSummaryViewModel
            {
                TotalCourses = totalCourses,
                CurrentAcademicYearName = currentAcademicYear?.Name,
                TotalSections = totalSections,
                TotalUsers = totalUsers
            };
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Roles()
        {
            var roles = _roleManager.Roles.Select(r => new RoleDTO
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();
            return View(roles);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
