using QRAttend.Dto;
using QRAttend.Models;
using QRAttend.Repositories;
using System.Security.AccessControl;

namespace QRAttend.Services
{
    public class SectionAttendanceRepo : ISectionAttendanceRepo
    {
        private readonly QRContext _context;

        public SectionAttendanceRepo(QRContext context)
        {
            _context = context;
        }

        public bool CheckStudentInSection(StudentSectionDTO studentSectionDTO)
        {
            var stdudent = _context.Students.FirstOrDefault(std => std.UniversityId == studentSectionDTO.UniversityId);
            if (stdudent == null) return false;

            var section = _context.Sections.FirstOrDefault(sec=>sec.Id == studentSectionDTO.SectionId);
            if (section == null) return false;

            var result = _context.StudentSections.Any(res=>res.SectionGroupId == section.SectionGroupId && res.StudentId == stdudent.Id);
            return result;
        }

        public int Create(SectionAttendance attendance)
        {
            _context.SectionAttendances.Add(attendance);
            var result = _context.SaveChanges();
            return result;
        }

        public List<SectionAttendanceDto> GetBySectionId(int id)
        {
            var result = _context.SectionAttendances.Where(attend => attend.SectionId == id).Select(attend => new SectionAttendanceDto
            {
                SectionId = attend.SectionId,
                UniversityStudentId =attend.Student.UniversityId
            }).ToList();
            return result;
        }

        public List<StudentsDTO>? GetSudentsBySectionId(int sectionId)
        {
            var students = _context.SectionAttendances.Where(std => std.SectionId == sectionId).Select(std => new StudentsDTO
            {
                StudentId = std.Student.UniversityId,
                StudentName = std.Student.Name
            }).ToList();
            if(students.Count > 0) 
                return students;
            return null;
        }

        public bool IsExsit(SectionAttendance attendance)
        {
            return _context.SectionAttendances.Any(attend => attend.SectionId == attendance.SectionId && attend.StudentId == attendance.StudentId);
        }
    }
}
