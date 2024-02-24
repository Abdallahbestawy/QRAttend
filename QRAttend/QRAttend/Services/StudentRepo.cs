using QRAttend.Models;
using QRAttend.Repositories;

namespace QRAttend.Services
{
    public class StudentRepo : IStudentRepo
    {
        QRContext context;

        public StudentRepo(QRContext _context)
        {
            context = _context;
        }
        public string AddToken(Student student)
        {
            if (student.Token == null)
            {
                string newToken = student.UniversityId + '1';
                student.Token = newToken;
                context.SaveChanges();
                return newToken;
            }else
            {
                int number = 0;
                string oldToken = student.Token;
                string numberPart = oldToken.Substring(student.UniversityId.Length);

                if (int.TryParse(numberPart, out number)) { }
                else
                {
                    throw new InvalidOperationException("Unable to parse the number part of the token.");
                }
                string newToken = $"{student.UniversityId}{number + 1}";
                student.Token = newToken;
                context.SaveChanges();
                return newToken;
            }
        }

        


        public void Create(Student student)
        {
            throw new NotImplementedException();
        }

        public void Delete(Student student)
        {
            throw new NotImplementedException();
        }

        public Student? GetByUnverstyId(string unverstyId)
        {
            var student = context.Students.FirstOrDefault(std=>std.UniversityId == unverstyId);
            if (student == null)
                return null;
            return student;
        }

        public void Update(Student student)
        {
            throw new NotImplementedException();
        }
    }
}
