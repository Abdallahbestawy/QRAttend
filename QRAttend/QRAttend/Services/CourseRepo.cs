using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QRAttend.Dto;
using QRAttend.Models;
using QRAttend.Repositories;

namespace QRAttend.Services
{
    public class CourseRepo : ICourseRepo
    {
        private readonly QRContext context;
        private readonly ISectionGroupRepo _sectionGroupRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseRepo(QRContext _context, ISectionGroupRepo sectionGroupRepo, UserManager<ApplicationUser> userManager)
        {
            context = _context;
            _sectionGroupRepo = sectionGroupRepo;
            _userManager = userManager;
        }

        public async Task<int> Create(Course course)
        {
            await context.Courses.AddAsync(course);
            return context.SaveChanges();
        }

        public void Delete(Course course)
        {
            context.Courses.Remove(course);
        }

        public async Task<Course> GetById(int Id)
        {
            return await context.Courses.FirstOrDefaultAsync(c => c.Id == Id);
        }

        public async Task<List<Course>> GetByTeacherId(string Id)
        {
             return await context.Courses.Where(c => c.TeacherId == Id && c.AcademicYear.IsCurrent == true).ToListAsync();
        }

        public async Task<List<Course>> GetByAssistantTeacherId(string Id)
        {
            var assistantGroups = await context.AssistantTeacherSections.Where(assist => assist.TeacherId == Id).Select(grp=> grp.SectionGroupId).ToListAsync();

            var courses = await context.SectionGroups.Where(sec => assistantGroups.Contains(sec.Id) && sec.Course.AcademicYear.IsCurrent == true)
                .Select(grp => grp.Course).Distinct().ToListAsync();

            return courses;
        }

        public void Update(Course course)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CourseDTO>>? GetAllCourses()
        {
            var courses = await context.Courses.Where(c => c.AcademicYear.IsCurrent == true).Select(course => new CourseDTO
            {
                Id = course.Id,
                Name = course.Name
            }).ToListAsync();
            return courses;
        }

        public async Task<List<CourseGroupsDTO>>? GetCourseGroupsByAssistantTeacherId(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var courses = await GetByAssistantTeacherId(id);
            var result = new List<CourseGroupsDTO>();
            if (courses.Count == 0)
            {
                result.Add(new CourseGroupsDTO
                {
                    CourseName = "0000",
                    AddGroupForUser = new AddSectionGroupsForUserDTO()
                });
                var allCoursess = await GetAllCourses();
                var allGroupss = await _sectionGroupRepo.GetAllSectionGroupByCourseId(allCoursess.FirstOrDefault().Id);
                result.FirstOrDefault().AddGroupForUser.Courses = allCoursess;
                if (allGroupss != null)
                {
                    result.FirstOrDefault().AddGroupForUser.SectionGroups = allGroupss;
                    result.FirstOrDefault().AddGroupForUser.UserId = id;
                    result.FirstOrDefault().AddGroupForUser.UserName = user.UserName;
                    result.FirstOrDefault().AddGroupForUser.CourseId = allCoursess.FirstOrDefault().Id;
                }
                else
                {
                    result.FirstOrDefault().AddGroupForUser.SectionGroups = new List<SectionGroupDTO>();
                }
                return result;
            }
            foreach (var course in courses)
            {
                var groups = await _sectionGroupRepo.GetAllSectionGroupByAssistantTeacherId(id, course.Id);
                result.Add(new CourseGroupsDTO
                {
                    CourseName = course.Name,
                    SectionGroups = groups,
                    AddGroupForUser = new AddSectionGroupsForUserDTO()
                });
            }
            var allCourses = await GetAllCourses();
            var allGroups = await _sectionGroupRepo.GetAllSectionGroupByCourseId(courses.FirstOrDefault().Id);
            var groupsForFirstcourse = await _sectionGroupRepo.GetAllSectionGroupByAssistantTeacherId(id, courses.FirstOrDefault().Id);

            foreach (var group in allGroups)
            {
                var isSelected = groupsForFirstcourse.Any(g => g.Id == group.Id);
                group.IsSelected = isSelected;
            }

            result.FirstOrDefault().AddGroupForUser.Courses = allCourses;
            if(allGroups != null)
            {
                result.FirstOrDefault().AddGroupForUser.UserId = id;
                result.FirstOrDefault().AddGroupForUser.UserName = user.UserName;
                result.FirstOrDefault().AddGroupForUser.CourseId = courses.FirstOrDefault().Id;
                result.FirstOrDefault().AddGroupForUser.SectionGroups = allGroups;
            }else
            {
                result.FirstOrDefault().AddGroupForUser.SectionGroups = new List<SectionGroupDTO>();
            }
            return result;
        }
    }
}
