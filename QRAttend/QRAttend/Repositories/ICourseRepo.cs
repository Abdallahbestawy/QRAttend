using QRAttend.Models;

namespace QRAttend.Repositories
{
    public interface ICourseRepo
    {
        Task<int> Create(Course course);

        Course GetById(int Id);

        Task<List<Course>> GetByTeacherId(string Id);

        void Update(Course course);

        void Delete(Course course);
    }
}
