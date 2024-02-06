using Microsoft.EntityFrameworkCore;
using QRAttend.Models;
using QRAttend.Repositories;

namespace QRAttend.Services
{
    public class CourseRepo : ICourseRepo
    {
        QRContext context;

        public CourseRepo(QRContext _context)
        {
            context = _context;
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

        public Course GetById(int Id)
        {
            return context.Courses.FirstOrDefault(c => c.Id == Id);
        }

        public async Task<List<Course>> GetByTeacherId(string Id)
        {
            return await context.Courses.Where(c => c.TeacherId == Id).ToListAsync();
        }

        public void Update(Course course)
        {
            throw new NotImplementedException();
        }
    }
}
