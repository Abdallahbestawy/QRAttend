using QRAttend.Models;

namespace QRAttend.Repositories
{
    public interface IStudentRepo
    {
        void Create(Student student);

        Task<Student>? GetByUnverstyId(string unverstyId);

        void Update(Student student);

        void Delete(Student student);

        Task<string> AddToken(Student student);
    }
}
