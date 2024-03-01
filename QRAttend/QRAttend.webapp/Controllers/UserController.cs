using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRAttend.Dto;
using QRAttend.Models;
using QRAttend.Repositories;

namespace QRAttend.webapp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        private readonly IUsersRepo _usersRepo;
        private readonly IRoleService _roleService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICourseRepo _courseRepo;
        private readonly ISectionGroupRepo _sectionGroupRepo;

        public UserController(IUsersRepo usersRepo, IRoleService roleService, UserManager<ApplicationUser> userManager, ICourseRepo courseRepo, ISectionGroupRepo sectionGroupRepo)
        {
            _usersRepo = usersRepo;
            _roleService = roleService;
            _userManager = userManager;
            _courseRepo = courseRepo;
            _sectionGroupRepo = sectionGroupRepo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _usersRepo.GetAllUsersAsync());
        }

        public async Task<IActionResult> Add()
        {
            var roles = await _roleService.GetAllRoles();

            var viewModel = new AddUserDTO
            {
                Roles = (List<RoleDTO>)roles
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!model.Roles.Any(r => r.IsSelected))
            {
                ModelState.AddModelError("Roles", "Please select at least one role");
                return View(model);
            }

            if (await _userManager.FindByNameAsync(model.UserName) != null)
            {
                ModelState.AddModelError("UserName", "Username is already exists");
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Roles", error.Description);
                }

                return View(model);
            }

            await _userManager.AddToRolesAsync(user, model.Roles.Where(r => r.IsSelected).Select(r => r.Name));

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            var viewModel = new EditUserDTO
            {
                Id = userId,
                UserName = user.UserName
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
                return NotFound();

            var userWithSameUserName = await _userManager.FindByNameAsync(model.UserName);

            if (userWithSameUserName != null && userWithSameUserName.Id != model.Id)
            {
                ModelState.AddModelError("UserName", "This username is already assiged to another user");
                return View(model);
            }
            user.UserName = model.UserName;
            if(model.Password != "")
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetPassResult = await _userManager.ResetPasswordAsync(user, token, model.Password);
                if (!resetPassResult.Succeeded)
                {
                    foreach (var error in resetPassResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return View(model);
                }
            }
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ManageRoles(string userId)
        {
            var result = await _usersRepo.GetUserRolesAsync(userId);
            if (result == null)
                return NotFound();
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesDTO model)
        {
            var result = await _usersRepo.ChangeUserRoles(model);
            if (result == false)
                return NotFound();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetGroupsByCourseId(int courseId, string userId)
        {
            var result = await _sectionGroupRepo.GetAllSectionGroupByCourseId(courseId);
            var userGroups = await _sectionGroupRepo.GetAllSectionGroupByAssistantTeacherId(userId, courseId);

            if (result == null)
                result = new List<SectionGroupDTO>();
            if(userGroups != null)
            {
                foreach (var group in result)
                {
                    group.IsSelected = userGroups.Any(g => g.Id == group.Id);
                }
            }

            return Json(result);
        }


        public async Task<IActionResult> ManageGroups(string userId)
        {
            var result = await _courseRepo.GetCourseGroupsByAssistantTeacherId(userId);
            if (result == null)
                result = new List<CourseGroupsDTO>();
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageGroups(AddSectionGroupsForUserDTO model)
        {
            var result = await _sectionGroupRepo.AddUserToSectionGroups(model);
            if (result == false)
                return NotFound();
            return RedirectToAction(nameof(ManageGroups),new { userId = model.UserId });
        }
    }
}
