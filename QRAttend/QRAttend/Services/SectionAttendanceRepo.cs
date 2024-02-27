using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> CheckStudentInSection(StudentSectionDTO studentSectionDTO)
        {
            var stdudent = await _context.Students.FirstOrDefaultAsync(std => std.UniversityId == studentSectionDTO.UniversityId);
            if (stdudent == null) return false;

            var section = await _context.Sections.FirstOrDefaultAsync(sec=>sec.Id == studentSectionDTO.SectionId);
            if (section == null) return false;

            var result = await _context.StudentSections.AnyAsync(res=>res.SectionGroupId == section.SectionGroupId && res.StudentId == stdudent.Id);
            return result;
        }

        public async Task<int> Create(SectionAttendance attendance)
        {
            await _context.SectionAttendances.AddAsync(attendance);
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<List<SectionAttendanceDto>> GetBySectionId(int id)
        {
            var result = await _context.SectionAttendances.Where(attend => attend.SectionId == id).Select(attend => new SectionAttendanceDto
            {
                SectionId = attend.SectionId,
                UniversityStudentId =attend.Student.UniversityId
            }).ToListAsync();
            return result;
        }

        public async Task<List<StudentLecturesDTO>>? GetStudentSections(StudentSectionsByGroupDTO model)
        {
            var student = await _context.Students.FirstOrDefaultAsync(std => std.UniversityId == model.UniversityId);
            var sections = await _context.Sections.Where(sec=>sec.SectionGroupId == model.groupId).Select(sec=>sec.Id).ToListAsync();
            var attendances = await _context.SectionAttendances.Where(attend => attend.StudentId == student.Id && sections.Contains(attend.SectionId))
                .Select(attends => new StudentLecturesDTO
                {
                    Date = attends.CurrentDate,
                    Title = attends.Section.Title
                }).ToListAsync();
            if (attendances.Count == 0)
                return null;
            return attendances;
        }

        public async Task<List<StudentsDTO>>? GetSudentsBySectionId(int sectionId)
        {
            var students = await _context.SectionAttendances.Where(std => std.SectionId == sectionId).Select(std => new StudentsDTO
            {
                StudentId = std.Student.UniversityId,
                StudentName = std.Student.Name
            }).ToListAsync();
            if(students.Count > 0) 
                return students;
            return null;
        }

        public async Task<bool> IsExsit(SectionAttendance attendance)
        {
            return await _context.SectionAttendances.AnyAsync(attend => attend.SectionId == attendance.SectionId && attend.StudentId == attendance.StudentId);
        }
    }
}
