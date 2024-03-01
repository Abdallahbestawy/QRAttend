using QRAttend.Dto;
using QRAttend.Models;

namespace QRAttend.Repositories
{
    public interface ICourseRepo
    {
        Task<int> Create(Course course);
        Task<Course> GetById(int Id);
        Task<List<CourseDTO>>? GetAllCourses();
        Task<List<Course>> GetByTeacherId(string Id);
        void Update(Course course);
        void Delete(Course course);
        Task<List<Course>> GetByAssistantTeacherId(string Id);
        Task<List<CourseGroupsDTO>>? GetCourseGroupsByAssistantTeacherId(string id);
    }
}
