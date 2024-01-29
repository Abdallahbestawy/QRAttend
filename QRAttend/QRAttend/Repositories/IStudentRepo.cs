using QRAttend.Models;

namespace QRAttend.Repositories
{
    public interface IStudentRepo
    {
        void Create(Student student);

        Student GetByUnverstyId(string unverstyId);

        void Update(Student student);

        void Delete(Student student);

        string AddToken(Student student);


    }
}
